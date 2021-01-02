using System.Text.Encodings.Web;
using System.Text.Json;

namespace Hao.Utility
{
    public class H_JsonSerializer
    {
        /// <summary>
        /// 将对象序列化成json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize<T>(T value) where T : class, new()
        {
            if (value == null) return null;

            return JsonSerializer.Serialize(value, new JsonSerializerOptions()
            {
                //在默认情况下，System.Text.Json 序列化程序对所有非 ASCII 字符进行转义；这就是中文被转义的根本原因
                //可以不必设置例外而达到不转义的效果，这个模式就是“非严格JSON”模式
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = null
            });
        }


        /// <summary>
        /// 将json字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json)) return default;

            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNamingPolicy = null
            });
        }
    }
}