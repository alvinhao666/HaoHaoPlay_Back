using Hao.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Event
{
    public interface ILogoutEventDataHandler
    {
        Task Logout(LogoutEventData data);
    }
}
