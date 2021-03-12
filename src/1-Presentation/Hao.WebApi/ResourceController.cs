using Hao.AppService;
using Hao.Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.WebApi
{
    /// <summary>
    /// 应用资源
    /// </summary>
    public class ResourceController : H_Controller
    {
        private readonly IModuleAppService _moduleAppService;

        public ResourceController(IModuleAppService moduleAppService)
        {
            _moduleAppService = moduleAppService;
        }


        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("App_Add_1_16384")]
        public async Task Add([FromBody] ResourceAddInput input) => await _moduleAppService.AddResource(input);

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("App_Delete_1_65536")]
        public async Task Delete(long? id) => await _moduleAppService.DeleteResource(id.Value);

        /// <summary>
        /// 查询资源列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("{parentId}")]
        [AuthCode("App_Search_1_8192")]
        public async Task<List<ResourceItemVM>> GetList(long? parentId) => await _moduleAppService.GetResourceList(parentId.Value);

        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("App_Update_1_32768")]
        public async Task Update(long? id, [FromBody] ResourceUpdateInput input) => await _moduleAppService.UpdateResource(id.Value, input);
    }
}
