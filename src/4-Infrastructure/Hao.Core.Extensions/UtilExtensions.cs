using Hao.Utility;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

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
                if (ip.Contains("::ffff") || ip == "::1" || ip.Contains("127.0.0.1"))
                {
                    ip = "127.0.0.1";
                }
#endif
            }

            return ip;
        }
    }
}
