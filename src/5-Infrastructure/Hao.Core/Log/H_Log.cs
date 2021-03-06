using System;
using Serilog;

namespace Hao.Core
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

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Verbose(string content) => Log.Verbose(content);

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Verbose(LogConent content) => Log.Verbose(_defaultTemplate, content);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Debug(string content) => Log.Debug(content);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Debug(LogConent content) => Log.Debug(_defaultTemplate, content);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Info(string content) => Log.Information(content);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="conent">日志内容</param>
        public static void Info(LogConent conent) => Log.Information(_defaultTemplate, conent);


        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Warn(string content) => Log.Warning(content);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="conent">日志内容</param>
        public static void Warn(LogConent conent) => Log.Warning(_defaultTemplate, conent);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Error(string content) => Log.Error(content);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="conent">日志内容</param>
        public static void Error(Exception ex, LogConent conent) => Log.Error(ex, _defaultTemplate, conent);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Fatal(string content) => Log.Fatal(content);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void Fatal(LogConent content) => Log.Fatal(_defaultTemplate, content);
    }
}