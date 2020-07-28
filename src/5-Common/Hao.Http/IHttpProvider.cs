using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Http
{
    public interface IHttpProvider
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
        Task<TResult> Post<T, TResult>(string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new();

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <param name="timeoutSeconds"></param>
        /// <returns></returns>
        Task<TResult> Get<T, TResult>(string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new();
    }
}
