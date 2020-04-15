using AutoMapper;
using Hao.AppService.ViewModel;
using Hao.Core;
using Hao.Repository;
using Hao.RunTimeException;
using Hao.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace Hao.AppService
{
    public class LoginAppService : ApplicationService, ILoginAppService
    {
        private readonly IMapper _mapper;

        private readonly ISysUserRepository _userRep;

        public LoginAppService(ISysUserRepository userRep, IMapper mapper)
        {
            _userRep = userRep;
            _mapper = mapper;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<LoginVM> Login(LoginQuery query)
        {
            var users = await _userRep.GetListAysnc(query);
            if (users.Count == 0)
                throw new HException("用户名或密码错误");
            var user = users.FirstOrDefault();
            if (!user.Enabled.IsTrue())
                throw new HException("用户已注销");
            return _mapper.Map<LoginVM>(user);
        }
    }
}
