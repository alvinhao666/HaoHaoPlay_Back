using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictController : H_Controller
    {
        private readonly IDictAppService _dictAppService;

        private readonly IMapper _mapper;


        public DictController(IMapper mapper, IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
            _mapper = mapper;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("1_1048576")]
        public async Task AddDict([FromBody]DictAddRequest request) => await _dictAppService.AddDict(request);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_524288")]
        public async Task<PagedList<DictVM>> GetDictPagedList([FromQuery]DictQueryInput query) => await _dictAppService.GetDictPagedList(_mapper.Map<DictQuery>(query));

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_2097152")]
        public async Task UpdateDict(long? id, [FromBody]DictUpdateRequest request) => await _dictAppService.UpdateDict(id.Value, request);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("1_4194304")]
        public async Task DeleteDict(long? id) => await _dictAppService.DeleteDict(id.Value);

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("1_1048576")]
        public async Task AddDictItem([FromBody]DictItemAddRequest request) => await _dictAppService.AddDictItem(request);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_524288")]
        public async Task<PagedList<DictItemVM>> GetDictItemPagedList([FromQuery]DictQueryInput query) => await _dictAppService.GetDictItemPagedList(_mapper.Map<DictQuery>(query));

        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_2097152")]
        public async Task UpdateDictItem(long? id, [FromBody]DictItemUpdateRequest request) => await _dictAppService.UpdateDictItem(id.Value, request);

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("1_4194304")]
        public async Task DeleteDictItem(long? id) => await _dictAppService.DeleteDictItem(id.Value);
    }
}