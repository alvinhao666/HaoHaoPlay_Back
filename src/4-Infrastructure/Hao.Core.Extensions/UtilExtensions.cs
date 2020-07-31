using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Hao.Core.Extensions
{
    public static class UtilExtensions
    {
        /// <summary>
        /// 获取ip
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetIp(this HttpContext httpContext)
        {
            var ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress.ToString();

#if DEBUG  
                if (ip.Contains("::ffff:")) return ip.Split("::ffff:")[1];

                if (ip == "::1" || ip.Contains("127.0.0.1")) return "127.0.0.1";
#endif
            }

            return ip;
        }
    }
}
