using System;
using System.Threading.Tasks;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.AppController;
using Hao.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Hao.Core.Model;
using Hao.AutoMapper;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Hao.Utility;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using System.Text;

namespace Hao.WebApi
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController: HController
    {
        protected IUserAppService _userAppService;

        protected IAutoMapper _mapper;


        private IHostingEnvironment _hostingEnvironment;

        public UserController(IHostingEnvironment hostingEnvironment,IAutoMapper mapper,IUserAppService userService, IConfigurationRoot config, ICurrentUser currentUser) : base(config, currentUser)
        {
            _userAppService = userService;
            _currentUser = currentUser;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<long> Post([FromBody]UserVMIn vm) => await _userAppService.AddUser(vm);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<UserVMOut>> GetAll([FromQuery]UserQueryInput query) => await _userAppService.GetUsers(_mapper.Map<UserQuery>(query));

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Put(long? id, [FromBody]UserVMIn vm) => await _userAppService.EditUser(id.Value, vm);
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserVMOut> Get(long? id) => await _userAppService.GetUser(id.Value);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task Delete(long? id) => await _userAppService.DeleteUser(id.Value);

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Disable(long? id) => await _userAppService.UpdateUserEnabled(id.Value,false);

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Enable(long? id) => await _userAppService.UpdateUserEnabled(id.Value, true);

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserVMOut> GetCurrentUser() => await _userAppService.GetCurrentUser();


        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [NoGlobalResult]
        public async Task<HttpResponseMessage> ExportUsers([FromQuery]UserQueryInput query)
        {
            string fileName = await _userAppService.ExportUsers(_mapper.Map<UserQuery>(query));

            string filePath = Path.Combine(new DirectoryInfo(_hostingEnvironment.WebRootPath).Parent.Parent.FullName + "/ExportFile/Excel/", $"{fileName}");

            var response = await DownFile(filePath, fileName);

            return response;
        }
    }
}
