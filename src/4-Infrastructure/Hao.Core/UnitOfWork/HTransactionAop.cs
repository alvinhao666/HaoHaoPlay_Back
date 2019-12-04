using Castle.DynamicProxy;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hao.Core
{
    /// <summary>
    /// 事务拦截器  继承IInterceptor接口
    /// </summary>
    public class HTransactionAop : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        public HTransactionAop(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            //对当前方法的特性验证
            //如果需要验证
            //true 布尔值，指示特性能否被派生类和重写成员继承

            if (method.GetCustomAttributes(typeof(ApplicationService.UseTransactionAttribute), true).FirstOrDefault() is ApplicationService.UseTransactionAttribute)
            {
                try
                {
                    _unitOfWork.BeginTran();

                    invocation.Proceed();

                    // 异步获取异常，普通的 try catch 外层不能达到目的，毕竟是异步的
                    if (IsAsyncMethod(invocation.Method))
                    {
                        if (invocation.Method.ReturnType == typeof(Task))
                        {
                            invocation.ReturnValue = InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                                (Task)invocation.ReturnValue,
                                async () => await TestActionAsync(invocation),
                                ex =>
                                {
                                    _unitOfWork.RollbackTran();
                                });
                        }
                        else //Task<TResult>
                        {
                            invocation.ReturnValue = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                             invocation.Method.ReturnType.GenericTypeArguments[0],
                             invocation.ReturnValue,
                             async () => await TestActionAsync(invocation),
                             ex =>
                             {
                                 _unitOfWork.RollbackTran();
                             });
                        }
                    }
                    _unitOfWork.CommitTran();
                }
                catch
                {
                    _unitOfWork.RollbackTran();
                }
            }
            else
            {
                invocation.Proceed();//直接执行被拦截方法
            }
        }

        /// <summary>
        /// 判断是否时异步方法或者异步的泛型方法
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(Task) || (method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
        }


        private async Task TestActionAsync(IInvocation invocation)
        {
        }

    }
}
