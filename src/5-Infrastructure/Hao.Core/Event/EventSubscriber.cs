using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using DotNetCore.CAP;
using Hao.Runtime;

namespace Hao.Core
{
    public abstract class EventSubscriber<T>: ICapSubscribe where T: H_EventData
    {
        [FromServiceContext]
        protected ICurrentUser CurrentUser { get; set; }
        
        public abstract Task Subscribe(T eventData);
    }
}