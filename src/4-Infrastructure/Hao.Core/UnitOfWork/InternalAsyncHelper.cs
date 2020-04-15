//using System;
//using System.Reflection;
//using System.Threading.Tasks;

//namespace Hao.Core
//{
//    internal static class InternalAsyncHelper
//    {
//        /// <summary>
//        /// 异步无返回值处理
//        /// </summary>
//        /// <param name="actualReturnValue"></param>
//        /// <param name="postAction"></param>
//        /// <param name="finalAction"></param>
//        /// <returns></returns>
//        public static async Task AwaitTaskWithPostActionAndFinally(Task actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
//        {
//            Exception exception = null;

//            try
//            {
//                // 等待原有任务执行完成
//                await actualReturnValue;
//                // 执行 postAction() 表示本工作单元已经顺利执行
//                await postAction();
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//            }
//            finally
//            {
//                finalAction(exception);
//            }
//        }

//        /// <summary>
//        /// 原理基本同上，只是多了一个返回值
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="actualReturnValue"></param>
//        /// <param name="postAction"></param>
//        /// <param name="finalAction"></param>
//        /// <returns></returns>
//        public static async Task<T> AwaitTaskWithPostActionAndFinallyAndGetResult<T>(Task<T> actualReturnValue, Func<Task> postAction, Action<Exception> finalAction)
//        {
//            Exception exception = null;

//            try
//            {
//                var result = await actualReturnValue;
//                await postAction();
//                return result;
//            }
//            catch (Exception ex)
//            {
//                exception = ex;
//                throw;
//            }
//            finally
//            {
//                finalAction(exception);
//            }
//        }

//        /// <summary>
//        /// 异步有返回值处理
//        /// </summary>
//        /// <param name="taskReturnType"></param>
//        /// <param name="actualReturnValue"></param>
//        /// <param name="action"></param>
//        /// <param name="finalAction"></param>
//        /// <returns></returns>
//        public static object CallAwaitTaskWithPostActionAndFinallyAndGetResult(Type taskReturnType, object actualReturnValue, Func<Task> action, Action<Exception> finalAction)
//        {
//            //这里通过反射获取到 AwaitTaskWithPostActionAndFinallyAndGetResult 方法，并调用。
//            return typeof(InternalAsyncHelper)
//                .GetMethod("AwaitTaskWithPostActionAndFinallyAndGetResult", BindingFlags.Public | BindingFlags.Static)
//                .MakeGenericMethod(taskReturnType)
//                .Invoke(null, new object[] { actualReturnValue, action, finalAction });
//        }
//    }
//}
