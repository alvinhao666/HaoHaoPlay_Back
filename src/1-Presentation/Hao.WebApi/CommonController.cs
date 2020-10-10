using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hao.TencentCloud.Cos;
using TencentCloud.Sts.V20180813.Models;

namespace Hao.WebApi
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController : H_Controller
    {
        private readonly IDictAppService _dictAppService;

        public CommonController(IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
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
        /// 获取腾讯云cos联合身份临时访问凭证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public GetFederationTokenResponse GetTencentCosFederationToken()
        {
            var provider = new TencentCosProvider();
            
            var result = provider.GetFederationToken();
            
            return result;
        }
    }
}
