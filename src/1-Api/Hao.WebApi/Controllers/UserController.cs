using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using AutoMapper;
using Hao.Library;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Hao.File;
using Hao.RunTimeException;
using Microsoft.Extensions.Options;
using Hao.Core.Extensions;
using Hao.Core;

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

        private readonly ITimeLimitedDataProtector _protector;

        private readonly AppSettingsInfo _appsettings;

        public UserController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, IDataProtectionProvider provider, IMapper mapper, IUserAppService userService, IRoleAppService roleAppService)
        {
            _userAppService = userService;
            _roleAppService = roleAppService;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value;
            _protector = provider.CreateProtector(appsettingsOptions.Value.DataProtectorPurpose.FileDownload).ToTimeLimitedDataProtector();
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
        public async Task<PagedList<UserItemVM>> GetPagedList([FromQuery]UserQueryInput query) => await _userAppService.GetUserPageList(_mapper.Map<UserQuery>(query));

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
        public async Task<object> ExportUser([FromQuery]UserQueryInput query)
        {
            string fileName = await _userAppService.ExportUsers(_mapper.Map<UserQuery>(query));

            return new { FileName = fileName, FileId = _protector.Protect(fileName.Split('.')[0], TimeSpan.FromSeconds(5)) };
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost("Import")]
        [AuthCode("1_1024")]
        public async Task ImportUser()
        {
            var files = HttpContext.Request.Form.Files;

            if (files == null || files.Count == 0) throw new H_Exception("请选择Excel文件");

            //格式限制
            var allowType = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            if (files.Any(b => !allowType.Contains(b.ContentType))) throw new H_Exception("只能上传Excel文件");


            ////大小限制
            //if (files.Sum(b => b.Length) >= 1024 * 1024 * 4)
            //{

            //}

            var users = new List<UserAddRequest>();

            foreach (IFormFile file in files)
            {
                var reader = new StreamReader(file.OpenReadStream());
                var content = reader.ReadToEnd();
                var name = file.FileName;

                string rootPath = _appsettings.FilePath.ImportExcelPath;

                H_File.CreateDirectory(rootPath);
                string filePath = Path.Combine(rootPath, $"{name}");

                using (var fs = System.IO.File.Create(filePath))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }

                using (var ep = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = ep.Workbook.Worksheets[0];
                    if (worksheet != null && worksheet.Cells[1, 1].Text.Trim() != "姓名")
                    {
                        throw new H_Exception("上传数据列名有误，请检查");
                    }
                    foreach (var ws in ep.Workbook.Worksheets)
                    {
                        int colStart = ws.Dimension.Start.Column;  //工作区开始列,start=1
                        int colEnd = ws.Dimension.End.Column;       //工作区结束列
                        int rowStart = ws.Dimension.Start.Row;       //工作区开始行号,start=1
                        int rowEnd = ws.Dimension.End.Row;       //工作区结束行号

                        for (int i = rowStart + 1; i <= rowEnd; i++) //第1行是列名,跳过
                        {
                            var user = new UserAddRequest();
                            user.Name = ws.Cells[i, colStart].Text;
                            users.Add(user);
                        }
                    }
                }
            }

            if (users.Count == 0) return;

            await _userAppService.AddUsers(users);
        }
    }
}
