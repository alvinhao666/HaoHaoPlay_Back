using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 登录服务接口
    /// </summary>
    public interface ILoginAppService
    {
        /// <summary>
        /// 账号密码登录
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fromIP"></param>
        /// <returns></returns>
        Task<LoginVM> LoginByAccountPwd(LoginByAccountPwdRequest request, string fromIP);
    }
}
