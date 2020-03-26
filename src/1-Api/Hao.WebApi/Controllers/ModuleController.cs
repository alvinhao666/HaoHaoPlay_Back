using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModuleController:HController
    {

        private readonly IModuleAppService _moduleAppService;

        public ModuleController(IModuleAppService moduleAppService)
        {
            _moduleAppService = moduleAppService;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add([FromBody]ModuleAddRequest request) => await _moduleAppService.AddModule(request);

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ModuleVM>> GetList() => await _moduleAppService.GetList();


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ModuleDetailVM> Get(long? id) => await _moduleAppService.Get(id.Value);

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Update(long? id, [FromBody]ModuleUpdateRequest request) => await _moduleAppService.UpdateModule(id.Value, request);


        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(long? id) => _moduleAppService.Delete(id.Value);
    }
}
