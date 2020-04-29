using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 应用资源
    /// </summary>
    public class ResourceController:HController
    {
        private readonly IModuleAppService _moduleAppService;

        public ResourceController(IModuleAppService moduleAppService)
        {
            _moduleAppService = moduleAppService;
        }


        /// <summary>
        /// 添加资源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("1_2097152")]
        public async Task Add([FromBody]ResourceAddRequest request) => await _moduleAppService.AddResource(request);

        /// <summary>
        /// 删除资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("1_8388608")]
        public async Task Delete(long id) => await _moduleAppService.DeleteResource(id);

        /// <summary>
        /// 查询资源分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetList/{parentId}")]
        [AuthCode("1_16384")]
        public async Task<List<ResourceItemVM>> GetList(long parentId) => await _moduleAppService.GetResourceList(parentId);

        /// <summary>
        /// 更新资源信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_4194304")]
        public async Task Update(long id, [FromBody]ResourceUpdateRequest request) => await _moduleAppService.UpdateResource(id, request);
    }
}
