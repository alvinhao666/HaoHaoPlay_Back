using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    public interface ILogoutEventDataConsumer
    {
        Task Logout(LogoutEventData data);
    }
}
