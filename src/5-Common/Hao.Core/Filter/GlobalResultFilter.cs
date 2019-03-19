using Hao.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hao.Core.Filter
{
    /// <summary>
    /// 全局过滤器，默认将返回值作为BaseResponse中的Data属性
    /// </summary>
    public class GlobalResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                if (!descriptor.MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(NoGlobalResultAttribute)))
                {
                    var result = context.Result;
                    if (result is EmptyResult || result is ObjectResult)
                    {
                        context.Result = result is EmptyResult ? new ObjectResult(null) : result;
                        var obj = context.Result as ObjectResult;
                        if (!(obj.Value is BaseResponse))
                        {
                            obj.Value = new BaseResponse
                            {
                                Success = true,
                                Data = obj.Value
                            };
                        }
                    }
                }
            }
            base.OnResultExecuting(context);
        }
    }
}
