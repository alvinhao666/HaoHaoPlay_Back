using System.Web;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sts.V20180813;
using TencentCloud.Sts.V20180813.Models;

namespace Hao.TencentCloud.Cos
{
    public class FederationTokenProvider : IFederationTokenProvider
    {
        private readonly FederationTokenConfig _cfg;

        public FederationTokenProvider(FederationTokenConfig config)
        {
            _cfg = config;
        }
        
        /// <summary>
        /// 获取联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        public GetFederationTokenResponse GetFederationToken()
        {
            Credential cred = new Credential {
                SecretId = _cfg.SecretId,
                SecretKey = _cfg.SecretKey
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = _cfg.EndPoint;
            clientProfile.HttpProfile = httpProfile;

            StsClient client = new StsClient(cred, _cfg.Region, clientProfile);
            GetFederationTokenRequest req = new GetFederationTokenRequest();
            req.Name = _cfg.Name;
            req.Policy = HttpUtility.UrlEncode(_cfg.Policy);
            req.DurationSeconds = _cfg.DurationSeconds;
            GetFederationTokenResponse resp = client.GetFederationTokenSync(req);

            return resp;
        }
    }
}