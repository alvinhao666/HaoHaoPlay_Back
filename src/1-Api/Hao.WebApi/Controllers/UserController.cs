using AutoMapper;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Core.Extensions;
using Hao.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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



        private readonly AppSettingsInfo _appsettings;

        public UserController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, IMapper mapper, IUserAppService userService, IRoleAppService roleAppService)
        {
            _userAppService = userService;
            _roleAppService = roleAppService;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;

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
        [HttpGet("GetRole")]
        [AuthCode("1_4")]
        public async Task<List<RoleSelectVM>> GetRoleList() => await _roleAppService.GetRoleListByCurrentRole();

        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("IsExistUser")]
        [AuthCode("1_4")]
        public async Task<bool> IsExistUser([FromQuery]UserQueryInput query) => await _userAppService.IsExistUser(_mapper.Map<UserQuery>(query));

        /// <summary>
        /// 查询用户分页列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AuthCode("1_4")]
        public async Task<PagedList<UserVM>> GetPagedList([FromQuery]UserQueryInput query) => await _userAppService.GetUserPageList(_mapper.Map<UserQuery>(query));

        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [AuthCode("1_4")]
        public async Task<UserDetailVM> Get(long id) => await _userAppService.GetUser(id);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AuthCode("1_64")]
        public async Task Update(long id, [FromBody]UserUpdateRequest request) => await _userAppService.EditUser(id, request);

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Disable/{id}")]
        [AuthCode("1_128")]
        public async Task Disable(long id) => await _userAppService.UpdateUserStatus(id, false);

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Enable/{id}")]
        [AuthCode("1_256")]
        public async Task Enable(long id) => await _userAppService.UpdateUserStatus(id, true);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [AuthCode("1_512")]
        public async Task Delete(long id) => await _userAppService.DeleteUser(id);


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("Export")]
        [AuthCode("1_2048")]
        public async Task<UserExcelVM> ExportUser([FromQuery]UserQueryInput query) => await _userAppService.ExportUser(_mapper.Map<UserQuery>(query));


        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost("Import")]
        [AuthCode("1_1024")]
        public async Task ImportUser() => await _userAppService.ImportUser(HttpContext.Request.Form.Files);
    }
}
