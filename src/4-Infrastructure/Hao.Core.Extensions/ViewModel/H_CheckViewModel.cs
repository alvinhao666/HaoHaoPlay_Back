using Hao.Response;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class H_CheckViewModel
    {
        public static IServiceCollection AddCheckViewModelService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var error = context.ModelState.Values.SelectMany(x => x.Errors.Select(p => p.ErrorMessage)).FirstOrDefault();
                    var response = new H_Response
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
