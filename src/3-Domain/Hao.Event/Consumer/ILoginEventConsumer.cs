using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 登录
    /// </summary>
    public interface ILoginEventConsumer
    {
        Task UpdateLogin(LoginEventData data);
    }
}
