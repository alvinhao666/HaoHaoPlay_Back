using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录事件订阅接口,事件总线是对观察者（发布-订阅）模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。
    /// </summary>
    public interface ILoginEventSubscriber
    {
        Task UpdateLogin(LoginEventData person);
    }

    /// <summary>
    /// 登录事件订阅实现
    /// </summary>
    public class LoginEventSubscriber : ILoginEventSubscriber, ICapSubscribe
    {
        private readonly ILoginEventHandler _handler;

        public LoginEventSubscriber(ILoginEventHandler handler)
        {
            _handler = handler;
        }

        [CapSubscribe(nameof(LoginEventData))]
        public async Task UpdateLogin(LoginEventData data)
        {
            await _handler.UpdateLogin(data);
        }
    }
}
