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
        Task Logout(LogoutEventData data);
    }

    public class LogoutEventDataSubscribe : ILogoutEventDataSubscribe, ICapSubscribe
    {

        private readonly ILogoutEventDataConsumer _consumer;

        public LogoutEventDataSubscribe(ILogoutEventDataConsumer consumer)
        {
            _consumer = consumer;
        }

        [CapSubscribe(nameof(LogoutEventData))]
        public async Task Logout(LogoutEventData data)
        {
            await _consumer.Logout(data);
        }
    }
}
