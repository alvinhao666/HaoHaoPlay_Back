using Hao.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hao.Core.Extensions
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class H_ResultFilter : ResultFilterAttribute, IResultFilter
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!(context.Result is JsonResult))
            {
                var response = new H_Response
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
