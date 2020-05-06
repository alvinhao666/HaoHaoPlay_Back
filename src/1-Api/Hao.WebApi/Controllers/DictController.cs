using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Hao.AppService.ViewModel.Dict;
using Hao.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public DictController(IMapper mapper,IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
            _mapper = mapper;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddDict")]
        public async Task AddDict(DictAddRequest request) => await _dictAppService.AddDict(request);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost("GetDictList")]
        public async Task<PagedList<DictVM>> GetDictList(DictQueryInput query) =>
            await _dictAppService.GetDictList(_mapper.Map<DictQuery>(query));
        
        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateDict/{id}")]
        public async Task UpdateDict(long id, DictUpdateRequest request) => await _dictAppService.UpdateDict(id,request);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDict/{id}")]
        public async Task DeleteDict(long id) => await _dictAppService.DeleteDict(id);
        
        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("AddDictItem")]
        public async Task AddDictItem(DictItemAddRequest request) => await _dictAppService.AddDictItem(request);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDictItemList")]
        public async Task<PagedList<DictItemVM>> GetDictItemList(DictQueryInput query) => await _dictAppService.GetDictItemList(_mapper.Map<DictQuery>(query));

        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateDictItem/{id}")]
        public async Task UpdateDictItem(long id, DictItemUpdateRequest request) =>
            await _dictAppService.UpdateDictItem(id, request);

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteDictItem/{id}")]
        public async Task DeleteDictItem(long id) => await _dictAppService.DeleteDictItem(id);
    }
}