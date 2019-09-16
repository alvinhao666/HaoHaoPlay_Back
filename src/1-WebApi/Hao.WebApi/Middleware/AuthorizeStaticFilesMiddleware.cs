using Hao.Core;
using Hao.Library;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hao.WebApi
{
    public class AuthorizeStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizeStaticFilesMiddleware( RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {

            if (!context.Request.Query.ContainsKey("Authorization"))
            {
                throw new HException(ErrorCode.E100001, nameof(ErrorCode.E100001).GetCode());
            }
            
            // 验证用户信息 //TODO

            //if (result.IsSucess == false)
            //{
            //    await context.ForbidAsync();
            //}

            await _next(context);
        }

    }
}
