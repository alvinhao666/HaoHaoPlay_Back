using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Utility
{
    public interface IHttpHelper
    {
        Task<string> Post(string url, Dictionary<string, string> dic, string contentType);
    }
}
