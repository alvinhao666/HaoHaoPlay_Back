using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.AppService;
using Hao.EventData;

namespace Hao.Event
{

    public interface ILoginSubscribe
    {
        Task UpdateLogin(LoginEventData person);
    }

    public class LoginSubscribe : ILoginSubscribe, ICapSubscribe
    {

        private readonly ILoginEventDataHandler _handler;


        public LoginSubscribe(ILoginEventDataHandler handler)
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
