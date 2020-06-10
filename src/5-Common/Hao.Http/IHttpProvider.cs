using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hao.Utility
{
    public interface IHttpProvider
    {
        Task<TResult> Post<TResult>(string url, Dictionary<string, string> dic, int timeoutSeconds, string contentType) where TResult : new();

        Task<TResult> Post<T, TResult>(string url, T t, int timeoutSeconds, string contentType) where T : new() where TResult : new();
    }
}
