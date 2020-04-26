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

        private readonly ILoginEventDataHandler _handler;


        public LoginEventDataSubscribe(ILoginEventDataHandler handler)
        {
            _handler = handler;
        }

        [CapSubscribe(nameof(LoginEventData))]
        public async Task UpdateLogin(LoginEventData person)
        {
            await _handler.UpdateLogin(person);
        }
    }
}
