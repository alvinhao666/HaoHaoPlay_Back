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

            if (attribute != null) schema.Description = ((H_EnumDescriptionAttribute)attribute).Description;

            StringBuilder sb = new StringBuilder();
            sb.Append(" ");

            var i = 0;      

            foreach (var name in enumNames)
            {
                var value = ((OpenApiPrimitive<int>) enumValues[i]).Value;
                var description = H_EnumDescription.GetDescription(context.Type, value);

                sb.Append($"{value}£º{name}  {description}");

                if (i < enumValues.Length - 1) sb.Append("£¬");

                i++;
            }
            
            schema.Format = sb.ToString();
            return;
        }
    }
}