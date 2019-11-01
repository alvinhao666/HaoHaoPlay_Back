using Hao.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hao.Core.Filter
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class HResultFilter : ResultFilterAttribute,IResultFilter
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
//            if (context.ActionDescriptor is ControllerActionDescriptor)
//            {
//                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
//
//                if (descriptor != null && descriptor.MethodInfo.CustomAttributes.All(x => x.AttributeType != typeof(NoHResultAttribute)))
//                {
//                    if(!(context.Result is JsonResult))
//                    {
//                        var response = new BaseResponse
//                        {
//                            Success = true,
//                            Data = context.Result is EmptyResult ? null : (context.Result as ObjectResult)?.Value
//                        };
//                        context.Result = new JsonResult(response);
//                    }
//                }
//            }
            if(!(context.Result is JsonResult))
            {
                var response = new BaseResponse
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
