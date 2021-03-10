namespace Hao.Log
{
    /// <summary>
    /// 日志信息
    /// </summary>
    public class LogNote
    {
        /// <summary>
        /// 记录当前位置，请求路径或者方法名称
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 日志跟踪信息id
        /// </summary>
        public string TraceId { get; set; }

        /// <summary>
        /// 客户端ip地址
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 需记录的数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 额外内容&描述
        /// </summary>
        public string Extra { get; set; }


        ///// <summary>
        ///// 重写方法，必须，用于日志记录
        ///// </summary>
        ///// <returns></returns>
        //public override string ToString()
        //{
        //    return JsonSerializer.Serialize(this, new JsonSerializerOptions()
        //    {
        //        //在默认情况下，System.Text.Json 序列化程序对所有非 ASCII 字符进行转义；这就是中文被转义的根本原因
        //        //可以不必设置例外而达到不转义的效果，这个模式就是“非严格JSON”模式
        //        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        //        PropertyNamingPolicy = null  //PropertyNamingPolicy = JsonNamingPolicy.CamelCase //开头字母小写 默认
        //    });
        //}
    }
}
