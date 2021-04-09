using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 登录事件监听
    /// </summary>
    public class LoginEventListener : EventListener<LoginEventData>
    {
        private readonly ILoginEventSolver _handler;

        public LoginEventListener(ILoginEventSolver handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LoginEventData))]
        public override async Task Listen(LoginEventData eventData)
        {
            await _handler.UpdateLogin(eventData);
        }
    }
}
