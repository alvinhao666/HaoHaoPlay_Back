using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hao.Core.Extensions
{
    public class LongJsonConvert : JsonConverter<long>  // 包括了 long? 
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (long.TryParse(reader.GetString(), out long num)) return num;
            }
            return reader.GetInt64();
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) // long? 等于null时 不会走这个方法
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
