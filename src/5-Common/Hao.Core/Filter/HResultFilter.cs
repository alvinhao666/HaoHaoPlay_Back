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
    public class HResultFilter : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

                if (!descriptor.MethodInfo.CustomAttributes.Any(x => x.AttributeType == typeof(NoHResultAttribute)))
                {
                    var result = context.Result;
                    if (!context.ModelState.IsValid)
                    {
                        var error = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).FirstOrDefault();
                        var response = new BaseResponse
                        {
                            Success = false,
                            Data = null,
                            ErrorCode = -1,
                            ErrorMsg = error
                        };
                        context.Result = new JsonResult(response);
                    }
                    else 
                    {
                        if (result is EmptyResult)
                        {
                            context.Result = new ObjectResult(null);
                        }
                        var response = new BaseResponse
                        {
                            Success = true,
                            Data = (context.Result as ObjectResult).Value,
                        };
                        context.Result = new JsonResult(response);
                    }
                }
            }
            base.OnResultExecuting(context);
        }
    }
}
