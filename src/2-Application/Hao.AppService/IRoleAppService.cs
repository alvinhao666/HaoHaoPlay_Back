using System.Threading.Tasks;
using Hao.AppService.ViewModel;

namespace Hao.AppService
{
    public interface IRoleAppService
    {
        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task AddRole(RoleAddRequest vm);
    }
}