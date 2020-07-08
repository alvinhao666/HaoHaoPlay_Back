using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 注销
    /// </summary>
    public interface ILogoutEventConsumer
    {
        Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data);
    }
}
