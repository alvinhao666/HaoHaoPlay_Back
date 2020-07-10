using Hao.AppService.ViewModel;
using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 登录服务接口
    /// </summary>
    public interface ILoginAppService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <param name="isRememberLogin"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        Task<LoginVM> Login(string loginName, string password, bool isRememberLogin, string ip);
    }
}
