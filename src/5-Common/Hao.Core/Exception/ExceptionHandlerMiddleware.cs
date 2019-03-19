using Hao.Core.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Core
{
    public static class ExceptionHandlerMiddleware
    {
        /// <summary>
		/// 未知异常ErrorCode
		/// </summary>
		public static int DEFAULT_ERROR_CODE = -1;

        /// <summary>
        /// 未知异常ErrorMessage
        /// </summary>
        public static string DEFAULT_ERROR_MESSAGE = "未知错误";

        private static ILogger _log;

        private static string _originUrl;

        private static bool _isAllow;


        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger log)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            _log = log;
            return app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = Invoke
            });
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, ILogger log, bool isAllow, string originUrl)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));
            if (log == null)
                throw new ArgumentNullException(nameof(log));

            _isAllow = isAllow;
            _originUrl = originUrl;
            _log = log;
            return app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = Invoke
            });
        }

        public static async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            if (_isAllow)
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", _originUrl);
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            }
            BaseResponse response = new BaseResponse
            {
                Success = false,
                ErrorCode = DEFAULT_ERROR_CODE,
                ErrorMsg = DEFAULT_ERROR_MESSAGE
            };

            Exception ex = context.Features.Get<IExceptionHandlerFeature>().Error;

            if (ex is HException)
            {
                var exception = ex as HException;
                response.ErrorCode = exception.Code;
                response.ErrorMsg = exception.Message;
            }


#if DEBUG
            response.ErrorMsg = ex.Message;
#endif

            var errorLog = new
            {
                Path = context.Request.Path,
                TraceIdentifier = context.TraceIdentifier,
                Message = ex.Message
            };

            _log.Error(ex, JsonConvert.SerializeObject(errorLog));

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));


        }
    }
}
