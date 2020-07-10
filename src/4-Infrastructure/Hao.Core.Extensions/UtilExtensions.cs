using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                if (ip.Contains("::ffff")) ip = ip.Split("::ffff:")[1].Trim(); //::ffff:192.168.2.144

                if (ip == "::1" || ip.Contains("127.0.0.1")) ip = "127.0.0.1";
            }

            return ip;
        }
    }
}
