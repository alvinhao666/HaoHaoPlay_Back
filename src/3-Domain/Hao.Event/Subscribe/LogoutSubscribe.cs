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

    public interface ILogoutSubscribe
    {
        Task Logout(LogoutEventData data);
    }

    public class LogoutSubscribe : ILogoutSubscribe, ICapSubscribe
    {

        private readonly ILogoutEventDataHandler _handler;

        public LogoutSubscribe(ILogoutEventDataHandler handler)
        {
            _handler = handler;
        }

        [CapSubscribe(nameof(LogoutEventData))]
        public async Task Logout(LogoutEventData data)
        {
            await _handler.Logout(data);
        }
    }
}
