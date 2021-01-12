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
        Task Add(UserAddRequest vm);
                
        /// <summary>
        /// 更新编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task Update(long userId, UserUpdateRequest vm);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<Paged<UserVM>> GetPaged(UserQueryInput queryInput);

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserDetailVM> Get(long id);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task Delete(long userId);

        /// <summary>
        /// 注销启
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="enabled"></param>
        /// <returns></returns>
        Task UpdateStatus(long userId,bool enabled);

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<UserExcelVM> Export(UserQueryInput queryInput);

        /// <summary>
        /// 是否存在账号
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        Task<bool> IsExistLoginName(string loginName);

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        Task Import(IFormFileCollection files);
    }
}
