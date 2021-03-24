using Hao.Core;
using Hao.Library;
using Hao.Model;
using Hao.Runtime;
using Npgsql;
using System.Threading.Tasks;

namespace Hao.Service
{
    public class UserDomainService : DomainService, IUserDomainService
    {
        private readonly IUserRepository _userRep;

        private readonly ICurrentUser _currentUser;

        public UserDomainService(IUserRepository userRep, ICurrentUser currentUser)
        {
            _userRep = userRep;
            _currentUser = currentUser;
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
                await _userRep.InsertAsync(user);
            }
            catch (PostgresException ex)
            {
                H_AssertEx.That(ex.SqlState == H_PostgresSqlState.E23505, "账号已存在，请重新输入");
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

    }
}
