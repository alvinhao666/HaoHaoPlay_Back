using Hao.AppService.ViewModel;
using Hao.Core;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserAppService 
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddUser(UserAddRequest vm);
                
        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task EditUser(long userId, UserUpdateRequest vm);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<PagedList<UserVM>> GetUserPagedList(UserQueryInput queryInput);

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDetailVM> GetUser(long id);

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
        /// 导出excel
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<UserExcelVM> ExportUser(UserQueryInput queryInput);

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<bool> IsExistUser(UserQueryInput queryInput);

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task ImportUser(IFormFileCollection files);
    }
}
