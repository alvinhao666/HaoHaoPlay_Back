using DotNetCore.CAP;
using Hao.EventData;
using System.Threading.Tasks;

namespace Hao.Event
{
    /// <summary>
    /// 注销
    /// </summary>
    public interface ILogoutEventDataSubscribe
    {
        Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data);
    }

    public class LogoutEventSubscribe : ILogoutEventDataSubscribe, ICapSubscribe
    {

        private readonly ILogoutEventConsumer _consumer;

        public LogoutEventSubscribe(ILogoutEventConsumer consumer)
        {
            _consumer = consumer;
        }

        [CapSubscribe(nameof(LogoutForUpdateAuthEventData))]
        public async Task LogoutForUpdateAuth(LogoutForUpdateAuthEventData data)
        {
            await _consumer.LogoutForUpdateAuth(data);
        }
    }
}
