using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Http
{
    public interface IHttpProvider
    {
        Task<TResult> Post<TResult>(string url, Dictionary<string, string> dic, int timeoutSeconds = 30) where TResult : new();

        Task<TResult> Post<T, TResult>(string url, T t, int timeoutSeconds = 30) where T : new() where TResult : new();
    }
}
