using Hao.AppService;
using Hao.AppService.ViewModel;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 通用接口
    /// </summary>
    public class CommonController:H_Controller
    {
        private readonly IDictAppService _dictAppService;

        public CommonController(IDictAppService dictAppService)
        {
            _dictAppService = dictAppService;
        }


        /// <summary>
        /// 根据字典编码查询数据项
        /// </summary>
        /// <param name="dictCode"></param>
        /// <returns></returns>
        [HttpGet("GetDictDataItem/{dictCode}")]
        public async Task<List<DictDataItemVM>> GetDictDataItem(string dictCode) => await _dictAppService.GetDictDataItem(dictCode);
    }
}
