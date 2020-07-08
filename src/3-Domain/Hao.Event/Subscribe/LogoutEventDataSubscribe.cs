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

    public class LogoutEventDataSubscribe : ILogoutEventDataSubscribe, ICapSubscribe
    {

        private readonly ILogoutEventDataConsumer _consumer;

        public LogoutEventDataSubscribe(ILogoutEventDataConsumer consumer)
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
