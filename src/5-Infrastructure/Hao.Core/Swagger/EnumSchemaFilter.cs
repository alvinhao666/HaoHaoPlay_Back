using System;
using System.Linq;
using System.Reflection;
using Hao.Utility;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hao.Core
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum) return;
            
            var enumValues = schema.Enum.ToArray();
            var i = 0;
            schema.Enum.Clear();
            foreach (var name in Enum.GetNames(context.Type).ToList())
            {
                var value = ((OpenApiPrimitive<int>) enumValues[i]).Value;
                
                schema.Enum.Add(new OpenApiString($"{value} = {name} = {H_Description.GetDescription(context.Type, value)}"));
                i++;
            }
        }
    }
}