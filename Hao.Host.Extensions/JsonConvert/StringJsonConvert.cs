using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hao.Core.Extensions
{
    public class StringJsonConvert : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString()?.Trim();
            }
            return reader.GetString();
        }


        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) // string 等于null时 不会走这个方法
        {
            writer.WriteStringValue(value.Trim()); //首尾去除所有空格
        }
    }
}
