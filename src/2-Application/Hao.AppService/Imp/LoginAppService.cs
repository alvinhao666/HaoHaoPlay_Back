using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.EventData;
using Hao.Library;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class LoginAppService : ApplicationService, ILoginAppService
    {
        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        private readonly AppSettingsInfo _appsettings;

        private readonly ICapPublisher _publisher;

        private readonly HttpContext _httpContext;

        public LoginAppService(IHttpContextAccessor httpContextAccessor, ISysUserRepository userRep, IMapper mapper, IOptionsSnapshot<AppSettingsInfo> appsettingsOptions, ICapPublisher publisher)
        {
            _userRep = userRep;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value; //IOptionsSnapshot动态获取配置
            _httpContext = httpContextAccessor.HttpContext;
            _publisher = publisher;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="isRememberLogin"></param>
        /// <returns></returns>
        public async Task<LoginVM> Login(string loginName, string password, bool isRememberLogin)
        {
            password = RSAHelper.Decrypt(_appsettings.KeyInfo.RsaPrivateKey, password); //解密

            password = EncryptProvider.HMACSHA256(password, _appsettings.KeyInfo.Sha256Key);

            var users = await _userRep.GetListAysnc(new LoginQuery()
            {
                LoginName = loginName,
                Password = password,
            });

            if (users.Count == 0) throw new HException("用户名或密码错误");
            if (users.Count > 1) throw new HException("用户数据异常，存在相同用户");
            var user = users.First();
            if (!user.Enabled.IsTrue()) throw new HException("用户已注销");

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
            var jwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _appsettings.JwtOptions.Issuer,
                audience: _appsettings.JwtOptions.Audience,
                claims: claims,
                notBefore: timeNow, //生效时间
                expires: timeNow.AddDays(isRememberLogin ? 3 : 1),//过期时间
                signingCredentials: _appsettings.JwtOptions.SigningCredentials
            ));

            //存入redis
            var userValue = new RedisCacheUserInfo
            {
                Id = user.Id.ToLong(),
                Name = user.Name,
                AuthNumbers = string.IsNullOrWhiteSpace(user.AuthNumbers) ? null : JsonSerializer.Deserialize<List<long>>(user.AuthNumbers)
            };
            await RedisHelper.SetAsync(_appsettings.RedisPrefixOptions.LoginInfo + user.Id, JsonSerializer.Serialize(userValue));


            var ip = _httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = _httpContext.Connection.RemoteIpAddress.ToString();
                if (ip == "::1" || ip.Contains("127.0.0.1")) ip = "127.0.0.1";
            }

            await _publisher.PublishAsync(nameof(LoginEventData), new LoginEventData
            {
                UserId = user.Id.ToLong(),
                LastLoginTime = timeNow,
                LastLoginIP = ip
            });

            return new LoginVM() { Id = user.Id, Name = user.Name, Jwt = jwt };
        }
    }
}
