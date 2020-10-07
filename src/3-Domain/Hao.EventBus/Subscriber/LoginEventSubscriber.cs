using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录事件订阅实现
    /// </summary>
    public class LoginEventSubscriber :  ICapSubscribe
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
