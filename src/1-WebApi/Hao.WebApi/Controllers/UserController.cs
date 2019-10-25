using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Hao.Core.Model;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Hao.Library;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using OfficeOpenXml;
using Hao.File;

namespace Hao.WebApi
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController: HController
    {
        private readonly  IUserAppService _userAppService;

        private readonly IMapper _mapper;
        
        private readonly IHostingEnvironment _hostingEnvironment;
        
        private readonly ITimeLimitedDataProtector _protector;

        public UserController(IConfiguration config,IDataProtectionProvider provider,IHostingEnvironment hostingEnvironment,IMapper mapper,IUserAppService userService) 
        {
            _userAppService = userService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _protector = provider.CreateProtector(config["DataProtectorPurpose:FileDownload"]).ToTimeLimitedDataProtector();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task Post([FromBody]UserVMIn vm) => await _userAppService.AddUser(vm);

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
        [HttpGet]
        public async Task<object> ExportUsers([FromQuery]UserQueryInput query)
        {
            string fileName = await _userAppService.ExportUsers(_mapper.Map<UserQuery>(query));

//            string filePath = Path.Combine(new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent.FullName + "/ExportFile/Excel/", $"{fileName}");

//            var response = await DownFile(filePath, fileName);

            return new {FileName = fileName,FileId= _protector.Protect(fileName.Split('.')[0], 
                TimeSpan.FromSeconds(5))};
        }


        [HttpPost]
        public async Task ImportUsers([FromForm] IFormCollection formCollection)
        {
            if (formCollection?.Files == null)
            {
                throw new HException(ErrorInfo.E005007,nameof(ErrorInfo.E005007).GetErrorCode());
            }
            //可能上传多个excel文件
            FormFileCollection files = (FormFileCollection)formCollection.Files;
            
            //格式限制
            var allowType = new string[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"};
            
            if (files.Any(b => !allowType.Contains(b.ContentType)))
            {
                throw new HException(ErrorInfo.E005008,nameof(ErrorInfo.E005008).GetErrorCode());
            }
            
//            //大小限制
//            if (files.Sum(b => b.Length) >= 1024 * 1024 * 4)
//            {
//                return Json(new { isSuccess = false, message = "上传文件的总大小只能在4M以下" }, "text/html");
//            }

            foreach (IFormFile file in files)
            {
                StreamReader reader = new StreamReader(file.OpenReadStream());
                String content = reader.ReadToEnd();
                String name = file.FileName;

                string rootPath = new DirectoryInfo(_hostingEnvironment.ContentRootPath).Parent.FullName + $"/ImportFile/Excel/";
                
                if (!HFile.IsExistDirectory(rootPath))
                    HFile.CreateDirectory(rootPath);
                string filePath = Path.Combine(rootPath, $"{name}");
                
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    // 复制文件
                    file.CopyTo(fs);
                    // 清空缓冲区数据
                    fs.Flush();
                }
                
                List<UserVMIn> users=new List<UserVMIn>();
                
                using (ExcelPackage ep = new ExcelPackage(new FileInfo(filePath)))
                {
                    foreach (var ws in ep.Workbook.Worksheets)
                    {
                        int colStart = ws.Dimension.Start.Column;  //工作区开始列,start=1
                        int colEnd = ws.Dimension.End.Column;       //工作区结束列
                        int rowStart = ws.Dimension.Start.Row;       //工作区开始行号,start=1
                        int rowEnd = ws.Dimension.End.Row;       //工作区结束行号
                    
                        for (int i = rowStart + 1; i <= rowEnd; i++) //第1行是列名,跳过
                        {
                            var user=new UserVMIn();
                            user.UserName = ws.Cells[i, colStart].Text;
                            user.LoginName=ws.Cells[i, colStart+1].Text;
                            user.Password = ws.Cells[i, colStart + 2].Text;
                            users.Add(user);
                        }
                    }
                }
                await _userAppService.AddUsers(users);
            }
        }
    }
}
