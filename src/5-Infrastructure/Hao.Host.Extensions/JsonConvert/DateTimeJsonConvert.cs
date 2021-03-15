using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hao.Core.Extensions
{
    public class DatetimeJsonConverter : JsonConverter<DateTime>  // 包括了 DateTime?  
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime date)) return date;
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)  // DateTime? 等于null时 不会走这个方法
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
