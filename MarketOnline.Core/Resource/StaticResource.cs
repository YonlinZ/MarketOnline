using MarketOnline.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core.Resource
{
    /// <summary>
    /// 预加载的资源
    /// </summary>
    public class StaticResource
    {
        /// <summary>
        /// 所有用usdt计价的交易对 264个
        /// </summary>
        public static List<string> AllSymbols { get; private set; } = new List<string>();
        /// <summary>
        /// key: symbol
        /// value: kline
        /// </summary>
        public static Dictionary<string, SymbolKlineSet> Klines { get; set; } = new Dictionary<string, SymbolKlineSet>();


    }
}
