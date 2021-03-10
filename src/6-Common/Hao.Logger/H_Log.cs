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

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Verbose(string note) => Serilog.Log.Verbose(note);

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Verbose(LogNote note) => Serilog.Log.Verbose(_defaultTemplate, note);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Debug(string note) => Serilog.Log.Debug(note);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Debug(LogNote note) => Serilog.Log.Debug(_defaultTemplate, note);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Info(string note) => Serilog.Log.Information(note);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Info(LogNote note) => Serilog.Log.Information(_defaultTemplate, note);


        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Warn(string note) => Serilog.Log.Warning(note);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Warn(LogNote note) => Serilog.Log.Warning(_defaultTemplate, note);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Error(string note) => Serilog.Log.Error(note);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="note">日志内容</param>
        public static void Error(Exception ex, LogNote note) => Serilog.Log.Error(ex, _defaultTemplate, note);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Fatal(string note) => Serilog.Log.Fatal(note);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Fatal(LogNote note) => Serilog.Log.Fatal(_defaultTemplate, note);
    }
}