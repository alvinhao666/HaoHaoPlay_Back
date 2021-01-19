using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录事件订阅
    /// </summary>
    public class LoginEventListener : EventListener<LoginEventData>
    {
        private readonly ILoginEventHandler _handler;

        public LoginEventListener(ILoginEventHandler handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LoginEventData))]
        public override async Task Subscribe(LoginEventData eventData)
        {
            await _handler.UpdateLogin(eventData);
        }
    }
}
