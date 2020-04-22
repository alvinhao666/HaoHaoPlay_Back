using AutoMapper;
using DotNetCore.CAP;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Encrypt;
using Hao.EventData;
using Hao.Library;
using Hao.Model;
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
    /// <summary>
    /// 登录应用服务
    /// </summary>
    public class LoginAppService : ApplicationService, ILoginAppService
    {
        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        private readonly ISysModuleRepository _moduleRep;

        private readonly AppSettingsInfo _appsettings;

        private readonly ICapPublisher _publisher;

        private readonly HttpContext _httpContext;

        public LoginAppService(IHttpContextAccessor httpContextAccessor,
            ISysUserRepository userRep,
            ISysModuleRepository moduleRep,
            IMapper mapper,
            IOptionsSnapshot<AppSettingsInfo> appsettingsOptions,
            ICapPublisher publisher)
        {
            _userRep = userRep;
            _mapper = mapper;
            _appsettings = appsettingsOptions.Value; //IOptionsSnapshot动态获取配置
            _httpContext = httpContextAccessor.HttpContext;
            _publisher = publisher;
            _moduleRep = moduleRep;
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
            var timeNow = DateTime.Now;

            password = RSAHelper.Decrypt(_appsettings.KeyInfo.RsaPrivateKey, password); //解密

            password = EncryptProvider.HMACSHA256(password, _appsettings.KeyInfo.Sha256Key);

            var user = await GetUser(loginName, password);

            var jwt = CreateJwt(timeNow, user, isRememberLogin);

            if (string.IsNullOrWhiteSpace(user.AuthNumbers)) throw new HException("没有系统权限，暂时无法登录");

            var authNums = JsonSerializer.Deserialize<List<long>>(user.AuthNumbers);

            var modules = await _moduleRep.GetListAysnc(new ModuleQuery() { IncludeResource = false });
            var menus = new List<MenuVM>();
            InitMenuTree(menus, 0, modules, authNums, user.Id); //找主菜单一级 parentId=0

            if (menus.Count == 0) throw new HException("没有系统权限，暂时无法登录");

            //存入redis
            var userValue = new RedisCacheUserInfo
            {
                Id = user.Id,
                Name = user.Name,
                AuthNumbers = authNums,
                Jwt = jwt
            };

            await RedisHelper.SetAsync(_appsettings.RedisPrefixOptions.LoginInfo + user.Id, JsonSerializer.Serialize(userValue));

            await AsyncLoginInfo(user.Id, timeNow);

            return new LoginVM()
            {
                Id = user.Id,
                Name = user.Name,
                FirstNameSpell = user.FirstNameSpell,
                HeadImgUrl = user.HeadImgUrl,
                Jwt = jwt,
                AuthNums = authNums,
                Menus = menus
            };
        }


        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<SysUser> GetUser(string loginName, string password)
        {
            var users = await _userRep.GetListAysnc(new LoginQuery()
            {
                LoginName = loginName,
                Password = password,
            });

            if (users.Count == 0) throw new HException("用户名或密码错误");
            if (users.Count > 1) throw new HException("用户数据异常，存在相同用户");
            var user = users.First();
            if (!user.Enabled.IsTrue()) throw new HException("用户已注销");
            return user;
        }

        /// <summary>
        /// 生成Jwt
        /// </summary>
        /// <param name="timeNow"></param>
        /// <param name="user"></param>
        /// <param name="isRememberLogin"></param>
        /// <returns></returns>
        private string CreateJwt(DateTime timeNow, SysUser user, bool isRememberLogin)
        {
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

            return jwt;
        }

        /// <summary>
        /// 同步登录信息
        /// </summary>
        /// <returns></returns>
        private async Task AsyncLoginInfo(long userId, DateTime loginTime)
        {
            var ip = _httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = _httpContext.Connection.RemoteIpAddress.ToString();
                if (ip == "::1" || ip.Contains("127.0.0.1")) ip = "127.0.0.1";
            }

            await _publisher.PublishAsync(nameof(LoginEventData), new LoginEventData
            {
                UserId = userId,
                LastLoginTime = loginTime,
                LastLoginIP = ip
            });
        }

        /// <summary>
        /// 递归初始化菜单树
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parentID"></param>
        /// <param name="sources"></param>
        /// <param name="authNums"></param>
        /// <param name="userId"></param>
        private void InitMenuTree(List<MenuVM> result, long? parentID, List<SysModule> sources, List<long> authNums, long userId)
        {
            //递归寻找子节点  
            var tempTree = sources.Where(item => item.ParentId == parentID).OrderBy(a => a.Sort).ToList();
            foreach (var item in tempTree)
            {
                if (userId != -1)
                {
                    if (authNums?.Count < 1 || item.Layer.Value > authNums.Count) continue;

                    if ((authNums[item.Layer.Value - 1] & item.Number) != item.Number) continue;
                }

                var node = new MenuVM()
                {
                    Name = item.Name,
                    Icon = item.Icon,
                    RouterUrl = item.RouterUrl,
                    ChildMenus = new List<MenuVM>()
                };
                result.Add(node);
                InitMenuTree(node.ChildMenus, item.Id, sources, authNums, userId);
            }
        }
    }
}
