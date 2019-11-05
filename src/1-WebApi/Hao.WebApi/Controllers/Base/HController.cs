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
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Hao.WebApi
{
    [Authorize]
    public class HController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ICurrentUser CurrentUser { get; set; }

        public IOptions<RedisPrefixOptions> RedisPrefix { get; set; }

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
            var body = ReadBodyJson(context);

            _logger.Info(new LogInfo()
            {
                Method = path,
                Argument = new
                {
                    TraceIdentifier = traceId,
                    UserId = userId.Value,
                    Query = context.HttpContext.Request.QueryString,
                    Body = body
                },
                Description = "请求信息"
            });

            var value = RedisHelper.Get(RedisPrefix.Value.LoginInfo + userId.Value);

            if (value == null) throw new HException(ErrorInfo.E100003, nameof(ErrorInfo.E100003).GetErrorCode());


            var cacheUser = JsonExtensions.DeserializeFromJson<RedisCacheUserInfo>(value);

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

        private JObject ReadBodyJson(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;
            string method = request.Method.ToLower();
            if (request.Body != null && request.Body.CanRead
                && (method.Equals("post") || method.Equals("put") || method.Equals("delete"))
                && request.ContentType != null && request.ContentType.Contains("application/json"))
            {
                request.EnableBuffering();
                request.Body.Seek(0, 0);
                string result = null;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                var parameters = context.ActionDescriptor.Parameters;
                var parameter = parameters.FirstOrDefault(a => a.BindingInfo?.BindingSource == BindingSource.Body);
                if (parameter != null && context.ActionArguments != null && !context.ActionArguments.ContainsKey(parameter.Name))
                {
                    _logger.Info(new LogInfo() { Method = context.HttpContext.Request.Path.Value, Argument = result, Description = "RequestBodyContent" });
                    throw new HException(ErrorInfo.E100011, nameof(ErrorInfo.E100011).GetErrorCode());
                }
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return JObject.Parse(result);
                }
            }
            return null;
        }
    }
}
