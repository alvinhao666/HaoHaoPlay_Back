using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件监听
    /// </summary>
    public class LogoutForUpdateAuthEventListener : EventListener<LogoutForUpdateAuthEventData>
    {
        private readonly ILogoutEventSolver _handler;

        public LogoutForUpdateAuthEventListener(ILogoutEventSolver handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LogoutForUpdateAuthEventData))]
        public override async Task Listen(LogoutForUpdateAuthEventData eventData)
        {
            await _handler.LogoutForUpdateAuth(eventData);
        }
    }
}