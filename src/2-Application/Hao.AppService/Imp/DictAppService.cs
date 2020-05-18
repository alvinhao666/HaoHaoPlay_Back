using AutoMapper;
using Hao.Core;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using System.Threading.Tasks;
using Hao.AppService.ViewModel;
using System.Linq;
using System.Collections.Generic;

namespace Hao.AppService
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictAppService : ApplicationService, IDictAppService
    {
        private readonly ISysDictRepository _dictRep;

        private readonly IMapper _mapper;

        public DictAppService(ISysDictRepository dictRep, IMapper mapper)
        {
            _dictRep = dictRep;
            _mapper = mapper;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDict(DictAddRequest request)
        {
            using (var redisLock = RedisHelper.Lock("AddDict", 10)) //redis 分布式锁
            {
                if (redisLock == null) throw new H_Exception("系统异常，请重新添加"); //对象为null，不占资源 ，编译后的代码没有fianlly,不执行dispose()方法

                var sameItems = await _dictRep.GetListAysnc(new DictQuery { EqualDictName = request.DictName });
                if (sameItems.Count > 0) throw new H_Exception("字典名称已存在，请重新添加");

                sameItems = await _dictRep.GetListAysnc(new DictQuery { EqualDictCode = request.DictCode });
                if (sameItems.Count > 0) throw new H_Exception("字典编码已存在，请重新添加");

                var dict = _mapper.Map<SysDict>(request);
                dict.Sort = 0;
                await _dictRep.InsertAysnc(dict);
            }
        }

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateDict(long id, DictUpdateRequest request)
        {
            using (var redisLock = RedisHelper.Lock("UpdateDict", 10)) //redis 分布式锁
            {
                if (redisLock == null) throw new H_Exception("系统异常，请重新添加");

                var sameItems = await _dictRep.GetListAysnc(new DictQuery { EqualDictName = request.DictName });
                if (sameItems.Where(a => a.Id != id).Count() > 0) throw new H_Exception("字典名称已存在，请重新添加");

                sameItems = await _dictRep.GetListAysnc(new DictQuery { EqualDictCode = request.DictCode });
                if (sameItems.Where(a => a.Id != id).Count() > 0) throw new H_Exception("字典编码已存在，请重新添加");

                var dict = await _dictRep.GetAysnc(id);
                dict.DictCode = request.DictCode;
                dict.DictName = request.DictName;
                dict.Remark = request.Remark;
                dict.Sort = request.Sort;
                await _dictRep.UpdateAsync(dict, a => new { a.DictCode, a.DictName, a.Remark, a.Sort });
            }
        }

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task DeleteDict(long id)
        {
            var dictItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = id });
            await _dictRep.DeleteAysnc(id);

            if (dictItems.Count < 1) return;

            await _dictRep.DeleteAysnc(dictItems);
        }



        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedList<DictVM>> GetDictList(DictQuery query)
        {
            var dicts = await _dictRep.GetPagedListAysnc(query);

            return _mapper.Map<PagedList<DictVM>>(dicts);
        }

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDictItem(DictItemAddRequest request)
        {

            using (var redisLock = RedisHelper.Lock("AddDictItem", 10)) //redis 分布式锁
            {
                if (redisLock == null) throw new H_Exception("系统异常，请重新添加");


                var sameItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = request.ParentId, EqualItemName = request.ItemName });
                if (sameItems.Count > 0) throw new H_Exception("数据项名称已存在，请重新添加");


                sameItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = request.ParentId, ItemValue = request.ItemValue });
                if (sameItems.Count > 0) throw new H_Exception("数据项值已存在，请重新添加");


                var parentDict = await GetDictDetail(request.ParentId.Value);
                var dict = _mapper.Map<SysDict>(request);
                dict.ParentId = parentDict.Id;
                dict.DictCode = parentDict.DictCode;
                dict.DictName = parentDict.DictName;
                if (!dict.Sort.HasValue)
                {
                    var dictItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = request.ParentId.Value });
                    dict.Sort = dictItems.Count + 1;
                }
                await _dictRep.InsertAysnc(dict);
            }
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<DictItemVM>> GetDictItemList(DictQuery query)
        {
            query.OrderFileds = $"{nameof(SysDict.Sort)},{nameof(SysDict.CreateTime)}";
            var dicts = await _dictRep.GetPagedListAysnc(query);

            return _mapper.Map<PagedList<DictItemVM>>(dicts);
        }

        /// <summary>
        /// 更新数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task UpdateDictItem(long id, DictItemUpdateRequest request)
        {
            using (var redisLock = RedisHelper.Lock("UpdateDictItem", 10))
            {
                if (redisLock == null) throw new H_Exception("系统异常，请重新添加");

                var item = await _dictRep.GetAysnc(id);

                var sameItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = item.ParentId, EqualItemName = request.ItemName });
                if (sameItems.Where(a => a.Id != id).Count() > 0) throw new H_Exception("数据项名称已存在，请重新添加");

                sameItems = await _dictRep.GetListAysnc(new DictQuery { ParentId = item.ParentId, ItemValue = request.ItemValue });
                if (sameItems.Where(a => a.Id != id).Count() > 0) throw new H_Exception("数据项值已存在，请重新添加");

                item.ItemName = request.ItemName;
                item.ItemValue = request.ItemValue;
                item.Remark = request.Remark;
                item.Sort = request.Sort;
                await _dictRep.UpdateAsync(item, a => new { a.ItemName, a.ItemValue, a.Remark, a.Sort });
            }
        }

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteDictItem(long id)
        {
            await _dictRep.DeleteAysnc(id);
        }

        /// <summary>
        /// 根据字典编码查询数据项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>

        public async Task<List<DictDataItemVM>> GetDictDataItem(string dictCode)
        {
            var dictItems = await _dictRep.GetListAysnc(new DictQuery { EqualDictCode = dictCode, IsQueryItem = true, OrderFileds = $"{nameof(SysDict.Sort)},{nameof(SysDict.CreateTime)}" });

            return _mapper.Map<List<DictDataItemVM>>(dictItems);
        }


        #region private

        /// <summary>
        /// 字典详情
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        private async Task<SysDict> GetDictDetail(long dictId)
        {
            var dict = await _dictRep.GetAysnc(dictId);
            if (dict == null) throw new H_Exception("字典数据不存在");
            if (dict.IsDeleted) throw new H_Exception("字典数据已删除");
            return dict;
        }


        #endregion
    }
}