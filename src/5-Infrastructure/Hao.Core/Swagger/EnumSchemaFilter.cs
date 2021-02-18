using System;
using System.Linq;
using System.Reflection;
using System.Text;
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

            StringBuilder sb = new StringBuilder();

            if (attribute != null) sb.Append(((H_EnumDescriptionAttribute)attribute).Description);

            var i = 0;      

            foreach (var name in enumNames)
            {
                var value = ((OpenApiPrimitive<int>) enumValues[i]).Value;
                var description = H_EnumDescription.GetDescription(context.Type, value);

                sb.Append($"<br/>{value}: {name}  {description}");

                //schema.Enum.Add(new OpenApiString($"{value}: {name} {description}"));

                i++;
            }
            schema.Description = sb.ToString();
            return;
        }
    }
}