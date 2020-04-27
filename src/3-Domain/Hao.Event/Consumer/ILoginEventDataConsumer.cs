using Hao.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Event
{
    public interface ILoginEventDataConsumer
    {
        Task UpdateLogin(LoginEventData person);
    }
}
