using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;
using Hao.Core;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件订阅实现
    /// </summary>
    public class LogoutEventSubscriber : EventSubscriber<LogoutEventData>
    {
        private readonly ILogoutEventHandler _handler;

        public LogoutEventSubscriber(ILogoutEventHandler handler)
        {
            _handler = handler;
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(LogoutEventData))]
        public override async Task Subscribe(LogoutEventData eventData)
        {
            CurrentUser = eventData.CurrentUser;
            await _handler.Logout(eventData);
        }
    }
}
