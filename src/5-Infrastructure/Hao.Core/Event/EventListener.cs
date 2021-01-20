using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using DotNetCore.CAP;
using Hao.Runtime;
using Mapster;

namespace Hao.Core
{
    /// <summary>
    /// 事件监听
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EventListener<T>: ICapSubscribe where T: H_EventData
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        [FromServiceContext]
        public ICurrentUser CurrentUser { get; set; }

        
        /// <summary>
        /// 订阅方法
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        public abstract Task Subscribe(T eventData);


        /// <summary>
        /// 初始化当前用户
        /// </summary>
        /// <param name="eventData"></param>
        public void InitCurrentUser(T eventData)
        {
            if (eventData?.PublishUser == null) return;

            var user = CurrentUser as CurrentUser;

            user = eventData.PublishUser.Adapt(user);
        }
    }
}