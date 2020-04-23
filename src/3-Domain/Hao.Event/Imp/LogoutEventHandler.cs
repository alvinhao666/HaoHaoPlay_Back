using DotNetCore.CAP;
using Hao.AppService;
using Hao.Core;
using Hao.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Event
{
    public class LogoutEventHandler : HEventHandler, ILogoutEventHandler, ICapSubscribe
    {

        private readonly ILogoutAppService _logoutService;

        public LogoutEventHandler(ILogoutAppService logoutService)
        {
            _logoutService = logoutService;
        }

        [CapSubscribe(nameof(LogoutEventData))]
        public async Task Logout(LogoutEventData person)
        {
            foreach(var id in person.UserIds)
            {
                await _logoutService.LogoutByUpdateAuth(id);
            }
        }
    }
}
