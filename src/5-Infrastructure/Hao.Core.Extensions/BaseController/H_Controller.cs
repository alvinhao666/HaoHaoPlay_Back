using Hao.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Hao.Runtime;
using Newtonsoft.Json;
using Mapster;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hao.Core.Extensions
{
    /// <summary>
    /// Controller基类
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class H_Controller : Controller
    {
        //public IOptionsSnapshot<H_AppSettingsConfig> AppsettingsOptions { get; set; } //属性注入必须public IOptionsSnapshot修改即更新  和IConfiguration效果一样  热更新

        /// <summary>
        /// 控制器中的操作执行之前调用此方法（先执行这个方法 再执行模型验证）
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            var userId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value; //Security Identifiers安全标识符

            var jti = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

            var ip = context.HttpContext.GetIp();
            
            var servicesParams = context.ActionDescriptor.Parameters.Where(a => a.BindingInfo.BindingSource == BindingSource.Services).Select(a => a.Name);

            H_Log.Info(new LogConent
            {
                Location = context.HttpContext.Request.Path.Value,
                Data = servicesParams.Any() ? context.ActionArguments.Where(a => !servicesParams.Contains(a.Key)) : context.ActionArguments,
                TraceId = context.HttpContext.TraceIdentifier,
                UserId = userId,
                IP = ip,
                Extra = "请求信息"
            });

            var cache = GetCacheUser(userId, jti);

            //if (ip != cache.Ip) throw new H_Exception("请重新登录", nameof(H_Error.E100004).GetErrorCode());

            CheckAuth(context, cache.AuthNums);
            
            var currentUser = context.HttpContext.RequestServices.GetService(typeof(ICurrentUser)) as CurrentUser;

            currentUser = cache.Adapt(currentUser);

            if (currentUser.Id == 2)
            {
                var method = context.HttpContext.Request.Method.ToLower();

                var isLogout = context.HttpContext.Request.Path.Value == "/CurrentUser/Logout";

                if (!isLogout)
                {
                    if (method.Equals("post") || method.Equals("put") || method.Equals("delete")) throw new H_Exception("游客账户，无法进行数据操作");
                }
            }

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

            var userId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid)?.Value;

            H_Log.Info(new LogConent
            {
                Location = HttpContext.Request.Path.Value,
                Data = result,
                TraceId = context.HttpContext.TraceIdentifier,
                UserId = userId,
                Extra = "响应结果"
            });

            base.OnActionExecuted(context);
        }


        /// <summary>
        /// 获取登录缓存的用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        private H_CacheUser GetCacheUser(string userId, string jti)
        {
            var config = HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            var prefix = config["RedisPrefix:Login"];

            var value = RedisHelper.Get($"{prefix}{userId}_{jti}");

            if (string.IsNullOrWhiteSpace(value)) throw new H_Exception(H_Error.E100002);

            H_CacheUser cacheUser;

            try
            {
                cacheUser = JsonConvert.DeserializeObject<H_CacheUser>(value);
            }
            catch
            {
                throw new H_Exception(H_Error.E100002);
            }

            if (cacheUser == null) throw new H_Exception(H_Error.E100002);

            if (cacheUser.LoginStatus == LoginStatus.Offline)
            {
                if (cacheUser.IsAuthUpdate) throw new H_Exception(H_Error.E100003);

                throw new H_Exception(H_Error.E100002);
            }

            return cacheUser;
        }


        /// <summary>
        /// 检查接口权限
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userAuthNumbers"></param>
        private void CheckAuth(ActionExecutingContext context, List<long> userAuthNumbers)
        {
            H_AssertEx.That(userAuthNumbers == null || userAuthNumbers.Count == 0, "您所拥有的权限值有误，请检查");

            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var attribute = descriptor.MethodInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType == typeof(AuthCodeAttribute));

            if (attribute != null)
            {
                var auths = attribute.ConstructorArguments.FirstOrDefault().Value.ToString().Split('_');

                H_AssertEx.That(auths.Length < 2, "接口权限值有误，请检查");

                var count = auths.Length;

                int layer = 0;
                long authNum = 0L;

                H_AssertEx.That(!int.TryParse(auths[count - 2], out layer) || !long.TryParse(auths[count - 1], out authNum), "接口权限值有误，请检查");

                H_AssertEx.That((userAuthNumbers[--layer] & authNum) != authNum, "没有接口权限");
            }
        }

        /// <summary>
        /// 权限值特性
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        protected class AuthCodeAttribute : Attribute
        {
            public AuthCodeAttribute(string code)
            {

            }
        }


//        protected async Task<HttpResponseMessage> DownFile(string filePath, string fileName)
//        {
//            return await Task.Factory.StartNew(() =>
//            {
//                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
//#if DEBUG

//                        Stream stream = new FileStream(filePath, FileMode.Open);
//                response.Content = new StreamContent(stream);
//#else
//                                response.Content = new StringContent("");
//#endif
//                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
//                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
//                response.Content.Headers.ContentDisposition.FileName = fileName;

//#if DEBUG
//#else
//                                response.Content.Headers.Add("X-Accel-Redirect", $"/Api/ExportExcel/{fileName}");
//#endif
//                        return response;
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
