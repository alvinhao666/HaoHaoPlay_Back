using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Library;
using Hao.Log;
using Hao.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;


namespace Hao.WebApi
{
    [ApiController]
    [Route("[action]")]
    //1.参数绑定策略的自动推断,可以省略[FromBody] 
    //2.行为自定义 像MVC框架的大部分组件一样，ApiControllerAttribute的行为是高度可自定义的。首先，上面说的大部分内容都是可以简单的用 on/off 来切换。具体的设置是在startup方法里面通过ApiBehaviorOptions来实现
    public class LoginController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly AppSettingsInfo _appsettings;

        private readonly IUserAppService _userAppService;

        public LoginController(IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, IUserAppService userService )
        {
            _appsettings = appsettingsOptions.Value;
            _userAppService = userService;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginOut> Login(LoginIn vm) //apicontroller 1.参数绑定策略的自动推断,可以省略[FromBody]
        {
            var query = new UserQuery() { LoginName = vm.LoginName, Password = vm.PassWord, Enabled = true };

            string pwd = RsaHelper.Decrypt(_appsettings.KeyInfo.RsaPrivateKey, query.Password); //解密

            query.Password = EncryptProvider.HMACSHA256(pwd, _appsettings.KeyInfo.Sha256Key);
            var user = await _userAppService.Login(query);

            var timeNow = DateTime.Now;
            var validFrom = timeNow.Ticks;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appsettings.JwtOptions.Subject), //主题
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //针对当前 token 的唯一标识
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, validFrom.ToString(), ClaimValueTypes.Integer64), //token 创建时间
            };
            var jwt = new JwtSecurityToken(
                issuer: _appsettings.JwtOptions.Issuer,
                audience: _appsettings.JwtOptions.Audience,
                claims: claims,
                notBefore: timeNow, //生效时间
                expires: timeNow.AddDays(3),//过期时间
                signingCredentials: _appsettings.JwtOptions.SigningKey
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            user.JwtToken = encodedJwt;


            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
                if (ip == "::1") ip = "127.0.0.1";
            }
            await _userAppService.UpdateLoginTimeAndIP(user.Id.Value, DateTime.Now, ip);

            //存入redis
            var userValue = new RedisCacheUserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                LoginName = user.LoginName
            };
            await RedisHelper.SetAsync(_appsettings.RedisPrefixOptions.LoginInfo + user.Id, JsonSerializer.Serialize(userValue));

            _logger.Info(new HLog() { Method = "Login", Argument = query.LoginName, Description = "登录成功" });

            return user;
        }
    }
}
