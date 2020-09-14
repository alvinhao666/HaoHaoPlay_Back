using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.EventBus
{
    /// <summary>
    /// 注销事件订阅接口
    /// </summary>
    public interface ILogoutEventSubscriber
    {
        Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data);

        Task Logout(LogoutEventData data);
    }

    /// <summary>
    /// 注销事件订阅实现
    /// </summary>
    public class LogoutEventSubscriber : ILogoutEventSubscriber, ICapSubscribe
    {

        private readonly ILogoutEventConsumer _consumer;

        public LogoutEventSubscriber(ILogoutEventConsumer consumer)
        {
            _consumer = consumer;
        }

        /// <summary>
        /// 注销-权限更新
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(LogoutForUpdateAuthEventData))]
        public async Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data)
        {
            await _consumer.LogoutForUpdateAuth(data);
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [CapSubscribe(nameof(LogoutEventData))]
        public async Task Logout(LogoutEventData data)
        {
            await _consumer.Logout(data);
        }
    }
}
