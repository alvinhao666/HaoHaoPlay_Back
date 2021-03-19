using AspectCore.DynamicProxy;
using Hao.Log;
using Hao.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hao.Core.Extensions
{
    /// <summary>
    /// 异常过滤
    /// </summary>
    internal class H_ExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常处理，不会处理eventbus中的异常
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            var ex = context.Exception;

            var response = new H_Response { Success = false };

            if (ex is H_Exception exception)
            {
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }
            else if (ex is AspectInvocationException aspectException)
            {
                response.ErrorMsg = aspectException.InnerException?.Message;
            }
#if DEBUG  //开发环境 能看具体错误
            else
            {
                response.ErrorMsg = ex.Message;
            }
#endif


            if (string.IsNullOrWhiteSpace(response.ErrorMsg)) response.ErrorMsg = "系统异常";

            context.Result = new JsonResult(response);

            H_Log.Error(ex, new LogNote()
            {
                Location = context.HttpContext.Request.Path.Value,
                TraceId = context.HttpContext.TraceIdentifier,
                Extra = "系统异常信息"
            }); //异常信息，记录到日志中

            base.OnException(context);  //返回结果 不会经过ResultFilter
        }
    }
}