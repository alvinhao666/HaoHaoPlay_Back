using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.Core;
using Hao.EventData;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件订阅
    /// </summary>
    public class LogoutForUpdateAuthEventSubcriber : EventSubscriber<LogoutForUpdateAuthEventData>
    {
        private readonly ILogoutEventHandler _handler;

        public LogoutForUpdateAuthEventSubcriber(ILogoutEventHandler handler)
        {
            _handler = handler;
        }


        [CapSubscribe(nameof(LogoutForUpdateAuthEventData))]
        public override async Task Subscribe(LogoutForUpdateAuthEventData eventData)
        {
            await _handler.LogoutForUpdateAuth(eventData);
        }
    }
}