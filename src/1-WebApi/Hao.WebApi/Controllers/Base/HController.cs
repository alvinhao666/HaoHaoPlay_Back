using Hao.Core;
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
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Hao.RunTimeException;
using Hao.Log;
using System.Text.Json;

namespace Hao.WebApi
{
    [Authorize]
    public class HController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ICurrentUser CurrentUser { get; set; }

        public IOptionsSnapshot<AppSettingsInfo> appsettingsOptions { get; set; }

        /// <summary>
        /// 在进入方法之前 获取用户jwt中用户信息, 先执行这个方法 再执行模型验证
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            #region 若需要Claims里面其他信息，则取消注释
            //var tokenHeader = HttpContext.Request.Headers["Authorization"];

            //var strToken = tokenHeader.ToString();
            //if (strToken.Contains("Bearer "))
            //{
            //    var jwtHandler = new JwtSecurityTokenHandler();
            //    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(strToken.Remove(0, 7)); //去除"Bearer "
            //    var identity = new ClaimsIdentity(jwtToken.Claims);
            //    var principal = new ClaimsPrincipal(identity);
            //    HttpContext.User = principal;
            //}
            #endregion

            var userId = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid); //Security Identifiers安全标识符
            if (userId == null) throw new HException(ErrorInfo.E100003, nameof(ErrorInfo.E100003).GetErrorCode());

            var traceId = context.HttpContext.TraceIdentifier;
            var path = context.HttpContext.Request.Path.Value;

            _logger.Info(new HLog()
            {
                Method = path,
                Argument = new
                {
                    TraceIdentifier = traceId,
                    UserId = userId.Value,
                    context.ActionArguments
                },
                Description = "请求信息"
            });

            var value = RedisHelper.Get(appsettingsOptions.Value.RedisPrefixOptions.LoginInfo + userId.Value);

            if (value == null) throw new HException(ErrorInfo.E100003, nameof(ErrorInfo.E100003).GetErrorCode());


            var cacheUser = JsonSerializer.Deserialize<RedisCacheUserInfo>(value);

            //当前用户信息
            CurrentUser.UserId = cacheUser.Id;
            CurrentUser.UserName = cacheUser.UserName;

            base.OnActionExecuting(context);
        }



        protected async Task<HttpResponseMessage> DownFile(string filePath, string fileName)
        {
            return await Task.Factory.StartNew(() =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
#if DEBUG

                Stream stream = new FileStream(filePath, FileMode.Open);
                response.Content = new StreamContent(stream);
#else
                response.Content = new StringContent("");
#endif
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;

#if DEBUG
#else
                response.Content.Headers.Add("X-Accel-Redirect", $"/Api/ExportExcel/{fileName}");
#endif
                return response;
            });

        }

        ///// <summary>
        ///// 读取body参数    （3.0 用的 新json api 会自动验证参数类型 转换不通过会报错  不需要此方法验证)
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
