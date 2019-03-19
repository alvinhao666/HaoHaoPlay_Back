using Hao.EventData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hao.Core.Dependency;

namespace Hao.Event
{
    public interface IPersonEventHandler: ITransientDependency
    {
        Task CheckReceivedMessage(PersonEventData person);
    }
}
