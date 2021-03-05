using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core.Resource
{
    /// <summary>
    /// 静态变量
    /// </summary>
    public class ConstVar
    {
        public const string BaseUrl = "https://api.binance.com/api/v3";

        /// <summary>
        /// k线周期 15钟
        /// </summary>
        public static string[] KlineIntervals { get; } = new string[] { "1m", "3m", "5m", "15m", "30m", "1h", "2h", "4h", "6h", "8h", "12h", "1d", "3d", "1w", "1M" };


 
    }
}
