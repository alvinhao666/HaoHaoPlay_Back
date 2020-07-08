using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 注销
    /// </summary>
    public interface ILogoutEventDataConsumer
    {
        Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data);
    }
}
