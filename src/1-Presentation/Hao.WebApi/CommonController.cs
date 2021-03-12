using Hao.AppService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;
using Hao.Runtime;
using Hao.TencentCloud.Cos;
using Hao.Utility;
using TencentCloud.Sts.V20180813.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Hao.Redis;

namespace Hao.WebApi
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : H_Controller
    {
        /// <summary>
        /// 获取服务器当前时间
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public DateTime GetCurrentTime() => DateTime.Now;


        /// <summary>
        /// 根据字典编码查询数据项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <param name="dictAppService"></param>
        /// <returns></returns>
        [HttpGet("{dictCode}")]
        public async Task<List<DictDataItemOutput>> GetDictItemList(string dictCode,
            [FromServices] IDictAppService dictAppService) => await dictAppService.GetDictDataItem(dictCode);


        /// <summary>
        /// 获取上传头像所需信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetUploadAvatarInfo([FromServices] IConfiguration config, [FromServices] ICurrentUser currentUser)
        {
            return new
            {
                Bucket = config["TencentCos:Bucket"],
                Key = config["TencentCos:AvatarKey"] + $"/{currentUser.Id}_{H_Util.GetUnixTimestamp()}",
                Region = config["TencentCos:Region"]
            };
        }


        /// <summary>
        /// 获取腾讯云cos联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object GetTencentCosFederationToken([FromServices] ITencentCosProvider tencentCosProvider)
        {
            string key = "HaoHaoPlay_Back_FederationToken";

            GetFederationTokenResponse result;

            var tokenCache = RedisHelper.Get(key);

            if (tokenCache.IsNullOrWhiteSpace())
            {
                result = tencentCosProvider.GetFederationToken();
                RedisHelper.Set(key, JsonConvert.SerializeObject(result), 7200);
            }
            else
            {
                result = JsonConvert.DeserializeObject<GetFederationTokenResponse>(tokenCache);
            }

            return new
            {
                result.Credentials,
                result.ExpiredTime,
                StartTime = H_Util.GetUnixTimestamp(DateTime.Now),
                result.RequestId,
            };
        }
    }
}