using AutoMapper;
using Hao.Core;
using Hao.Model;
using Hao.Repository;
using Hao.RunTimeException;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 数据字典应用服务
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
            
            await _dictRep.InsertAysnc(dict);
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

            await _dictRep.InsertAysnc(dict);
        }


        #region private

        /// <summary>
        /// 获取字典详情
        /// </summary>
        /// <param name="dictId"></param>
        /// <returns></returns>
        private async Task<SysDict> GetDictDetail(long dictId)
        {
            var user = await _dictRep.GetAysnc(dictId);
            if (user == null) throw new HException("用户不存在");
            if (user.IsDeleted) throw new HException("用户已删除");
            return user;
        }
        #endregion
    }
}