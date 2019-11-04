using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.Library;
using Hao.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NLog;


namespace Hao.WebApi
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly JwtOptions _jwtOptions;

        private readonly IUserAppService _userAppService;

        private readonly IMapper _mapper;

        private readonly RedisPrefixOptions _redisPrefix;

        private readonly IConfiguration _config;

        public LoginController(IOptions<JwtOptions> jwtOptions, IOptions<RedisPrefixOptions> redisPrefix, IMapper mapper, IUserAppService userService, IConfiguration Configuration)
        {
            _jwtOptions = jwtOptions.Value;
            _redisPrefix = redisPrefix.Value;
            _userAppService = userService;
            _mapper = mapper;
            _config = Configuration;
        }

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="queryInput"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<LoginOut> Login([FromBody]UserQueryInput queryInput)
        {
            var query = _mapper.Map<UserQuery>(queryInput);
            query.Enabled = true;

            string pwd = RsaHelper.Decrypt(_config["KeyInfo:RsaPrivateKey"], query.Password); //解密

            query.Password = EncryptProvider.HMACSHA256(pwd, _config["KeyInfo:Sha256Key"]);
            var user = await _userAppService.Login(query);

            var timeNow = DateTime.Now;
            var validFrom = timeNow.Ticks;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _jwtOptions.Subject), //主题
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //针对当前 token 的唯一标识
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, validFrom.ToString(), ClaimValueTypes.Integer64), //token 创建时间
            };
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: timeNow, //生效时间
                expires: timeNow.AddDays(3),//过期时间
                signingCredentials: _jwtOptions.SigningKey
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            user.JwtToken = encodedJwt;


            var ip = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            }
            await _userAppService.UpdateLoginTimeAndIP(user.Id.Value, DateTime.Now, ip);

            //存入redis
            var userValue = new RedisCacheUserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                LoginName = user.LoginName
            };
            await RedisHelper.SetAsync(_redisPrefix.LoginInfo + user.Id, JsonExtensions.SerializeToJson(userValue));

            _logger.Info(new LogInfo() { Method = "Login", Argument = query.LoginName, Description = "登录成功" }.ToString());

            return user;
        }
    }
}
