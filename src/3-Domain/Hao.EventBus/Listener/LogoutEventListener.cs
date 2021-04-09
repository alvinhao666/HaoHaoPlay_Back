using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件监听
    /// </summary>
    public class LogoutEventListener : EventListener<LogoutEventData>
    {
        private readonly ILogoutEventSolver _handler;


        public LogoutEventListener(ILogoutEventSolver handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LogoutEventData))]
        public override async Task Listen(LogoutEventData eventData)
        {
            InitCurrentUser(eventData);

            await _handler.Logout(eventData);
        }
    }
}
