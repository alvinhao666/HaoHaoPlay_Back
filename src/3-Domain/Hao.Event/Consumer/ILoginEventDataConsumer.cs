using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    public interface ILoginEventDataConsumer
    {
        Task UpdateLogin(LoginEventData person);
    }
}
