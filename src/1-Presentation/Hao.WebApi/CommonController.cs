using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.Core;
using Hao.TencentCloud.Cos;
using Hao.Utility;
using TencentCloud.Sts.V20180813.Models;

namespace Hao.WebApi
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : H_Controller
    {
        private readonly IDictAppService _dictAppService;

        private readonly IFederationTokenProvider _tencentCosProvider;

        private readonly ICurrentUser _currentUser;
        public CommonController(IDictAppService dictAppService,ICurrentUser currentUser,IFederationTokenProvider tencentCosProvider)
        {
            _dictAppService = dictAppService;
            _tencentCosProvider = tencentCosProvider;
            _currentUser = currentUser;
        }

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
        /// <returns></returns>
        [HttpGet("{dictCode}")]
        public async Task<List<DictDataItemVM>> GetDictItemList(string dictCode) => await _dictAppService.GetDictDataItem(dictCode);


        /// <summary>
        /// 获取头像文件名称
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAvatarName() => $"{_currentUser.Id}_{H_Util.GetUnixTimestamp()}";
        
        /// <summary>
        /// 获取腾讯云cos联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> GetTencentCosFederationToken()
        {
            string key = "HaoHaoPlayBack_FederationToken";

            GetFederationTokenResponse result;

            var tokenCache = await RedisHelper.GetAsync(key);

            if (tokenCache.IsNullOrWhiteSpace())
            {
                result = _tencentCosProvider.GetFederationToken();
                await RedisHelper.SetAsync(key, H_JsonSerializer.Serialize(result), 7200);
            }
            else
            {
                result = H_JsonSerializer.Deserialize<GetFederationTokenResponse>(tokenCache);
            }

            return new {result.Credentials, result.ExpiredTime, StartTime = H_Util.GetUnixTimestamp(DateTime.Now), result.RequestId};
        }
    }
}
