using AutoMapper;
using DotNetCore.CAP;
using Hao.Core;
using Hao.Encrypt;
using Hao.Enum;
using Hao.EventData;
using Hao.Library;
using Hao.Model;
using Hao.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        private readonly ICapPublisher _publisher;

        private readonly HttpContext _httpContext;

        private readonly H_AppSettingsConfig _appsettings;

        private const string _noAuthTip= "没有系统权限，暂时无法登录，请联系管理员";

        public LoginAppService(
            IHttpContextAccessor httpContextAccessor,
            ISysUserRepository userRep,
            ISysModuleRepository moduleRep,
            IMapper mapper,
            ICapPublisher publisher,
            IOptionsSnapshot<H_AppSettingsConfig> appsettingsOptions)
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
        /// <param name="request"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public async Task<LoginVM> Login(LoginRequest request)
        {
            var timeNow = DateTime.Now;
            var expireTime = timeNow.AddDays(request.IsRememberLogin ? 3 : 1);
            //rsa解密
            var password = RsaHelper.Decrypt(_appsettings.Key.RsaPrivateKey, request.Password);
            //sha256加密
            password = EncryptProvider.HMACSHA256(password, _appsettings.Key.Sha256Key); 

            //根据账号密码查询用户
            var user = await GetUserByLoginName(request.LoginName, password);

            if (string.IsNullOrWhiteSpace(user.AuthNumbers)) throw new H_Exception(_noAuthTip);

            var authNums = H_JsonSerializer.Deserialize<List<long>>(user.AuthNumbers);

            if (authNums.Count == 0) throw new H_Exception(_noAuthTip);

            //查询用户菜单
            var modules = await _moduleRep.GetListAysnc(new ModuleQuery { IncludeResource = false });
            var menus = new List<MenuVM>();

            //找主菜单一级 parentId=0
            InitMenuTree(menus, 0, modules, authNums, user.Id); 

            if (menus.Count == 0) throw new H_Exception(_noAuthTip);

            //jwt的唯一身份标识，避免重复
            var jti = Guid.NewGuid().ToString();
            var jwt = CreateJwt(timeNow, expireTime, jti, user);

            //存入redis
            var userValue = new H_RedisCacheUser
            {
                Id = user.Id,
                Name = user.Name,
                AuthNumbers = authNums,
                Jwt = jwt,
                LoginStatus = LoginStatus.Online,
                Ip = request.Ip
            };

            int expireSeconds = (int)expireTime.Subtract(timeNow).Duration().TotalSeconds + 1;
            await RedisHelper.SetAsync($"{_appsettings.RedisPrefix.Login}{user.Id}_{jti}", H_JsonSerializer.Serialize(userValue), expireSeconds);

            //同步登录信息，例如ip等等
            await AsyncLoginInfo(user.Id, timeNow, request.Ip);

            return new LoginVM
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
        /// 登录获取用户
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<SysUser> GetUserByLoginName(string loginName, string password)
        {
            var users = await _userRep.GetUserByLoginName(loginName, password);

            if (users.Count == 0) throw new H_Exception("用户名或密码错误");
            if (users.Count > 1) throw new H_Exception("用户数据异常，存在相同用户");
            var user = users.First();
            if (!user.Enabled.IsTrue()) throw new H_Exception("用户已注销");
            return user;
        }

        /// <summary>
        /// 生成Jwt
        /// </summary>
        /// <param name="timeNow"></param>
        /// <param name="expireTime"></param>
        /// <param name="jti"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CreateJwt(DateTime timeNow, DateTime expireTime,string jti, SysUser user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _appsettings.Jwt.Subject), //主题
                new Claim(JwtRegisteredClaimNames.Jti, jti), //针对当前 token 的唯一标识 jwt的唯一身份标识，避免重复
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, timeNow.Ticks.ToString(), ClaimValueTypes.Integer64), //token 创建时间
                new Claim(H_ClaimsName.Name, user.Name),
                new Claim(H_ClaimsName.RoleLevel, user.RoleLevel.ToString())
            };

            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appsettings.Jwt.SecretKey));

            var jwt = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: _appsettings.Jwt.Issuer,
                audience: _appsettings.Jwt.Audience,
                claims: claims,
                notBefore: timeNow, //生效时间
                expires: expireTime,//过期时间
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256)
            ));

            return jwt;
        }

        /// <summary>
        /// 同步登录信息
        /// </summary>
        /// <returns></returns>
        private async Task AsyncLoginInfo(long userId, DateTime loginTime, string ip)
        {
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
            var tempTree = sources.Where(item => item.ParentId == parentID).OrderBy(a => a.Sort);
            foreach (var item in tempTree)
            {
                if (authNums?.Count < 1 || item.Layer.Value > authNums.Count) continue;

                if ((authNums[item.Layer.Value - 1] & item.Number) != item.Number) continue;

                var node = new MenuVM()
                {
                    Name = item.Name,
                    Icon = item.Icon,
                    RouterUrl = item.RouterUrl,
                    ChildMenus = new List<MenuVM>()
                };
                result.Add(node);
                InitMenuTree(node.ChildMenus, item.Id, sources, authNums, userId);
                if (item.Type == ModuleType.Main &&node.ChildMenus.Count < 1) result.Remove(node);
            }
        }
    }
}
