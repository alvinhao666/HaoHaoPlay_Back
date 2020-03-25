using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.AppService;
using Hao.Core;
using Hao.EventData;
using Hao.Model;
using Hao.Repository;

namespace Hao.Event
{
    public class LoginEventHandler : HEventHandler, ILoginEventHandler, ICapSubscribe
    {

        private readonly IUserAppService _userService;


        public LoginEventHandler(IUserAppService userService)
        {
            _userService = userService;
        }

        [CapSubscribe(nameof(LoginEventData))]
        public async Task UpdateLogin(LoginEventData person)
        {
            await _userService.UpdateLogin(person.UserId.Value, person.LastLoginTime, person.LastLoginIP);
        }
    }
}
