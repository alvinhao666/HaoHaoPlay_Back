using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.EventData;

namespace Hao.Event
{
    public class PersonEventHandler : IPersonEventHandler, ICapSubscribe
    {
        [CapSubscribe("xxx.services.account.check")]
        public async Task CheckReceivedMessage(PersonEventData person)
        {
            int i = 0;
        }
    }
}
