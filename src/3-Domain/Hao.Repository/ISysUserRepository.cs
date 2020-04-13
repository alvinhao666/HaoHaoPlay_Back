using Hao.Core;
using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Repository
{
    public interface ISysUserRepository : IRepository<SysUser,long>
    {
        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="authNumbers"></param>
        /// <returns></returns>
        void UpdateAuth(long roleId, string authNumbers);
    }
}
