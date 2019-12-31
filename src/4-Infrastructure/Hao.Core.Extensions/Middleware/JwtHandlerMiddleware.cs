using Hao.RunTimeException;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    public class JwtHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("Authorization")) throw new HException("Header头缺少Authorization参数");

            var strToken = context.Request.Headers["Authorization"].ToString();
            if (!strToken.Contains("Bearer ")) throw new HException("Authorization格式有误");

            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(strToken.Remove(0, 7)); //去除"Bearer "
            var identity = new ClaimsIdentity(jwtToken.Claims);
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
            await _next(context);
        }
    }
}
