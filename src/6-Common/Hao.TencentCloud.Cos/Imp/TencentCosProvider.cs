using COSXML;
using COSXML.Auth;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Transfer;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Sts.V20180813;
using TencentCloud.Sts.V20180813.Models;

namespace Hao.TencentCloud.Cos
{
    public class TencentCosProvider : ITencentCosProvider
    {
        private readonly TencentCosConfig _cosConfig;

        public TencentCosProvider(TencentCosConfig config)
        {
            _cosConfig = config;
        }
        
        /// <summary>
        /// 获取联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        public GetFederationTokenResponse GetFederationToken()
        {
            Credential cred = new Credential {
                SecretId = _cosConfig.SecretId,
                SecretKey = _cosConfig.SecretKey
            };

            ClientProfile clientProfile = new ClientProfile();
            HttpProfile httpProfile = new HttpProfile();
            httpProfile.Endpoint = _cosConfig.EndPoint;
            clientProfile.HttpProfile = httpProfile;

            StsClient client = new StsClient(cred, _cosConfig.Region, clientProfile);
            GetFederationTokenRequest req = new GetFederationTokenRequest();
            req.Name = _cosConfig.Name;
            req.Policy = HttpUtility.UrlEncode(_cosConfig.Policy);
            req.DurationSeconds = _cosConfig.DurationSeconds;
            
            GetFederationTokenResponse resp = client.GetFederationTokenSync(req);

            return resp;
        }

        /// <summary>
        /// 高级接口上传对象
        /// </summary>
        /// <returns></returns>
        public async Task TransferUploadFile(string filePath)
        {
            CosXmlConfig config = new CosXmlConfig.Builder()
                      .SetRegion(_cosConfig.Region) //设置一个默认的存储桶地域
                      .Build();

            string secretId = _cosConfig.SecretId;   //云 API 密钥 SecretId
            string secretKey = _cosConfig.SecretKey; //云 API 密钥 SecretKey
            long durationSecond = 600;          //每次请求签名有效时长，单位为秒
            QCloudCredentialProvider qCloudCredentialProvider = new DefaultQCloudCredentialProvider(secretId,
              secretKey, durationSecond);

            CosXml cosXml = new CosXmlServer(config, qCloudCredentialProvider);
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();

            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            string bucket = _cosConfig.Bucket; //存储桶，格式：BucketName-APPID
            string cosPath = _cosConfig.UploadKey; //对象在存储桶中的位置标识符，即称对象键

            // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, cosPath);
            uploadTask.SetSrcPath(filePath); //本地文件绝对路径

            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(string.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            uploadTask.successCallback = delegate (CosResult cosResult)
            {
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = cosResult
                  as COSXML.Transfer.COSXMLUploadTask.UploadTaskResult;
                Console.WriteLine(result.GetResultInfo());
                string eTag = result.eTag;
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx)
            {
                if (clientEx != null)
                {
                    Console.WriteLine("CosClientException: " + clientEx);
                }
                if (serverEx != null)
                {
                    Console.WriteLine("CosServerException: " + serverEx.GetInfo());
                }
            };
            await transferManager.UploadAsync(uploadTask);
        }
    }
}