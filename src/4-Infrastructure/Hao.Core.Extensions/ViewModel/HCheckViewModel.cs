using Hao.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HCheckViewModel
    {
        public static IServiceCollection AddCheckViewModelService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).FirstOrDefault();
                    var response = new HResponse
                    {
                        Success = false,
                        ErrorMsg = error
                    };
                    return new JsonResult(response);
                };
            });
            return services;
        }
    }
}
