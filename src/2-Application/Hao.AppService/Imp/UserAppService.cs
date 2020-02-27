using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Entity;
using Hao.EventData;
using Hao.File;
using Hao.Library;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class UserAppService : ApplicationService, IUserAppService
    {

        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;


        private readonly ISysLoginRecordRepository _recordRep;

        private readonly AppSettingsInfo _appsettings;

        public FilePathInfo PathInfo { get; set; }

        private readonly ICurrentUser _currentUser;

        public UserAppService(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, ISysUserRepository userRepository, ISysLoginRecordRepository recordRep, IMapper mapper,ICurrentUser currentUser)
        {
            _userRep = userRepository;
            _recordRep = recordRep;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;
            _currentUser = currentUser;
        }

        public async Task<UserOut> GetByID(long? id)
        {
            var user = await _userRep.GetAysnc(id.Value);

            return _mapper.Map<UserOut>(user);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<LoginOut> Login(UserQuery query)
        {
            var users = await _userRep.GetListAysnc(query);
            if (users.Count == 0)
                throw new HException(ErrorInfo.E005005, nameof(ErrorInfo.E005005).GetErrorCode());
            var user = users.FirstOrDefault();
            return _mapper.Map<LoginOut>(user);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task<long> AddUser(UserIn vm)
        {
            var user = _mapper.Map<SysUser>(vm);
            user.FirstNameSpell = HSpell.GetFirstLetter(user.Name.ToCharArray()[0]);
            user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.KeyInfo.Sha256Key);
            user.Enabled = true;
            return await _userRep.InsertAysnc(user);
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task AddUsers(List<UserIn> vms)
        {
            var users = _mapper.Map<List<SysUser>>(vms);
            foreach (var user in users)
            {
                user.FirstNameSpell = HSpell.GetFirstLetter(user.Name.ToCharArray()[0]);
                user.Password = EncryptProvider.HMACSHA256(user.Password, _appsettings.KeyInfo.Sha256Key);
            }
            await _userRep.InsertAysnc(users);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<UserOut>> GetUsers(UserQuery query)
        {
            var users = await _userRep.GetPagedListAysnc(query);
            var result = _mapper.Map<PagedList<UserOut>>(users);

            return result;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserOut> GetUser(long id)
        {
            var user = await _userRep.GetAysnc(id);
            return _mapper.Map<UserOut>(user);
        }

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<CurrentUserOut> GetCurrentUser()
        {
            var user = await _userRep.GetAysnc(_currentUser.Id);
            return _mapper.Map<CurrentUserOut>(user);
        }

        /// <summary>
        /// 更新用户登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task UpdateLogin(long userId, DateTime lastLoginTime, string ip)
        {
            var user = await _userRep.GetAysnc(userId);
            await UpdateLogin(user, lastLoginTime, ip);
        }
        
        [UseTransaction] //注意，事务命令只能用于 insert、delete、update 操作，而其他命令，比如建表、删表，会被自动提交。
        private async Task UpdateLogin(SysUser user, DateTime lastLoginTime, string ip)
        {
            if (user != null)
            {
                user.LastLoginTime = lastLoginTime;
                user.LastLoginIP = ip;
                try
                {
                    await _userRep.UpdateAsync(user, user => new { user.LastLoginTime, user.LastLoginIP });
                    await _recordRep.InsertAysnc(new SysLoginRecord() { UserId = user.Id, IP = ip, Time = lastLoginTime });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteUser(long userId)
        {
            await _userRep.DeleteAysnc(userId);
        }

        /// <summary>
        /// 注销/启用，用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateUserEnabled(long userId, bool enabled)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user != null)
            {
                user.Enabled = enabled;
                await _userRep.UpdateAsync(user, user => new {user.Enabled});
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        public async Task EditUser(long userId, UserIn vm)
        {
            var user = await _userRep.GetAysnc(userId);
            if (user != null)
            {
                user.Name = vm.Name;
                user.Age = vm.Age;
                user.Gender = vm.Gender;
                user.Phone = vm.Phone;
                user.Email = vm.Email;
                user.WeChat = vm.WeChat;
                await _userRep.UpdateAsync(user,
                    user => new {user.Name, user.Age, user.Gender, user.Phone, user.Email, user.WeChat});
            }
        }

        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> ExportUsers(UserQuery query)
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
            string rootPath = PathInfo.ExportExcelPath;

            HFile.CreateDirectory(rootPath);
            string filePath = Path.Combine(rootPath, $"{fileName}");

            await HFile.ExportToExcelEPPlus(filePath, exportData);

            return fileName;
        }
    }
}
