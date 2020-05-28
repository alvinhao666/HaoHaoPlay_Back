using AspectCore.DynamicProxy;
using Hao.Response;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using NLog;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.Core.Extensions
{
    public static class ExceptionMiddleware
    {
        private readonly static ILogger _logger = LogManager.GetCurrentClassLogger();

        public static void UseExceptionMiddleware(this IApplicationBuilder app)
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
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            var response = new H_Response
            {
                Success = false,
                ErrorMsg = "未知错误"
            };

            var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

            if (ex is H_Exception exception)
            {
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }
            else if(ex is AspectInvocationException aspectException) 
            {
                var errorSplit = "--->";

                if (aspectException.Message.Contains(errorSplit))
                {
                    response.ErrorMsg = aspectException.Message.Split(errorSplit)[1].Trim();
                }
            }
            else
            {
                response.ErrorMsg = ex.Message;
            }

            var errorLog = new
            {
                context.Request.Path,
                context.TraceIdentifier,
                ex.Message
            };

            _logger.Error(ex, JsonSerializer.Serialize(errorLog));

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response,new JsonSerializerOptions() {Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping}),
                Encoding.UTF8);
        }
    }
}