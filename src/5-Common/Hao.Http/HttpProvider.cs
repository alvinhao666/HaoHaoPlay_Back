using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.Http
{
    public class HttpProvider : IHttpProvider
    {
        private readonly HttpClient _httpClient;

        public HttpProvider(IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient();
        }

        /// <summary>
        /// Post提交 需要用[FromForm]接受
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public async Task<TResult> Post<TResult>(string url, Dictionary<string, string> dic, int timeoutSeconds = 30) where TResult : new()
        {
            var body = dic.Select(pair => pair.Key + "=" + WebUtility.UrlEncode(pair.Value))
                          .DefaultIfEmpty("") //如果是空 返回 new List<string>(){""};
                          .Aggregate((a, b) => a + "&" + b);
            StringContent c = new StringContent(body, Encoding.UTF8);
            c.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            _httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);
            var response = await _httpClient.PostAsync(url, c);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }

        /// <summary>
        /// Post提交 需要用[FromBody]接受
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<TResult> Post<T, TResult>(string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new()
        {
            var json = JsonSerializer.Serialize(t);

            _httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);

            var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }
    }
}
