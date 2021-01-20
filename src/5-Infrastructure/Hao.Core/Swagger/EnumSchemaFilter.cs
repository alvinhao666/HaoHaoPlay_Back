using System;
using System.Linq;
using Hao.Utility;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hao.Core
{
    /// <summary>
    /// swagger枚举处理
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Apply实现方法
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (!context.Type.IsEnum) return;

            var enumValues = schema.Enum.ToArray();
            var enumNames = Enum.GetNames(context.Type).ToList();

            var i = 0;
            schema.Enum.Clear();

            foreach (var name in enumNames)
            {
                var value = ((OpenApiPrimitive<int>) enumValues[i]).Value;
                schema.Enum.Add(new OpenApiString($"{value} = {name} = {H_Description.GetDescription(context.Type, value)}"));
                i++;
            }
        }
    }
}