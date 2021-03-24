using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 菜单模块领域服务接口
    /// </summary>
    public interface IModuleDomainService
    {
        /// <summary>
        /// 添加菜单模块
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        Task Add(SysModule module);


        /// <summary>
        /// 获取菜单模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysModule> Get(long id);
    }
}
