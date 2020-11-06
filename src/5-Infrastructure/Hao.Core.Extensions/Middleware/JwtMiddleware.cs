using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    internal class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var strToken = context.Request.Headers["Authorization"].ToString();
                if (!strToken.Contains("Bearer ")) throw new H_Exception("Authorization参数格式有误");

                var jwtHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(strToken.Remove(0, 7)); //去除"Bearer "
                var identity = new ClaimsIdentity(jwtToken.Claims);
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;
            }
            await _next(context);
        }
    }
}
