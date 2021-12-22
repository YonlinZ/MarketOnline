using System;
using System.Linq;
using System.Text;

namespace MarketOnline.DB
{
    /// <summary>
    /// 通用扩展方法
    /// </summary>
    public static class CommonExtension
    {
        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="isMilliseconds">是否为毫秒</param>
        /// <returns></returns>
        public static DateTime StampToDatetime(this long timeStamp, bool isMilliseconds = false)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));//当地时区
            //返回转换后的日期
            if (isMilliseconds)
                return startTime.AddMilliseconds(timeStamp);
            else
                return startTime.AddSeconds(timeStamp);
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="isMilliseconds"></param>
        /// <returns></returns>
        public static DateTime StampToDatetime(this string timeStamp, bool isMilliseconds = false)
        {
            var time = long.Parse(timeStamp);
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));//当地时区
            //返回转换后的日期
            if (isMilliseconds)
                return startTime.AddMilliseconds(time);
            else
                return startTime.AddSeconds(time);
        }
    }
}
