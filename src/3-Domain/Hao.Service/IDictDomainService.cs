using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 字典领域服务接口
    /// </summary>
    public interface IDictDomainService
    {
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysDict> Get(long id);

        /// <summary>
        /// 检查字典是否存在相同名字
        /// </summary>
        /// <returns></returns>
        Task CheckNameCode(string name, string code);

        /// <summary>
        /// 检查字典数据项是否存在相同名字or相同值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task CheckItemNameValue(string name, int value, long parentId);
    }
}
