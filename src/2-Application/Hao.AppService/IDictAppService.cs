using System.Threading.Tasks;
using Hao.AppService.ViewModel.Dict;
using Hao.Core;

namespace Hao.AppService
{
    public interface IDictAppService
    {
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDict(DictAddRequest request);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PagedList<DictVM>> GetDictList(DictQuery query);
        
        /// <summary>
        /// 添加字典数据项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task AddDictItem(DictItemAddRequest request);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        Task<PagedList<DictItemVM>> GetDictItemList(DictQuery query);
    }
}