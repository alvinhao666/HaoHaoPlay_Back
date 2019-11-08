using Hao.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hao.Dependency;

namespace Hao.Event
{
    public interface IPersonEventHandler: ITransientDependency
    {
        Task CheckReceivedMessage(PersonEventData person);
    }
}
