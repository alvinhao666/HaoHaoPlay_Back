using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.AppService
{
    /// <summary>
    /// 字典服务接口
    /// </summary>
    public interface IDictAppService
    {
        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Add(DictAddRequest request);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        Task<Paged<DictVM>> GetPaged(DictQueryInput queryInput);

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task Update(long id, DictUpdateRequest request);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);






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
        Task<Paged<DictItemVM>> GetDictItemPaged(DictQueryInput queryInput);

        /// <summary>
        /// 更新数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateDictItem(long id, DictItemUpdateRequest request);

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteDictItem(long id);

        /// <summary>
        /// 根据字典编码查询数据项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        Task<List<DictDataItemVM>> GetDictDataItem(string dictCode);
    }
}