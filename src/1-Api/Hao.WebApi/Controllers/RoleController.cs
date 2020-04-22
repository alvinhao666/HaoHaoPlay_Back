using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 角色
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoleController:HController
    {
        private readonly IRoleAppService _roleAppService;
        
        public RoleController(IRoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        ///// <summary>
        ///// 添加角色
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task Add([FromBody] RoleAddRequest request) => await _roleAppService.AddRole(request);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_262144")]
        public async Task<List<RoleVM>> GetRoleList() => await _roleAppService.GetRoleList();

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateRoleAuth/{id}")]
        [AuthCode("1_524288")]
        public async Task UpdateRoleAuth(long id, [FromBody]RoleUpdateRequest request) =>
            await _roleAppService.UpdateRoleAuth(id, request);


        /// <summary>
        /// 获取角色用户的模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRoleModule/{id}")]
        [AuthCode("1_262144")]
        public async Task<RoleModuleVM> GetRoleModule(long id) => await _roleAppService.GetRoleModule(id);

        ///// <summary>
        ///// 删除角色
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpDelete("{id}")]
        //public async Task DeleteRole(long id) => await _roleAppService.DeleteRole(id);
    }
}