using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Hao.Core.Filter
{
    public class JwtAuthorizationFilter
    {
        private readonly RequestDelegate _next;

        public JwtAuthorizationFilter(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            ////检测是否包含'Authorization'请求头，如果不包含则直接放行
            //if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            //    return _next(httpContext);

            //var tokenHeader = httpContext.Request.Headers["Authorization"];

            if (!httpContext.Request.Headers.ContainsKey("token"))
                return _next(httpContext);

            var tokenHeader = httpContext.Request.Headers["token"];

            var jwtHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(tokenHeader.ToString());

            var identity = new ClaimsIdentity(jwtToken.Claims);
            var principal = new ClaimsPrincipal(identity);
            httpContext.User = principal;

            return _next(httpContext);
        }
    }
}
