using Hao.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hao.Http
{
    public class HttpProvider : IHttpProvider
    {
        private readonly IHttpClientFactory _httpFactory;

        public HttpProvider(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        /// <summary>
        /// Post提交 需要用[FromBody]接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<TResult> Post<T, TResult>(string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new()
        {
            var json = H_JsonSerializer.Serialize(t);

            var httpClient = _httpFactory.CreateClient();
            httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);

            var response = await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var result = H_JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }

        /// <summary>
        /// Post提交 需要用[FromForm]接收
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<TResult> Post<TResult>(string url, Dictionary<string, string> dic, int timeoutSeconds = 30) where TResult : new()
        {

            var body = dic.Select(pair => pair.Key + "=" + WebUtility.UrlEncode(pair.Value))
                          .DefaultIfEmpty("") //如果是空 返回 new List<string>(){""};
                          .Aggregate((a, b) => a + "&" + b);

            var stringContent = new StringContent(body, Encoding.UTF8);

            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var httpClient = _httpFactory.CreateClient();
            httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);
            var response = await httpClient.PostAsync(url, stringContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = H_JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public async Task<TResult> Get<TResult>(string url, object obj, int timeoutSeconds = 30)
        {
            var httpClient = _httpFactory.CreateClient();
            httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);

            var response = await httpClient.GetAsync(url + ToUrlParam(obj));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var result = H_JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }



        /// <summary>
        /// 将对象组装成url参数 ?a=1&b=2&c=3&d=4
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string ToUrlParam(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            var count = properties.Length;
            var index = 1;
            StringBuilder sb = new StringBuilder();
            sb.Append("?");
            foreach (var p in properties)
            {
                var v = p.GetValue(obj, null);

                if (v == null) continue;

                if (p.PropertyType.IsEnum || IsNullableEnum(p.PropertyType))
                {
                    var enumInt = (int)v;
                    sb.Append($"{p.Name}={enumInt}");
                }
                else
                {
                    sb.Append($"{p.Name}={HttpUtility.UrlEncode(v.ToString())}");
                }

                if (index < count)
                {
                    sb.Append("&");
                    index++;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 判断是否是枚举
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static bool IsNullableEnum(Type t)
        {
            Type u = Nullable.GetUnderlyingType(t);
            return (u != null) && u.IsEnum;
        }
    }
}
