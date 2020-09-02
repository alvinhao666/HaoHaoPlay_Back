using Hao.Json;
using Hao.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Hao.Core.Extensions
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class H_Controller : Controller
    {
        protected readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        ////IOptionsSnapshot修改即更新  和IConfiguration效果一样  热更新
        //public IOptionsSnapshot<H_AppSettingsConfig> AppsettingsOptions { get; set; } //属性注入必须public

        /// <summary>
        /// 控制器中的操作执行之前调用此方法（先执行这个方法 再执行模型验证）
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var userId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value; //Security Identifiers安全标识符

            var jti = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var ip = HttpContext.GetIp();

            var argument = new { context.HttpContext.TraceIdentifier, UserId = userId, context.ActionArguments, IP = ip };

            Logger.Info(new H_Log{ Method = context.HttpContext.Request.Path.Value, Argument = argument, Description = "请求信息" });

            var cache = GetCacheUser(userId, jti);

            //if (ip != cache.Ip) throw new H_Exception("请重新登录", nameof(H_Error.E100004).GetErrorCode());

            CheckAuth(context, cache.AuthNumbers);

            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 控制器中的操作执行后调用此方法 再执行H_ResultFilter全局过滤器
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            object result = null; //EmptyResult对应null

            if (context.Result is ObjectResult)
            {
                result = (context.Result as ObjectResult)?.Value;
            }
            else if (context.Result is JsonResult)
            {
                result = (context.Result as JsonResult)?.Value;
            }

            var param = new
            {
                context.HttpContext.TraceIdentifier,
                UserId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value,
                Result = result
            };

            Logger.Info(new H_Log{ Method = HttpContext.Request.Path.Value, Argument = param, Description = "返回信息" });

            base.OnActionExecuted(context);
        }


        /// <summary>
        /// 获取登录缓存的用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        private H_RedisCacheUser GetCacheUser(string userId, string jti)
        {
            var config = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            var prefix = config["RedisPrefix:Login"];

            var value = RedisHelper.Get($"{prefix}{userId}_{jti}");

            if (string.IsNullOrWhiteSpace(value)) throw new H_Exception(H_Error.E100002, nameof(H_Error.E100002).GetErrorCode());

            var cacheUser = H_JsonSerializer.Deserialize<H_RedisCacheUser>(value);

            if (cacheUser?.Id == null) throw new H_Exception(H_Error.E100002, nameof(H_Error.E100002).GetErrorCode());

            if (cacheUser.LoginStatus.HasValue
                && cacheUser.LoginStatus == LoginStatus.Offline
                && cacheUser.IsAuthUpdate.HasValue
                && cacheUser.IsAuthUpdate.Value) throw new H_Exception(H_Error.E100003, nameof(H_Error.E100003).GetErrorCode());

            if (!cacheUser.LoginStatus.HasValue || cacheUser.LoginStatus == LoginStatus.Offline) throw new H_Exception(H_Error.E100002, nameof(H_Error.E100002).GetErrorCode());

            return cacheUser;
        }


        /// <summary>
        /// 检查接口权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthNumbers"></param>
        private void CheckAuth(ActionExecutingContext context, List<long> userAuthNumbers)
        {

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor.MethodInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthCodeAttribute));

            if (attribute != null)
            {
                var authInfos = attribute.ConstructorArguments.FirstOrDefault().Value.ToString().Split('_');

                if (authInfos.Length != 2 || !int.TryParse(authInfos[0], out var layer) || !long.TryParse(authInfos[1], out var authCode)) throw new H_Exception("接口权限值有误，请检查");

                if (userAuthNumbers == null || userAuthNumbers.Count == 0) throw new H_Exception("所拥有的权限值有误，请检查");

                if ((userAuthNumbers[--layer] & authCode) != authCode) throw new H_Exception("没有接口权限");
            }
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
        //                        response.Content = new StringContent("");
        //#endif
        //                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
        //                response.Content.Headers.ContentDisposition.FileName = fileName;

        //#if DEBUG
        //#else
        //                        response.Content.Headers.Add("X-Accel-Redirect", $"/Api/ExportExcel/{fileName}");
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



        [AttributeUsage(AttributeTargets.Method)]
        protected class AuthCodeAttribute : Attribute
        {
            public AuthCodeAttribute(string code)
            {

            }
        }
    }
}
