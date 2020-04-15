using Hao.Library;
using Hao.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HaoHaoPlay.ApiHost
{

    /// <summary>
	/// 重写JWT触发函数
	/// </summary>
	public class HJwtBearerEvents : JwtBearerEvents
    {
        #region 暂不需要重写
        ///// <summary>
        ///// 接收时
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public override Task MessageReceived(MessageReceivedContext context)
        //{
        //    context.Token = context.Request.Headers["Authorization"];
        //    return Task.CompletedTask;
        //}

        ///// <summary>
        ///// TokenValidated：在Token验证通过后调用。
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //public override Task TokenValidated(TokenValidatedContext context)
        //{

        //    return Task.CompletedTask;
        //}
        #endregion


        /**注释原因：token过期AuthenticationFailed执行完后 Challenge方法会报System.InvalidOperationException: StatusCode cannot be set because the response has already started**/
        ///// <summary>
        ///// AuthenticationFailed: 认证失败时调用。触发场景：1.token过期（一定）  使用时一定要在 Controller或方法名上加[Authorize]
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>        
        //public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        //{
        //    context.Response.StatusCode = StatusCodes.Status200OK;
        //    context.Response.ContentType = "application/json";
        //    var response = new HResponse()
        //    {
        //        Success = false,
        //        ErrorCode = nameof(ErrorInfo.E100001).GetErrorCode(),
        //        ErrorMsg = ErrorInfo.E100001
        //    };
        //    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        //}


        /// <summary>
        /// Challenge:未授权时调用 ：服务器可以用来针对客户端的请求发送质询(challenge)。 触发场景：1.token值为空（一定） 2.token过期（一定） 2.token值有误 (一定)。 使用时一定要在 Controller或方法名上加[Authorize]
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Challenge(JwtBearerChallengeContext context)
        {
            context.HandleResponse(); //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须，不加的话 控制台里会报异常System.InvalidOperationException: StatusCode cannot be set because the response has already started
            context.Response.StatusCode = StatusCodes.Status200OK;
            context.Response.ContentType = "application/json";
            var response = new HResponse()
            {
                Success = false,
                ErrorCode = nameof(ErrorInfo.E100001).GetErrorCode(),
                ErrorMsg = ErrorInfo.E100001
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
