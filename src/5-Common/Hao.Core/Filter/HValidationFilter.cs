using Hao.Core.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hao.Core
{
    public class HValidationFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).FirstOrDefault();
                var response = new BaseResponse
                {
                    Success = false,
                    Data = null,
                    ErrorMsg = error
                };
                context.Result = new JsonResult(response);
            }
            base.OnActionExecuting(context);
        }
    }
}
