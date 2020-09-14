using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录
    /// </summary>
    public interface ILoginEventHandler
    {
        Task UpdateLogin(LoginEventData data);
    }
}
