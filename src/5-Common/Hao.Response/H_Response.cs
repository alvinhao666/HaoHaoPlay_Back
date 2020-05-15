namespace Hao.Response
{
    /// <summary>
    /// 视图输出根类
    /// </summary>
    public class H_Response
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public int? ErrorCode { get; set; }

        /// <summary>
        /// 是否请求成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 数据对象
        /// </summary>
        public object Data { get; set; }
    }
}
