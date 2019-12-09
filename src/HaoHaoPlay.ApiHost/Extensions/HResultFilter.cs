using Hao.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HaoHaoPlay.ApiHost
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class HResultFilter : ResultFilterAttribute,IResultFilter
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if(!(context.Result is JsonResult))
            {
                var response = new HResponse
                {
                    Success = true,
                    Data = context.Result is EmptyResult ? null : (context.Result as ObjectResult)?.Value
                };
                context.Result = new JsonResult(response);
            }
            base.OnResultExecuting(context);
        }
    }
}
