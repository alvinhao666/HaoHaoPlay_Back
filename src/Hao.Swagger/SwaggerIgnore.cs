using System;

namespace Hao.Swagger
{
    /// <summary>
    /// 用于忽略swagger文档请求实体的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerIgnoreAttribute : Attribute
    {

    }
}