using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Runtime;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件订阅
    /// </summary>
    public class LogoutEventSubscriber : EventSubscriber<LogoutEventData>
    {
        private readonly ILogoutEventHandler _handler;


        public LogoutEventSubscriber(ILogoutEventHandler handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LogoutEventData))]
        public override async Task Subscribe(LogoutEventData eventData)
        {
            InitCurrentUser(eventData);

            await _handler.Logout(eventData);
        }
    }
}
