using System;

namespace Hao.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public static class H_Log
    {
        /// <summary>
        /// 默认模板
        /// </summary>
        private const string _defaultTemplate = "{@Log}";

        private const string _templateString = "{Log}";

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Verbose(string log) => Serilog.Log.Verbose(_templateString, log);

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Verbose<T>(T log) => Serilog.Log.Verbose(_defaultTemplate, log);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Debug(string log) => Serilog.Log.Debug(_templateString, log);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Debug<T>(T log) => Serilog.Log.Debug(_defaultTemplate, log);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Info(string log) => Serilog.Log.Information(_templateString, log);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Info<T>(T log) => Serilog.Log.Information(_defaultTemplate, log);


        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Warn(string log) => Serilog.Log.Warning(_templateString, log);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Warn<T>(T log) => Serilog.Log.Warning(_defaultTemplate, log);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Error(string log) => Serilog.Log.Error(_templateString, log);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="log">日志内容</param>
        public static void Error<T>(Exception ex, T log) => Serilog.Log.Error(ex, _defaultTemplate, log);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Fatal(string log) => Serilog.Log.Fatal(_templateString, log);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="log">日志内容</param>
        public static void Fatal<T>(T log) => Serilog.Log.Fatal(_defaultTemplate, log);
    }
}