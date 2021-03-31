using DotNetCore.CAP;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.EventData;
using Hao.Excel;
using Hao.Library;
using Hao.Model;
using Hao.Utility;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hao.Runtime;
using Mapster;
using ToolGood.Words;
using Hao.Service;

namespace Hao.AppService
{
    /// <summary>
    /// 用户应用服务
    /// </summary>
    public class UserAppService : ApplicationService, IUserAppService
    {

        private readonly AppSettings _appSettings;

        private readonly IUserRepository _userRep;

        private readonly IRoleRepository _roleRep;

        private readonly ICurrentUser _currentUser;

        private readonly ICapPublisher _publisher;

        private readonly ITimeLimitedDataProtector _protector;

        private readonly IUserDomainService _userDomainService;

        private readonly IRoleDomainService _roleDomainService;

        public UserAppService(IRoleRepository roleRep,
            IOptionsSnapshot<AppSettings> appSettingsOptions,
            IUserRepository userRepository,
            ICurrentUser currentUser,
            ICapPublisher publisher,
            IDataProtectionProvider provider, IUserDomainService userDomainService, IRoleDomainService roleDomainService)
        {
            _userRep = userRepository;
            _appSettings = appSettingsOptions.Value;
            _currentUser = currentUser;
            _roleRep = roleRep;
            _protector = provider.CreateProtector(appSettingsOptions.Value.DataProtectorPurpose.FileDownload).ToTimeLimitedDataProtector();
            _publisher = publisher;
            _userDomainService = userDomainService;
            _roleDomainService = roleDomainService;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("UserAppService_AddUser")]
        public async Task Add(UserAddInput input)
        {
            var user = input.Adapt<SysUser>();
            user.FirstNameInitial = WordsHelper.GetFirstPinyin(user.Name.Substring(0, 1));
            user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(user.Password);
            user.Password = H_EncryptProvider.HMACSHA256(user.Password, _appSettings.Key.Sha256Key);
            user.Enabled = true;

            var role = await _roleDomainService.Get(input.RoleId.Value);
            user.RoleId = role.Id;
            user.RoleName = role.Name;
            user.AuthNumbers = role.AuthNumbers;
            user.RoleLevel = role.Level;

            await _userDomainService.Add(user);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<Paged<UserOutput>> GetPaged(UserQueryInput queryInput)
        {
            var query = queryInput.Adapt<UserQuery>();

            query.CurrentRoleLevel = _currentUser.RoleLevel;

            var users = await _userRep.GetPagedAsync(query);
            var result = users.Adapt<Paged<UserOutput>>();

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserDetailOutput> Get(long id)
        {
            var user = await _userDomainService.Get(id);

            return user.Adapt<UserDetailOutput>();
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [CapUnitOfWork]
        public async Task Delete(long userId)
        {
            _userDomainService.CheckUser(userId);
            var user = await _userDomainService.Get(userId);

            await _userRep.DeleteAsync(user);

            await _publisher.PublishAsync(nameof(LogoutEventData), new LogoutEventData
            {
                UserIds = new List<long> { userId }
            });
        }

        /// <summary>
        /// 注销/启用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        [CapUnitOfWork]
        public async Task UpdateStatus(long userId, bool enabled)
        {
            _userDomainService.CheckUser(userId);
            var user = await _userDomainService.Get(userId);
            user.Enabled = enabled;
            await _userRep.UpdateAsync(user, user => new { user.Enabled });
            // 注销用户，删除登录缓存
            if (!enabled)
            {
                await _publisher.PublishAsync(nameof(LogoutEventData), new LogoutEventData
                {
                    UserIds = new List<long> { userId },
                    PublishUser = _currentUser as CurrentUser
                });
            }
        }

        /// <summary>
        /// 更新编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Update(long userId, UserUpdateInput input)
        {
            _userDomainService.CheckUser(userId);
            var user = await _userDomainService.Get(userId);

            user = input.Adapt(user);

            await _userRep.UpdateAsync(user,
                user => new { user.Name, user.Birthday, user.Gender, user.Phone, user.Email, user.WeChat, user.QQ, user.RoleId, user.RoleName });
        }

        /// <summary>
        /// 是否存在账号
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAccount(string account)
        {
            var query = new UserQuery { Account = account };
            var users = await _userRep.GetListAsync(query);
            return users.Count > 0;
        }


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<UserExcelOutput> Export(UserQueryInput queryInput)
        {
            var query = queryInput.Adapt<UserQuery>();
            var users = await _userRep.GetListAsync(query);

            var exportData = users.Select(a => new Dictionary<string, string>{
                {"姓名",a.Name},
                {"性别", a.Gender.ToDescription() },
                {"出生日期",a.Birthday.ToDateTimeString()},
                {"手机号",a.Phone},
                {"邮箱",a.Email},
                {"微信",a.WeChat},
                {"状态",a.Enabled.IsTrue()?"启用":"注销"},
                {"最后登录时间",a.LastLoginTime.ToDateTimeString()},
                {"最后登录地点",a.LastLoginIP}
            });

            string fileName = $"{Guid.NewGuid()}.xlsx";
            string rootPath = _appSettings.FilePath.ExportExcelPath;

            H_File.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await H_Excel.ExportByEPPlus(filePath, exportData);

            return new UserExcelOutput { FileName = fileName, FileId = _protector.Protect(fileName.Split('.')[0], TimeSpan.FromSeconds(5)) };
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task Import(IFormFileCollection files)
        {
            H_AssertEx.That(files == null || files.Count == 0, "请选择Excel文件");

            //格式限制
            var allowType = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            H_AssertEx.That(files.Any(b => !allowType.Contains(b.ContentType)), "只能上传Excel文件");
            ////大小限制
            //if (files.Sum(b => b.Length) >= 1024 * 1024 * 4)
            //{

            //}

            var users = new List<SysUser>();

            foreach (IFormFile file in files)
            {
                string rootPath = _appSettings.FilePath.ImportExcelPath;

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
                            user.FirstNameInitial = WordsHelper.GetFirstPinyin(user.Name.Substring(0, 1));
                            user.Password = H_EncryptProvider.HMACSHA256("123456", _appSettings.Key.Sha256Key);
                            users.Add(user);
                        }
                    }
                }
            }

            if (users.Count == 0) return;

            await _userRep.InsertAsync(users);
        }
    }
}
