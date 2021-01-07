using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.WebApi
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictController : H_Controller
    {
        private readonly IDictAppService _dictAppService;

        public DictController(IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
        }


        /// <summary>
        /// 添加字典
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("Dict_Add_1_1048576")]
        public async Task AddDict([FromBody]DictAddRequest request) => await _dictAppService.Add(request);

        /// <summary>
        /// 查询字典
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("Dict_Search_1_67108864")]
        public async Task<PagedResult<DictVM>> GetDictPagedList([FromQuery]DictQueryInput queryInput) => await _dictAppService.GetPagedList(queryInput);

        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("Dict_Edit_1_2097152")]
        public async Task UpdateDict(long? id, [FromBody]DictUpdateRequest request) => await _dictAppService.Update(id.Value, request);

        /// <summary>
        /// 删除字典
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("Dict_Delete_1_4194304")]
        public async Task DeleteDict(long? id) => await _dictAppService.Delete(id.Value);

        /// <summary>
        /// 添加字典项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("Dict_Add_1_1048576")]
        public async Task AddDictItem([FromBody]DictItemAddRequest request) => await _dictAppService.AddDictItem(request);

        /// <summary>
        /// 查询字典数据项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("Dict_Search_1_67108864")]
        public async Task<PagedResult<DictItemVM>> GetDictItemPagedList([FromQuery]DictQueryInput queryInput) => await _dictAppService.GetDictItemPagedList(queryInput);

        /// <summary>
        /// 修改数据项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("Dict_Edit_1_2097152")]
        public async Task UpdateDictItem(long? id, [FromBody]DictItemUpdateRequest request) => await _dictAppService.UpdateDictItem(id.Value, request);

        /// <summary>
        /// 删除数据项
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("Dict_Delete_1_4194304")]
        public async Task DeleteDictItem(long? id) => await _dictAppService.DeleteDictItem(id.Value);
    }
}