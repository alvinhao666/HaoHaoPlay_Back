using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.Event
{
    /// <summary>
    /// 登录
    /// </summary>
    public interface ILoginEventDataSubscribe
    {
        Task UpdateLogin(LoginEventData person);
    }

    public class LoginEventDataSubscribe : ILoginEventDataSubscribe, ICapSubscribe
    {

        private readonly ILoginEventDataConsumer _consumer;


        public LoginEventDataSubscribe(ILoginEventDataConsumer consumer)
        {
            _consumer = consumer;
        }

        [CapSubscribe(nameof(LoginEventData))]
        public async Task UpdateLogin(LoginEventData person)
        {
            await _consumer.UpdateLogin(person);
        }
    }
}
