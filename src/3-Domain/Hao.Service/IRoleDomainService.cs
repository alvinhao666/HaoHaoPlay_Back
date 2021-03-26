using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 角色领域服务接口
    /// </summary>
    public interface IRoleDomainService
    {
        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<SysRole> Get(long roleId);
    }
}
