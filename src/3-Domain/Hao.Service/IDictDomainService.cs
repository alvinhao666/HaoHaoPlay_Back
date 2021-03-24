using Hao.Model;
using System.Threading.Tasks;

namespace Hao.Service
{
    /// <summary>
    /// 字典领域服务
    /// </summary>
    public interface IDictDomainService
    {
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SysDict> Get(long id);
    }
}
