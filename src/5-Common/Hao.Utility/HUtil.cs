using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hao.Utility
{
    public static class HUtil
    {
        public static Type IntType = typeof(int);
        public static Type LongType = typeof(long);
        public static Type GuidType = typeof(Guid);
        public static Type BoolType = typeof(bool);
        public static Type BoolTypeNull = typeof(bool?);
        public static Type ByteType = typeof(Byte);
        public static Type ObjType = typeof(object);
        public static Type DobType = typeof(double);
        public static Type FloatType = typeof(float);
        public static Type ShortType = typeof(short);
        public static Type DecType = typeof(decimal);
        public static Type StringType = typeof(string);
        public static Type DateType = typeof(DateTime);
        public static Type DateTimeOffsetType = typeof(DateTimeOffset);
        public static Type TimeSpanType = typeof(TimeSpan);
        public static Type ByteArrayType = typeof(byte[]);
        public static Type DynamicType = typeof(ExpandoObject);

        /// <summary>  
        /// 获取当前时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime time)
        {
            TimeSpan ts = time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间戳转换为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(long timeStamp)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long mTime = long.Parse($"{timeStamp}0000000");
            TimeSpan toNow = new TimeSpan(mTime);
            return startTime.Add(toNow);
        }

        /// <summary>
        /// 获取指定长度的随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomCha(int length)
        {
            char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < length; i++)
            {
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());
            }

            return num.ToString();
        }

        /// <summary>
        /// 将网络图片转base64
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static string GetBase64String(string imageUrl)
        {
            var webreq = WebRequest.Create(imageUrl);
            var webres = webreq.GetResponse();
            string image = string.Empty;
            using (var stream = webres.GetResponseStream())
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    ms.Flush();
                    ms.Seek(0, SeekOrigin.Begin);
                    byte[] fileBytes = new byte[ms.Length];
                    ms.Read(fileBytes, 0, (int)ms.Length);
                    image = Convert.ToBase64String(fileBytes);
                }
            }
            return image;
        }
    }
}
