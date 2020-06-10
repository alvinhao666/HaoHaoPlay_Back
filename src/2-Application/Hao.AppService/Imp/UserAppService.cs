using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.Excel;
using Hao.File;
using Hao.Library;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Npgsql;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 用户应用服务
    /// </summary>
    public partial class UserAppService : ApplicationService, IUserAppService
    {

        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        private readonly ISysRoleRepository _roleRep;

        private readonly AppSettingsConfig _appsettings;

        private readonly ICurrentUser _currentUser;

        private readonly ITimeLimitedDataProtector _protector;

        public UserAppService(ISysRoleRepository roleRep,
            IOptionsSnapshot<AppSettingsConfig> appsettingsOptions, 
            ISysUserRepository userRepository,
            ISysLoginRecordRepository recordRep, 
            IMapper mapper,
            ICurrentUser currentUser,
            IDataProtectionProvider provider)
        {
            _userRep = userRepository;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;
            _currentUser = currentUser;
            _roleRep = roleRep;
            _protector = provider.CreateProtector(appsettingsOptions.Value.DataProtectorPurpose.FileDownload).ToTimeLimitedDataProtector();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task AddUser(UserAddRequest vm)
        {
            using (var redisLock = RedisHelper.Lock($"{_appsettings.RedisPrefix.Lock}UserAppService_AddUser", 10))
            {
                H_Check.InspectRedisLock(redisLock);

                var users = await _userRep.GetAllAysnc(new UserQuery()
                {
                    LoginName = vm.LoginName
                });
                if (users.Count > 0) throw new H_Exception("账号已存在，请重新输入");

                var role = await _roleRep.GetAysnc(vm.RoleId.Value);
                if (role == null) throw new H_Exception("角色不存在，请重新选择");
                if (role.IsDeleted) throw new H_Exception("角色已删除，请重新选择");
                if (role.Level <= _currentUser.RoleLevel) throw new H_Exception("无法添加同级及高级角色用户");

                var user = _mapper.Map<SysUser>(vm);
                user.FirstNameSpell = H_Spell.GetFirstLetter(user.Name.ToCharArray()[0]);
                user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(user.Password);
                user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.Key.Sha256Key);
                user.Enabled = true;
                user.RoleId = role.Id;
                user.RoleName = role.Name;
                user.AuthNumbers = role.AuthNumbers;
                user.RoleLevel = role.Level;

                role.UserCount = role.UserCount.HasValue ? ++role.UserCount : 1; 
                try
                {
                    await _userRep.InsertAysnc(user);

                    await _roleRep.UpdateAsync(role, a => new { a.UserCount });
                }
                catch (PostgresException ex)
                {
                    if (ex.SqlState == PostgresSqlState.E23505) throw new H_Exception("账号已存在，请重新输入");//违反唯一键
                }
            } 
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserVM>> GetUserPagedList(UserQuery query)
        {
            query.CurrentRoleLevel = _currentUser.RoleLevel; //只能获取角色等级低用户

            var users = await _userRep.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserVM>>(users);

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailVM> GetUser(long id)
        {
            var user = await GetUserDetail(id);
            return _mapper.Map<UserDetailVM>(user);
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task DeleteUser(long userId)
        {
            using (var redisLock = RedisHelper.Lock($"{_appsettings.RedisPrefix.Lock}UserAppService_DeleteUser", 10))
            {
                H_Check.InspectRedisLock(redisLock);

                CheckUser(userId);
                var user = await GetUserDetail(userId);

                var role = await _roleRep.GetAysnc(user.RoleId.Value);
                role.UserCount--;

                await _userRep.DeleteAysnc(user.Id);               
                await _roleRep.UpdateAsync(role, a => new { a.UserCount });

                await RedisHelper.DelAsync(_appsettings.RedisPrefix.Login + userId);
            }
        }

        /// <summary>
        /// 注销/启用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        public async Task UpdateUserStatus(long userId, bool enabled)
        {
            CheckUser(userId);
            var user = await GetUserDetail(userId);
            user.Enabled = enabled;
            await _userRep.UpdateAsync(user, user => new { user.Enabled });
            // 注销用户，删除登录缓存
            if(!enabled) await RedisHelper.DelAsync(_appsettings.RedisPrefix.Login + user.Id);
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserUpdateRequest vm)
        {
            CheckUser(userId);
            var user = await GetUserDetail(userId);
            user.Name = vm.Name;
            user.Age = vm.Age;
            user.Gender = vm.Gender;
            user.Phone = vm.Phone;
            user.Email = vm.Email;
            user.WeChat = vm.WeChat;
            user.QQ = vm.QQ;
            await _userRep.UpdateAsync(user,
                user => new { user.Name, user.Age, user.Gender, user.Phone, user.Email, user.WeChat, user.QQ, user.RoleId, user.RoleName });
        }

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<bool> IsExistUser(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);
            return users.Count > 0;
        }


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<UserExcelVM> ExportUser(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);

            var exportData = users.Select(a => new Dictionary<string, string>{
                {"姓名",a.Name},
                {"性别", a.Gender.GetDescription() },
                {"年龄",a.Age.ToString()},
                {"手机号",a.Phone},
                {"邮箱",a.Email},
                {"微信",a.WeChat},
                {"状态",a.Enabled.IsTrue()?"启用":"注销"},
                {"最后登录时间",a.LastLoginTime.ToDateString()},
                {"最后登录地点",a.LastLoginIP}
            });

            string fileName = $"{Guid.NewGuid()}.xlsx";
            string rootPath = _appsettings.FilePath.ExportExcelPath;

            H_File.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await H_Excel.ExportToExcelEPPlus(filePath, exportData);

            return new UserExcelVM { FileName = fileName, FileId = _protector.Protect(fileName.Split('.')[0], TimeSpan.FromSeconds(5)) };
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task ImportUser(IFormFileCollection files)
        {
            if (files == null || files.Count == 0) throw new H_Exception("请选择Excel文件");

            //格式限制
            var allowType = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            if (files.Any(b => !allowType.Contains(b.ContentType))) throw new H_Exception("只能上传Excel文件");


            ////大小限制
            //if (files.Sum(b => b.Length) >= 1024 * 1024 * 4)
            //{

            //}

            var users = new List<SysUser>();

            foreach (IFormFile file in files)
            {
                string rootPath = _appsettings.FilePath.ImportExcelPath;

                H_File.CreateDirectory(rootPath);

                string filePath = Path.Combine(rootPath, file.FileName);

                using (var fs = System.IO.File.Create(filePath))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }

                using (var ep = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = ep.Workbook.Worksheets[0];
                    if (worksheet != null && worksheet.Cells[1, 1].Text.Trim() != "姓名")
                    {
                        throw new H_Exception("上传数据列名有误，请检查");
                    }
                    foreach (var ws in ep.Workbook.Worksheets)
                    {
                        int colStart = ws.Dimension.Start.Column;  //工作区开始列,start=1
                        int colEnd = ws.Dimension.End.Column;       //工作区结束列
                        int rowStart = ws.Dimension.Start.Row;       //工作区开始行号,start=1
                        int rowEnd = ws.Dimension.End.Row;       //工作区结束行号

                        for (int i = rowStart + 1; i <= rowEnd; i++) //第1行是列名,跳过
                        {
                            var user = new SysUser();
                            user.Name = ws.Cells[i, colStart].Text;
                            user.FirstNameSpell = H_Spell.GetFirstLetter(user.Name.ToCharArray()[0]);
                            user.Password = EncryptProvider.HMACSHA256("123456", _appsettings.Key.Sha256Key);
                            users.Add(user);
                        }
                    }
                }
            }

            if (users.Count == 0) return;

            await _userRep.InsertAysnc(users);
        }

        #region private
        /// <summary>
        /// 检测用户
        /// </summary>
        /// <param name="userId"></param>
        private void CheckUser(long userId)
        {
            if (userId == _currentUser.Id) throw new H_Exception("无法操作当前登录用户");
            if (userId == -1) throw new H_Exception("无法操作系统管理员账户");
        }

        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<SysUser> GetUserDetail(long userId)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user == null) throw new H_Exception("用户不存在");
            if (user.IsDeleted) throw new H_Exception("用户已删除");
            if (user.RoleLevel <= _currentUser.RoleLevel) throw new H_Exception("无法操作同级及高级角色用户");
            return user;
        }
        #endregion
    }
}
