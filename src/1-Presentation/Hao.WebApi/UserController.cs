using AutoMapper;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : H_Controller
    {
        private readonly IUserAppService _userAppService;

        private readonly IRoleAppService _roleAppService;

        private readonly IMapper _mapper;


        public UserController(IMapper mapper, IUserAppService userService, IRoleAppService roleAppService)
        {
            _userAppService = userService;
            _roleAppService = roleAppService;
            _mapper = mapper;

        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("1_32")]
        public async Task Add([FromBody]UserAddRequest request) => await _userAppService.AddUser(request);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_4")]
        public async Task<List<RoleSelectVM>> GetRoleList() => await _roleAppService.GetRoleListByCurrentRole();

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_4")]
        public async Task<bool> IsExistUser([FromQuery]UserQueryInput queryInput) => await _userAppService.IsExistUser(queryInput);

        /// <summary>
        /// 查询用户分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_4")]
        public async Task<PagedList<UserVM>> GetPagedList([FromQuery]UserQueryInput queryInput) => await _userAppService.GetUserPagedList(queryInput);

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AuthCode("1_4")]
        public async Task<UserDetailVM> Get(long? id) => await _userAppService.GetUser(id.Value);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_64")]
        public async Task Update(long? id, [FromBody]UserUpdateRequest request) => await _userAppService.EditUser(id.Value, request);

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_128")]
        public async Task Disable(long? id) => await _userAppService.UpdateUserStatus(id.Value, false);

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_256")]
        public async Task Enable(long? id) => await _userAppService.UpdateUserStatus(id.Value, true);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("1_512")]
        public async Task Delete(long? id) => await _userAppService.DeleteUser(id.Value);


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_2048")]
        public async Task<UserExcelVM> Export([FromQuery]UserQueryInput queryInput) => await _userAppService.ExportUser(queryInput);


        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [AuthCode("1_1024")]
        public async Task Import() => await _userAppService.ImportUser(HttpContext.Request.Form.Files);
    }
}
