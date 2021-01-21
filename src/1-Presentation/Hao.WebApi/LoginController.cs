using System.Threading.Tasks;
using AspectCore.DependencyInjection;
using Hao.AppService;
using Hao.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Hao.WebApi
{
    /// <summary>
    /// 登录
    /// </summary>
    [ApiController]
    [Route("[action]")]
    //1.参数绑定策略的自动推断,可以省略[FromBody] 
    //2.行为自定义 像MVC框架的大部分组件一样，ApiControllerAttribute的行为是高度可自定义的。首先，上面说的大部分内容都是可以简单的用 on/off 来切换。具体的设置是在startup方法里面通过ApiBehaviorOptions来实现
    public class LoginController : ControllerBase
    {
        private readonly ILoginAppService _loginAppService;

        public LoginController(ILoginAppService loginService)
        {
            _loginAppService = loginService; //顺序2
        }

        /// <summary>
        /// 账号密码登录 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginVM> LoginByAccountPwd(LoginByAccountPwdRequest request)
        {
            string ip = HttpContext.GetIp();

            Log.Information(new H_Log() { Method = "LoginByAccountPwd", Data = request , Description = $"登录请求TraceId：{HttpContext.TraceIdentifier}，IP：{ip}" }.ToString());

            var result = await _loginAppService.LoginByAccountPwd(request, ip);

            Log.Information(new H_Log() { Method = "LoginByAccountPwd", Data = result, Description = $"登录返回TraceId：{HttpContext.TraceIdentifier}" }.ToString());

            return result;
        }
    }
}
