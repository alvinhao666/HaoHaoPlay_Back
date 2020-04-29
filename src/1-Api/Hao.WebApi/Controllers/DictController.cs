using Hao.AppService;
using Hao.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Hao.WebApi.Controllers
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DictController:HController
    {
        private readonly DictAppService _dictAppService;
        public DictController(DictAppService dictAppService)
        {
            _dictAppService = dictAppService;
        }
        
        
    }
}