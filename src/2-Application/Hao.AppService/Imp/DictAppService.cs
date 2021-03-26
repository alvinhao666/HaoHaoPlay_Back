using Hao.Core;
using Hao.Model;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Mapster;
using Hao.Enum;
using Hao.Service;

namespace Hao.AppService
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictAppService : ApplicationService, IDictAppService
    {
        private readonly IDictRepository _dictRep;

        private readonly IDictDomainService _dictDomainService;

        public DictAppService(IDictRepository dictRep, IDictDomainService dictDomainService)
        {
            _dictRep = dictRep;
            _dictDomainService = dictDomainService;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("DictAppService_AddDict")]
        public async Task Add(DictAddInput input)
        {
            await _dictDomainService.CheckNameOrCode(input.DictName, input.DictCode);

            var dict = input.Adapt<SysDict>();
            dict.ParentId = -1;
            dict.Sort = 0;
            await _dictRep.InsertAsync(dict);
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("DictAppService_UpdateDict")]
        public async Task Update(long id, DictUpdateInput input)
        {
            await _dictDomainService.CheckNameOrCode(input.DictName, input.DictCode);

            var dict = await _dictRep.GetAsync(id);
            dict.DictCode = input.DictCode;
            dict.DictName = input.DictName;
            dict.Remark = input.Remark;
            dict.Sort = input.Sort;
            await _dictRep.UpdateAsync(dict, a => new { a.DictCode, a.DictName, a.Remark, a.Sort });
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Delete(long id)
        {
            var dict = await _dictRep.GetAsync(id);

            var dictItems = await _dictRep.GetListAsync(new DictQuery { ParentId = id });

            await _dictRep.DeleteAsync(dict);

            if (dictItems.Count < 1) return;

            await _dictRep.DeleteAsync(dictItems);
        }



        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        public async Task<Paged<DictOutput>> GetPaged(DictQueryInput queryInput)
        {
            var query = queryInput.Adapt<DictQuery>();
            query.DictType = DictType.Main;

            var result = await _dictRep.GetDictPagedResult(query);

            return result.Adapt<Paged<DictOutput>>();

        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("DictAppService_AddDictItem")]
        public async Task AddDictItem(DictItemAddInput input)
        {
            await _dictDomainService.CheckItemNameOrValue(input.ItemName, input.ItemValue.Value, input.ParentId.Value);

            var parentDict = await _dictDomainService.Get(input.ParentId.Value);
            var dict = input.Adapt<SysDict>();
            dict.ParentId = parentDict.Id;
            dict.DictCode = parentDict.DictCode;
            dict.DictName = parentDict.DictName;
            dict.DictType = DictType.Sub;
            if (!dict.Sort.HasValue)
            {
                var dictItems = await _dictRep.GetListAsync(new DictQuery { ParentId = input.ParentId.Value });
                dict.Sort = dictItems.Count + 1;
            }
            await _dictRep.InsertAsync(dict);
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<Paged<DictItemOutput>> GetDictItemPaged(DictQueryInput queryInput)
        {
            var query = queryInput.Adapt<DictQuery>();

            query.OrderBy(a => a.Sort).OrderBy(a => a.CreateTime);

            var dicts = await _dictRep.GetPagedAsync(query);

            return dicts.Adapt<Paged<DictItemOutput>>();
        }

        /// <summary>
        /// 更新数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [DistributedLock("DictAppService_UpdateDictItem")]
        public async Task UpdateDictItem(long id, DictItemUpdateInput input)
        {
            var item = await _dictDomainService.Get(id);

            await _dictDomainService.CheckItemNameOrValue(input.ItemName, input.ItemValue.Value, item.ParentId.Value);

            item.ItemName = input.ItemName;
            item.ItemValue = input.ItemValue;
            item.Remark = input.Remark;
            item.Sort = input.Sort;
            await _dictRep.UpdateAsync(item, a => new { a.ItemName, a.ItemValue, a.Remark, a.Sort });
        }

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteDictItem(long id)
        {
            var dict = await _dictRep.GetAsync(id);
            await _dictRep.DeleteAsync(dict);
        }

        /// <summary>
        /// 根据字典编码查询数据项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>

        public async Task<List<DictDataItemOutput>> GetDictDataItem(string dictCode)
        {
            var query = new DictQuery
            {
                DictCode = dictCode,
                DictType = DictType.Sub
            };

            query.OrderBy(a => a.Sort).OrderBy(a => a.CreateTime);

            var dictItems = await _dictRep.GetListAsync(query);

            return dictItems.Adapt<List<DictDataItemOutput>>();
        }
    }
}