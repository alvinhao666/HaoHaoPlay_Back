using System;
using Serilog;

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
        public static void Verbose(string note) => Log.Verbose(note);

        /// <summary>
        /// Verbose
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Verbose(LogNote note) => Log.Verbose(_defaultTemplate, note);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Debug(string note) => Log.Debug(note);

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Debug(LogNote note) => Log.Debug(_defaultTemplate, note);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Info(string note) => Log.Information(note);

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Info(LogNote note) => Log.Information(_defaultTemplate, note);


        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Warn(string note) => Log.Warning(note);

        /// <summary>
        /// Warn
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Warn(LogNote note) => Log.Warning(_defaultTemplate, note);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Error(string note) => Log.Error(note);

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="note">日志内容</param>
        public static void Error(Exception ex, LogNote note) => Log.Error(ex, _defaultTemplate, note);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Fatal(string note) => Log.Fatal(note);

        /// <summary>
        /// Fatal
        /// </summary>
        /// <param name="note">日志内容</param>
        public static void Fatal(LogNote note) => Log.Fatal(_defaultTemplate, note);
    }
}