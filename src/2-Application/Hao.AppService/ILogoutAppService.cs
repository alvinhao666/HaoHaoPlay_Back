using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 注销服务接口
    /// </summary>
    public interface ILogoutAppService
    {
        /// <summary>
        /// 注销当前登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jti"></param>
        /// <returns></returns>
        Task Logout(long userId, string jti);
    }
}
