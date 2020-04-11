using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Hao.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController
    {
        private readonly RoleAppService _roleAppService;
        
        public RoleController(RoleAppService roleAppService)
        {
            _roleAppService = roleAppService;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add([FromBody] RoleAddRequest request) => await _roleAppService.AddRole(request);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRoleList")]
        public async Task<List<RoleVM>> GetRoleList() => await _roleAppService.GetRoleList();

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modueIds"></param>
        /// <returns></returns>
        [HttpPut("UpdateRoleAuth/{id}")]
        public async Task UpdateRoleAuth(long id, [FromBody] List<long> modueIds) =>
            await _roleAppService.UpdateRoleAuth(id, modueIds);

        /// <summary>
        /// 删除橘色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task DeleteRole(long id) => await _roleAppService.DeleteRole(id);
    }
}