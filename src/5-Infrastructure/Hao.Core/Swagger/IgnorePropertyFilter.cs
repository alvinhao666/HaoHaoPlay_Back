using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hao.Core
{
    public class IgnorePropertyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription == null || operation.Parameters == null)
                return;
        
            if (!context.ApiDescription.ParameterDescriptions.Any())
                return;

            //https://stackoverflow.com/questions/60837108/ignore-property-from-swagger-ui
            //[JsonIgnore] will just ignore property if the way of model binding is [FromBody] and it does not work for [FromQuery] and[FromForm] use this filter to ignore properties in all binding way

            //var list = context.ApiDescription.ParameterDescriptions.Where(p => p.Source.Equals(BindingSource.Form)
            //                                                    && p.CustomAttributes().Any(p => p.GetType().Equals(typeof(SwaggerIgnoreAttribute)))).ToList();

            //list.ForEach(p => operation.RequestBody.Content.Values.Single(v => v.Schema.Properties.Remove(p.Name)));


            var list = context.ApiDescription.ParameterDescriptions.Where(p => p.Source.Equals(BindingSource.Query)
                                                                && p.CustomAttributes().Any(a => a.GetType() == typeof(SwaggerIgnoreAttribute))).ToList();


            list.ForEach(p => operation.Parameters.Remove(operation.Parameters.Single(w => w.Name.Equals(p.Name))));
        }


        // public void Apply(OpenApiOperation operation, OperationFilterContext context)
        // {
        //     var ignoredProperties = context.MethodInfo.GetParameters()
        //         .SelectMany(p => p.ParameterType.GetProperties()
        //             .Where(prop => prop.GetCustomAttribute<SwaggerIgnoreAttribute>() != null))
        //         .ToList();
        //
        //     if (!ignoredProperties.Any()) return;
        //
        //     foreach (var property in ignoredProperties)
        //     {
        //         operation.Parameters = operation.Parameters
        //             .Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture)))
        //             .ToList();
        //     }
        // }
    }
    
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || schema.Properties.Count == 0) return;
            
            foreach (var property in context.Type.GetProperties())
            {
                var ignoreAttributes = property.GetCustomAttributes(typeof(SwaggerIgnoreAttribute),true);
    
                if (ignoreAttributes.Any() && schema.Properties.ContainsKey(property.Name))
                {
                    schema.Properties.Remove(property.Name);
                }
            }
        }
    }

}