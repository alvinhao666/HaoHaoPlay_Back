using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public interface IUserAppService 
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<LoginOut> Login(UserQuery query);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task<long> AddUser(UserIn vm);
        
        /// <summary>
        /// 批量添加用户
        /// </summary>
        /// <param name="vms"></param>
        /// <returns></returns>
        Task AddUsers(List<UserIn> vms);
        
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task EditUser(long userId, UserIn vm);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedList<UserOut>> GetUsers(UserQuery query);

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserOut> GetUser(long id);

        /// <summary>
        /// 当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserOut> GetCurrentUser();

        /// <summary>
        /// 更新登录时间和ip
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lastLoginTime"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        Task UpdateLogin(long userId, DateTime lastLoginTime, string ip);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteUser(long userId);

        /// <summary>
        /// 注销启
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task UpdateUserStatus(long userId,bool enabled);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<string> ExportUsers(UserQuery query);

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<bool> IsExistUser(UserQuery query);

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        Task UpdateCurrentUserHeadImg(string imgUrl);

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateCurrentUserBaseInfo(UserIn vm);

        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task UpdateCurrentUserPassword(string oldPassword, string newPassword);
    }
}
