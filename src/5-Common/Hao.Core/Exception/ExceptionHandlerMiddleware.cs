using Hao.Core.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Threading.Tasks;

namespace Hao.Core
{
    public static class ExceptionHandlerMiddleware
    {

        private const string EErrorMsg = "未知错误";

        public static ILogger _log = LogManager.GetCurrentClassLogger();

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            try
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = Invoke
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex, JsonConvert.SerializeObject(ex.Message));
            }
        }
        
        private static async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var response = new BaseResponse
            {
                Success = false,
                ErrorMsg = EErrorMsg
            };

            var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

            if (ex is HException exception)
            {
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }
            
#if DEBUG
            response.ErrorMsg = ex.Message;
#endif

            var errorLog = new
            {
                context.Request.Path,
                context.TraceIdentifier,
                ex.Message
            };

            _log.Error(ex, JsonConvert.SerializeObject(errorLog));

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
