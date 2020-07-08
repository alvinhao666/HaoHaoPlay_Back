using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.Event
{
    /// <summary>
    /// 登录
    /// </summary>
    public interface ILoginEventSubscribe
    {
        Task UpdateLogin(LoginEventData person);
    }

    public class LoginEventSubscribe : ILoginEventSubscribe, ICapSubscribe
    {

        private readonly ILoginEventConsumer _consumer;


        public LoginEventSubscribe(ILoginEventConsumer consumer)
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
