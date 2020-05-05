using AutoMapper;
using Hao.Core;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using System.Threading.Tasks;
using Hao.AppService.ViewModel.Dict;

namespace Hao.AppService
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictAppService:ApplicationService,IDictAppService
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
            var dict = _mapper.Map<SysDict>(request);
            dict.Sort = 0;
            await _dictRep.InsertAysnc(dict);
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
            var parentDict = await GetDictDetail(request.ParentId.Value);

            var dict = _mapper.Map<SysDict>(request);
            dict.ParentId = parentDict.Id;
            dict.DictCode = parentDict.DictCode;
            dict.DictName = parentDict.DictName;
            dict.Sort = 0;
            await _dictRep.InsertAysnc(dict);
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<DictItemVM>> GetDictItemList(DictQuery query)
        {
            var dicts = await _dictRep.GetPagedListAysnc(query);

            return _mapper.Map<PagedList<DictItemVM>>(dicts);
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
            if (dict == null) throw new HException("字典数据不存在");
            if (dict.IsDeleted) throw new HException("字典数据已删除");
            return dict;
        }
        #endregion
    }
}