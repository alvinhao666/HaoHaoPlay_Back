using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.VisualBasic;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictController:HController
    {
        private readonly IDictAppService _dictAppService;

        private readonly IMapper _mapper;
        public DictController(IMapper _mapper,IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task AddDict(DictAddRequest request) => await _dictAppService.AddDict(request);


        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task GetDictList(DictQueryInput query) =>
            await _dictAppService.GetDictList(_mapper.Map<DictQuery>(query));
        
        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task AddDictItme(DictItemAddRequest request) => await _dictAppService.AddDictItem(request);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        public async Task GetDictItemList(DictQueryInput query) => await _dictAppService.GetDictItemList(_mapper.Map<DictQuery>(query));
    }
}