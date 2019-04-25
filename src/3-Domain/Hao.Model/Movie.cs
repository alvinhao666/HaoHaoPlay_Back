using Hao.Core.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hao.Model
{
    /// <summary>
    /// 电影
    /// </summary>
    [SugarTable("Movie")]
    public class Movie : Entity<long>
    {
        /// <summary>
        /// 电影名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电影类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 电影地区
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// 年代
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 上映日期
        /// </summary>
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// 语言类型
        /// </summary>
        public int LanguageType { get; set; }

        /// <summary>
        /// 导演
        /// </summary>
        public string Director { get; set; }

        /// <summary>
        /// 主演
        /// </summary>
        public string MainActors { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// 简介描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 正文描述
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverPicture { get; set; }

        /// <summary>
        /// 内容图片1
        /// </summary>
        public string PictureFirst { get; set; }

        /// <summary>
        /// 内容图片2
        /// </summary>
        public string PictureSecond { get; set; }

        /// <summary>
        /// 内容图片3
        /// </summary>
        public string PictureThird { get; set; }

        /// <summary>
        /// 下载地址1
        /// </summary>
        public string DownloadUrlFirst { get; set; }

        /// <summary>
        /// 下载地址2
        /// </summary>
        public string DownloadUrlSecond { get; set; }

        /// <summary>
        /// 下载地址3
        /// </summary>
        public string DownloadUrlThird { get; set; }

        /// <summary>
        /// 百度网盘地址
        /// </summary>
        public string BaiduPanUrl { get; set; }

        /// <summary>
        /// 在线观看地址
        /// </summary>
        public string ViewingUrl { get; set; }
    }
}
