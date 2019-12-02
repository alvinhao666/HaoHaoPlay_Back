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
using Hao.Entity;
using Microsoft.Extensions.Options;

namespace Hao.WebApi
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : HController
    {
        private readonly IUserAppService _userAppService;

        private readonly IMapper _mapper;

        private readonly ITimeLimitedDataProtector _protector;

        public FilePathInfo PathInfo { get; set; }

        public UserController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, IDataProtectionProvider provider, IMapper mapper, IUserAppService userService)
        {
            _userAppService = userService;
            _mapper = mapper;
            _protector = provider.CreateProtector(appsettingsOptions.Value.DataProtectorPurpose.FileDownload).ToTimeLimitedDataProtector();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Add([FromBody]UserIn vm) => await _userAppService.AddUser(vm);

        /// <summary>
        /// 查询用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedList<UserOut>> GetPagedList([FromQuery]UserQueryInput query) => await _userAppService.GetUsers(_mapper.Map<UserQuery>(query));

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task Modify(long? id, [FromBody]UserIn vm) => await _userAppService.EditUser(id.Value, vm);
        /// <summary>
        /// 根据id获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOut> Get(long? id) => await _userAppService.GetUser(id.Value);

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
        [HttpPut("Disable/{id}")]
        public async Task Disable(long? id) => await _userAppService.UpdateUserEnabled(id.Value, false);

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Enable/{id}")]
        public async Task Enable(long? id) => await _userAppService.UpdateUserEnabled(id.Value, true);

        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUser")]
        public async Task<UserOut> GetCurrentUser() => await _userAppService.GetCurrentUser();


        //        /// <summary>
        //        /// 导出用户
        //        /// </summary>
        //        /// <param name="query"></param>
        //        /// <returns></returns>
        //        [HttpGet]
        //        [NoGlobalResult]
        //        public async Task<HttpResponseMessage> ExportUsers([FromQuery]UserQueryInput query)
        //        {
        //            string fileName = await _userAppService.ExportUsers(_mapper.Map<UserQuery>(query));
        //
        //            string filePath = Path.Combine(new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent.FullName + "/ExportFile/Excel/", $"{fileName}");
        //
        //            var response = await DownFile(filePath, fileName);
        //
        //            return response;
        //        }

        /// <summary>
        /// 导出用户
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("ExportUser")]
        public async Task<object> ExportUser([FromQuery]UserQueryInput query)
        {
            string fileName = await _userAppService.ExportUsers(_mapper.Map<UserQuery>(query));

            return new { FileName = fileName, FileId = _protector.Protect(fileName.Split('.')[0], TimeSpan.FromSeconds(5)) };
        }


        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost("ImportUser")]
        public async Task ImportUser()
        {
            var files = HttpContext.Request.Form.Files;

            if (files == null || files.Count == 0) 
            {
                throw new HException(ErrorInfo.E005007, nameof(ErrorInfo.E005007).GetErrorCode());
            }

            //格式限制
            var allowType = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" };

            if (files.Any(b => !allowType.Contains(b.ContentType)))
            {
                throw new HException(ErrorInfo.E005008, nameof(ErrorInfo.E005008).GetErrorCode());
            }

            ////大小限制
            //if (files.Sum(b => b.Length) >= 1024 * 1024 * 4)
            //{
               
            //}

            var users = new List<UserIn>();
            foreach (IFormFile file in files)
            {
                var reader = new StreamReader(file.OpenReadStream());
                var content = reader.ReadToEnd();
                var name = file.FileName;

                string rootPath = PathInfo.ImportExcelPath;

                if (!HFile.IsExistDirectory(rootPath))
                    HFile.CreateDirectory(rootPath);
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
                    foreach (var ws in ep.Workbook.Worksheets)
                    {
                        int colStart = ws.Dimension.Start.Column;  //工作区开始列,start=1
                        int colEnd = ws.Dimension.End.Column;       //工作区结束列
                        int rowStart = ws.Dimension.Start.Row;       //工作区开始行号,start=1
                        int rowEnd = ws.Dimension.End.Row;       //工作区结束行号

                        for (int i = rowStart + 1; i <= rowEnd; i++) //第1行是列名,跳过
                        {
                            var user = new UserIn();
                            user.Name = ws.Cells[i, colStart].Text;
                            user.LoginName = ws.Cells[i, colStart + 1].Text;
                            user.Password = ws.Cells[i, colStart + 2].Text;
                            users.Add(user);
                        }
                    }
                }
            }
            await _userAppService.AddUsers(users);
        }
    }
}
