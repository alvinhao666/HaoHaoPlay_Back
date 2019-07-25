using Hao.Core;
using Hao.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core.AppController
{
    public class HController : Controller
    {
        protected static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        protected IConfigurationRoot _config;

        protected ICurrentUser _currentUser;

        public HController( IConfigurationRoot config, ICurrentUser currentUser)
        {
            _config = config;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 在进入方法之前 获取用户jwt中用户信息
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
        
            var traceId = HttpContext.TraceIdentifier;
            var path = HttpContext.Request.Path.Value;

            //用户未登录
            if (userId == null)
            {
                //输入日志
                var preRequest = new
                {
                    TraceIdentifier = traceId,
                    Arguments = context.ActionArguments
                };
                _logger.Info(new LogInfo() { Method = path, Argument = preRequest, Description = "用户未登录" });

                throw new HException(ErrorCode.E005006, nameof(ErrorCode.E005006).GetCode());
            }

            var request = new
            {
                TraceIdentifier = traceId,
                UserId = userId.Value,
                Arguments = context.ActionArguments
            };
            _logger.Info(new LogInfo() { Method = path, Argument = request, Description = "获取jwt用户信息" });

            var value = RedisHelper.Get(_config["LoginCachePrefix"] + userId.Value);
            RedisCacheUser cacheUser = null;
            if (value == null)
                throw new HException(ErrorCode.E100002, nameof(ErrorCode.E100002).GetCode());
            else
                cacheUser = JsonExtensions.DeserializeFromJson<RedisCacheUser>(value);

            //当前用户信息
            _currentUser.UserID = cacheUser.ID;
            _currentUser.UserName = cacheUser.UserName;

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
    }

}
