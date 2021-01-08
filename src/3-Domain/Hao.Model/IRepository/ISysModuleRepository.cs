using Hao.Core;
using Hao.Enum;
using System.Threading.Tasks;

namespace Hao.Model
{
    public interface ISysModuleRepository : IRepository<SysModule, long>
    {
        /// <summary>
        /// 获取每一层的数量，包括已删除的，最多31个 0~30
        /// </summary>
        /// <returns></returns>
        Task<ModuleLayerCountDTO> GetLayerCount();


        /// <summary>
        /// 是否存在相同名字的模块
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> IsExistSameNameModule(string name, ModuleType? moduleType, long? parentId, long? id = null);

        /// <summary>
        /// 是否存在相同别名的模块
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="moduleType"></param>
        /// <param name="parentId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> IsExistSameAliasModule(string alias, ModuleType? moduleType, long? parentId, long? id = null);
    }
}
