using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Utility
{
    public class HttpHelper : IHttpHelper
    {
        public readonly IHttpClientFactory HttpFactory;

        public HttpHelper(IHttpClientFactory httpFactory)
        {
            HttpFactory = httpFactory;
        }

        /// <summary>
        /// Post提交
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public async Task<string> Post(string url, Dictionary<string, string> dic, string contentType)
        {
            var body = dic.Select(pair => pair.Key + "=" + WebUtility.UrlEncode(pair.Value))
                          .DefaultIfEmpty("") //如果是空 返回 new List<string>(){""};
                          .Aggregate((a, b) => a + "&" + b);
            StringContent c = new StringContent(body, Encoding.UTF8);
            c.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            var msg = await HttpFactory.CreateClient().PostAsync(url, c);
            var result = await msg.Content.ReadAsStringAsync();
            return result;
        }
    }
}
