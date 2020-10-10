using TencentCloud.Sts.V20180813.Models;

namespace Hao.TencentCloud.Cos
{
    public interface ITencentCosProvider
    {
        /// <summary>
        /// 获取联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        GetFederationTokenResponse GetFederationToken();
    }
}