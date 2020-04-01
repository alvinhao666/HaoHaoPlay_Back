using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Encrypt;
using Hao.EventData;
using Hao.Library;
using Hao.Log;
using Hao.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;


namespace Hao.WebApi
{
    [ApiController]
    [Route("[controller]")]
    //1.参数绑定策略的自动推断,可以省略[FromBody] 
    //2.行为自定义 像MVC框架的大部分组件一样，ApiControllerAttribute的行为是高度可自定义的。首先，上面说的大部分内容都是可以简单的用 on/off 来切换。具体的设置是在startup方法里面通过ApiBehaviorOptions来实现
    public class LoginController : ControllerBase
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly AppSettingsInfo _appsettings;

        private readonly ILoginAppService _loginAppService;

        private readonly ICapPublisher _publisher;

        public LoginController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, ILoginAppService loginService, ICapPublisher publisher)
        {
            _appsettings = appsettingsOptions.Value; //IOptionsSnapshot动态获取配置
            _publisher = publisher;
            _loginAppService = loginService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginVM> Login(LoginRequest rq) //apicontroller 1.参数绑定策略的自动推断,可以省略[FromBody]
        {
            string pwd = RSAHelper.Decrypt(_appsettings.KeyInfo.RsaPrivateKey, rq.Password); //解密

            pwd = EncryptProvider.HMACSHA256(pwd, _appsettings.KeyInfo.Sha256Key);

            var query = new LoginQuery() { LoginName = rq.LoginName, Password = pwd };
            var user = await _loginAppService.Login(query);

            var timeNow = DateTime.Now;
            var validFrom = timeNow.Ticks;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appsettings.JwtOptions.Subject), //主题
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //针对当前 token 的唯一标识
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, validFrom.ToString(), ClaimValueTypes.Integer64), //token 创建时间
                new Claim(ClaimsName.Name, user.Name)
            };
            var jwt = new JwtSecurityToken(
                issuer: _appsettings.JwtOptions.Issuer,
                audience: _appsettings.JwtOptions.Audience,
                claims: claims,
                notBefore: timeNow, //生效时间
                expires: timeNow.AddDays(3),//过期时间
                signingCredentials: _appsettings.JwtOptions.SigningCredentials
            );
            user.Jwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //存入redis
            var userValue = new RedisCacheUserInfo
            {
                Id = user.Id.ToLong(),
                Name = user.Name,
                LoginName = user.LoginName
            };
            await RedisHelper.SetAsync(_appsettings.RedisPrefixOptions.LoginInfo + user.Id, JsonSerializer.Serialize(userValue));


            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
                if (ip == "::1"|| ip.Contains("127.0.0.1")) ip = "127.0.0.1";
            }

            await _publisher.PublishAsync(nameof(LoginEventData), new LoginEventData
            {
                UserId = user.Id.ToLong(),
                LastLoginTime = timeNow,
                LastLoginIP = ip
            });

            _logger.Info(new HLog() { Method = "Login", Argument = user, Description = "登录成功" });

            return user;
        }
    }
}
