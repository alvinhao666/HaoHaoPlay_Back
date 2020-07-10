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
                //ip = httpContext.Connection.RemoteIpAddress.ToString();

                //if (ip.Contains("::ffff")) ip = ip.Split("::ffff:")[1].Trim(); //::ffff:192.168.2.144

                //if (ip == "::1" || ip.Contains("127.0.0.1")) ip = "127.0.0.1";

                var list = new[] { "127.0.0.1", "::1" };
                var result = httpContext?.Connection?.RemoteIpAddress.SafeString();

                if (string.IsNullOrWhiteSpace(result) || list.Contains(result) || result.Contains("::ffff:"))
                {
                    result = H_Common.IsWindows ? GetLanIp() : GetLanIp(NetworkInterfaceType.Ethernet);
                }

                return result;
            }

            return ip;
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        private static string GetLanIp()
        {
            foreach (var hostAddress in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (hostAddress.AddressFamily == AddressFamily.InterNetwork)
                    return hostAddress.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取局域网IP
        /// </summary>
        /// <param name="type">网络接口类型</param>
        private static string GetLanIp(NetworkInterfaceType type)
        {
            try
            {
                foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.NetworkInterfaceType != type || item.OperationalStatus != OperationalStatus.Up)
                        continue;
                    var ipProperties = item.GetIPProperties();
                    if (ipProperties.GatewayAddresses.FirstOrDefault() == null)
                        continue;
                    foreach (var ip in ipProperties.UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            return ip.Address.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
