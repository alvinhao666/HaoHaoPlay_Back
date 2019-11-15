using Hao.EventData;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.Event
{
    public interface ILoginEventHandler
    {
        Task UpdateLogin(LoginEventData person);
    }
}
