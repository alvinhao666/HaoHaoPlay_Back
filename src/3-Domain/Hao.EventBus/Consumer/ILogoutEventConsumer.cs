using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销
    /// </summary>
    public interface ILogoutEventConsumer
    {
        Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data);

        Task Logout(LogoutEventData data);
    }
}
