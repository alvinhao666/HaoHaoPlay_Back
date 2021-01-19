using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件订阅
    /// </summary>
    public class LogoutEventListener : EventListener<LogoutEventData>
    {
        private readonly ILogoutEventHandler _handler;


        public LogoutEventListener(ILogoutEventHandler handler)
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
