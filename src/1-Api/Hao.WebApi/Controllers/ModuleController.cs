using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 应用模块
    /// </summary>
    public class ModuleController:H_Controller
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
        //[AuthCode("1_32768")]
        public async Task Add([FromBody]ModuleAddRequest request) => await _moduleAppService.AddModule(request);

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[AuthCode("1_16384")]
        public async Task<List<ModuleVM>> GetList() => await _moduleAppService.GetList();


        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[AuthCode("1_16384")]
        public async Task<ModuleDetailVM> Get(long id) => await _moduleAppService.Get(id);

        /// <summary>
        /// 更新模块信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[AuthCode("1_65536")]
        public async Task Update(long id, [FromBody]ModuleUpdateRequest request) => await _moduleAppService.UpdateModule(id, request);


        /// <summary>
        /// 删除模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[AuthCode("1_131072")]
        public async Task Delete(long id) => await _moduleAppService.Delete(id);
    }
}
