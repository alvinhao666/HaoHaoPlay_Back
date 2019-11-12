using Hao.EventData;
using System.Threading.Tasks;
using Hao.Dependency;

namespace Hao.Event
{
    public interface ILoginEventHandler: ITransientDependency
    {
        Task UpdateLoginTimeAndIP(LoginEventData person);
    }
}
