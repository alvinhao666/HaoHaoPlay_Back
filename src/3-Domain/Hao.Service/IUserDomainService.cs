using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 用户领域服务
    /// </summary>
    public interface IUserDomainService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task Add(SysUser user);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<SysUser> Get(long userId);

        /// <summary>
        /// 检测用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        void CheckUser(long userId);
    }
}
