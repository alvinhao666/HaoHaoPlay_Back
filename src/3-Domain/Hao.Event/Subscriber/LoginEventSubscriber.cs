using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.Event
{
    /// <summary>
    /// 登录事件订阅接口
    /// </summary>
    public interface ILoginEventSubscriber
    {
        Task UpdateLogin(LoginEventData person);
    }

    /// <summary>
    /// 登录事件订阅实现
    /// </summary>
    public class LoginEventSubscriber : ILoginEventSubscriber, ICapSubscribe
    {
        private readonly ILoginEventConsumer _consumer;

        public LoginEventSubscriber(ILoginEventConsumer consumer)
        {
            _consumer = consumer;
        }

        [CapSubscribe(nameof(LoginEventData))]
        public async Task UpdateLogin(LoginEventData data)
        {
            await _consumer.UpdateLogin(data);
        }
    }
}
