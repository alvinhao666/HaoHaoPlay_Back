using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using DotNetCore.CAP;
using Hao.Runtime;
using Mapster;

namespace Hao.Core
{
    public abstract class EventSubscriber<T>: ICapSubscribe where T: H_EventData
    {
        [FromServiceContext]
        public ICurrentUser CurrentUser { get; set; }

        /// <summary>
        /// ¶©ÔÄ
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public abstract Task Subscribe(T eventData);


        public void InitCurrentUser(T eventData)
        {
            if (eventData?.CurrentUser == null) return;

            var user = CurrentUser as CurrentUser;

            user = eventData.CurrentUser.Adapt(user);
        }
    }
}