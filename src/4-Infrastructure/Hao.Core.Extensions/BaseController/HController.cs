﻿using Hao.Core;
using Hao.Library;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Hao.RunTimeException;
using System.Text.Json;
using System.Security.Claims;

namespace Hao.Core.Extensions
{
    [Authorize]
    public class HController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ICurrentUser CurrentUser { get; set; }

        public IOptionsSnapshot<AppSettingsInfo> AppsettingsOptions { get; set; }

        /// <summary>
        /// 在进入方法之前 获取用户jwt中用户信息, 先执行这个方法 再执行模型验证
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var userId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value; //Security Identifiers安全标识符
            if (userId == null) throw new HException(ErrorInfo.E100003, nameof(ErrorInfo.E100003).GetErrorCode());

            var traceId = context.HttpContext.TraceIdentifier;
            var path = context.HttpContext.Request.Path.Value;

            _logger.Info(new
            {
                Method = path,
                Argument = new
                {
                    TraceIdentifier = traceId,
                    UserId = userId,
                    context.ActionArguments
                },
                Description = "请求信息"
            });

            var value = RedisHelper.Get(AppsettingsOptions.Value.RedisPrefixOptions.LoginInfo + userId);

            if (value == null) throw new HException(ErrorInfo.E100003, nameof(ErrorInfo.E100003).GetErrorCode());

            //var cacheUser = JsonSerializer.Deserialize<RedisCacheUserInfo>(value);

            base.OnActionExecuting(context);
        }



//        protected async Task<HttpResponseMessage> DownFile(string filePath, string fileName)
//        {
//            return await Task.Factory.StartNew(() =>
//            {
//                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
//#if DEBUG

//                Stream stream = new FileStream(filePath, FileMode.Open);
//                response.Content = new StreamContent(stream);
//#else
//                response.Content = new StringContent("");
//#endif
//                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
//                response.Content.Headers.ContentDisposition.FileName = fileName;

//#if DEBUG
//#else
//                response.Content.Headers.Add("X-Accel-Redirect", $"/Api/ExportExcel/{fileName}");
//#endif
//                return response;
//            });

//        }

        ///// <summary>
        ///// 读取body参数    （.net core 3.0 system.text.json不支持隐式转换   转换不通过会报错  不需要此方法验证)
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private async Task<JObject> ReadBodyJson(ActionExecutingContext context)
        //{
        //    var request = context.HttpContext.Request;
        //    string method = request.Method.ToLower();
        //    if (request.Body != null && request.Body.CanRead
        //        && (method.Equals("post") || method.Equals("put") || method.Equals("delete"))
        //        && request.ContentType != null && request.ContentType.Contains("application/json"))
        //    {
        //        request.EnableBuffering();
        //        string result = null;
        //        using (var reader = new StreamReader(request.Body, Encoding.UTF8))
        //        {
        //            result = await reader.ReadToEndAsync();
        //            request.Body.Seek(0, SeekOrigin.Begin);
        //        }
        //        var parameters = context.ActionDescriptor.Parameters;
        //        var parameter = parameters.FirstOrDefault(a => a.BindingInfo?.BindingSource == BindingSource.Body);
        //        if (parameter != null && context.ActionArguments != null && !context.ActionArguments.ContainsKey(parameter.Name))
        //        {
        //            _logger.Info(new HLog() { Method = context.HttpContext.Request.Path.Value, Argument = result, Description = "RequestBodyContent" });
        //            throw new HException(ErrorInfo.E100011, nameof(ErrorInfo.E100011).GetErrorCode());
        //        }
        //        if (!string.IsNullOrWhiteSpace(result))
        //        {
        //            return JObject.Parse(result);
        //        }
        //    }
        //    return null;
        //}
    }
}