using Hao.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.RunTimeException
{
    public static class ExceptionHandlerMiddleware
    {
        private readonly static ILogger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Error(ex, JsonSerializer.Serialize(ex.Message));
            }
        }
        
        private static async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var response = new HResponse
            {
                Success = false,
                ErrorMsg = "未知错误"
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

            _logger.Error(ex, JsonSerializer.Serialize(errorLog));

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
