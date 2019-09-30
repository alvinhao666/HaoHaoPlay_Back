using Hao.Core;
using Hao.Library;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;

namespace Hao.WebApi
{
    public class AuthorizeStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly ITimeLimitedDataProtector _protector;


        public AuthorizeStaticFilesMiddleware( RequestDelegate next,IDataProtectionProvider provider)
        {
            _next = next;
            _protector = provider.CreateProtector("fileName")
                .ToTimeLimitedDataProtector();
        }

        public async Task Invoke(HttpContext context)
        {
            //https传输 URL参数肯定是安全的 被加密
            //https传递查询参数肯定是没有问题的，但是不要用来传递可能引发安全问题的敏感信息奥。
            if (!context.Request.Query.ContainsKey("Authorization")||!context.Request.Query.ContainsKey("FileId"))
            {
                throw new HException(ErrorCode.E100001, nameof(ErrorCode.E100001).GetCode());
            }
            
            var fileId = _protector.Unprotect(context.Request.Query["FileId"].ToString());
            var path = context.Request.Path.ToString();
            var pathbase = context.Request.PathBase.ToString();
            if (!path.Contains($"{fileId}.xlsx"))
            {
                throw new HException("不存在", nameof(ErrorCode.E100001).GetCode());
            }

            // 验证用户信息 //TODO
            //var tokenHeader = HttpContext.Request.Query["Authorization"];
            //var strToken = tokenHeader.ToString();
            //if (strToken.Contains("Bearer "))
            //{
            //    var jwtHandler = new JwtSecurityTokenHandler();
            //    JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(strToken.Remove(0, 7)); //去除"Bearer "
            //    var identity = new ClaimsIdentity(jwtToken.Claims);
            //    var principal = new ClaimsPrincipal(identity);
            //    HttpContext.User = principal;
            //}

            await _next(context);
        }

    }
}
