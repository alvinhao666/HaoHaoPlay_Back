using System;
using System.Linq;
using System.Reflection;
using Hao.Utility;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hao.Core
{
    /// <summary>
    /// swagger
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum) return;

            var enumValues = schema.Enum.ToArray();
            var enumNames = Enum.GetNames(context.Type).ToList();
            
            var attribute = context.Type.GetCustomAttributes().FirstOrDefault(a => a.GetType() == typeof(H_EnumDescriptionAttribute));

            if (attribute != null) schema.Description = ((H_EnumDescriptionAttribute)attribute).Description;

            schema.Enum.Clear();
            var i = 0;
            
            foreach (var name in enumNames)
            {
                var value = ((OpenApiPrimitive<int>) enumValues[i]).Value;
                var description = H_EnumDescription.GetDescription(context.Type, value);

                if (!string.IsNullOrWhiteSpace(description)) description = $"= {description}";

                schema.Enum.Add(new OpenApiString($"{value} = {name} {description}"));
                i++;
            }
        }
    }
}