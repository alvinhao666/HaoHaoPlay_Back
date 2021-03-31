using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.Library;
using Hao.Model;
using Hao.Runtime;
using Hao.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 用户领域服务
    /// </summary>
    public class UserDomainService : DomainService, IUserDomainService
    {
        private readonly IUserRepository _userRep;

        private readonly ICurrentUser _currentUser;

        private readonly AppSettings _appSettings;


        public UserDomainService(IUserRepository userRep, ICurrentUser currentUser, IOptionsSnapshot<AppSettings> appSettingsOptions)
        {
            _userRep = userRep;
            _currentUser = currentUser;
            _appSettings = appSettingsOptions.Value;
        }


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Add(SysUser user)
        {
            try
            {
                var users = await _userRep.GetAllAsync(new UserQuery { Account = user.Account });

                H_AssertEx.That(users.Count > 0, "账号已存在");


                H_AssertEx.That(user.RoleLevel <= _currentUser.RoleLevel, "无法添加同级及高级角色用户");

                await _userRep.InsertAsync(user);
            }
            catch (PostgresException ex)
            {
                H_AssertEx.That(ex.SqlState == H_PostgresSqlState.E23505, "账号已存在");
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SysUser> Get(long userId)
        {
            var user = await _userRep.GetAsync(userId);
            H_AssertEx.That(user == null, "用户不存在");
            H_AssertEx.That(user.IsDeleted, "用户已删除");
            H_AssertEx.That(user.RoleLevel <= _currentUser.RoleLevel, "无法操作同级及高级角色用户");

            return user;
        }

        /// <summary>
        /// 检测用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void CheckUser(long userId)
        {
            H_AssertEx.That(userId == _currentUser.Id, "无法操作当前登录用户");
            H_AssertEx.That(userId == -1, "无法操作系统管理员账户");
        }


        /// <summary>
        /// 根据账号密码登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SysUser> LoginByAccountPwd(string account, string password)
        {
            var users = await _userRep.GetUserByAccountPwd(account, password);

            H_AssertEx.That(users.Count == 0, "账号或密码错误");

            H_AssertEx.That(users.Count > 1, "用户数据异常");

            var user = users.First();

            H_AssertEx.That(!user.Enabled.Value, "用户已注销");

            H_AssertEx.That(string.IsNullOrWhiteSpace(user.AuthNumbers), "没有系统权限，暂时无法登录，请联系管理员");

            var authNums = JsonConvert.DeserializeObject<List<long>>(user.AuthNumbers);

            H_AssertEx.That(authNums.Count == 0, "没有系统权限，暂时无法登录，请联系管理员");

            return user;
        }

        /// <summary>
        /// 更新用户密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public async Task UpdatePwd(long userId, string oldPwd, string newPwd)
        {
            var user = await Get(userId);
            oldPwd = H_EncryptProvider.HMACSHA256(oldPwd, _appSettings.Key.Sha256Key);

            H_AssertEx.That(user.Password != oldPwd, "原密码错误");

            user.PasswordLevel = (PasswordLevel)H_Util.CheckPasswordLevel(newPwd);
            user.Password = H_EncryptProvider.HMACSHA256(newPwd, _appSettings.Key.Sha256Key);

            await _userRep.UpdateAsync(user, user => new { user.Password, user.PasswordLevel });
        }
    }
}
