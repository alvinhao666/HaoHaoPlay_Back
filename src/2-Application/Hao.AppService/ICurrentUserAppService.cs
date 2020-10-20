using System.Threading.Tasks;

namespace Hao.AppService
{
    /// <summary>
    /// 当前用户服务接口
    /// </summary>
    public interface ICurrentUserAppService
    {

        /// <summary>
        /// 当前用户信息
        /// </summary>
        /// <returns></returns>
        Task<CurrentUserVM> Get();

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task UpdateHeadImg(UpdateHeadImgRequest request);

        /// <summary>
        /// 更新当前用户基本信息
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        Task UpdateBaseInfo(CurrentUserUpdateRequest vm);

        /// <summary>
        /// 更新当前用户密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Task UpdatePassword(string oldPassword, string newPassword);

        /// <summary>
        /// 当前用户的安全信息
        /// </summary>
        /// <returns></returns>
        Task<UserSecurityVM> GetSecurityInfo();

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        Task Logout();
    }
}
