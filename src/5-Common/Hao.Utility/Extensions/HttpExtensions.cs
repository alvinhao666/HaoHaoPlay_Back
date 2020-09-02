using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Utility
{
    public static class HttpExtensions
    {
        /// <summary>
        /// Post提交 需要用[FromBody]接收
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="t"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public static async Task<TResult> Post<T, TResult>(this HttpClient httpClient, string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new()
        {
            var json = H_JsonSerializer.Serialize(t);

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
        /// Get请求
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="obj"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        public static async Task<TResult> Get<T, TResult>(this HttpClient httpClient, string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new()
        {
            httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);

            var response = await httpClient.GetAsync(url + H_Util.ToUrlParam(t));

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                var result = H_JsonSerializer.Deserialize<TResult>(content);

                return result;
            }

            return default;
        }

        ///// <summary>
        ///// Post提交 需要用[FromForm]接收
        ///// </summary>
        ///// <param name="url"></param>
        ///// <param name="dic"></param>
        ///// <param name="timeoutSeconds"></param>
        ///// <returns></returns>
        //public async Task<TResult> Post<TResult>(string url, Dictionary<string, string> dic, int timeoutSeconds = 30) where TResult : new()
        //{

        //    var body = dic.Select(pair => pair.Key + "=" + WebUtility.UrlEncode(pair.Value))
        //                  .DefaultIfEmpty("") //如果是空 返回 new List<string>(){""};
        //                  .Aggregate((a, b) => a + "&" + b);

        //    var stringContent = new StringContent(body, Encoding.UTF8);

        //    stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        //    var httpClient = _httpFactory.CreateClient();
        //    httpClient.Timeout = new TimeSpan(0, 0, timeoutSeconds);
        //    var response = await httpClient.PostAsync(url, stringContent);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();

        //        var result = H_JsonSerializer.Deserialize<TResult>(content);

        //        return result;
        //    }

        //    return default;
        //}
    }
}
