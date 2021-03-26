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
    public class PreloadResource
    {
        /// <summary>
        /// 所有用usdt计价的交易对 260+个
        /// </summary>
        public static List<string> AllSymbols { get; set; } = new List<string>();
        public static ExchangeInfo ExchangeInfo { get; set; }
        /// <summary>
        /// key: symbol
        /// value: kline
        /// </summary>
        public static Dictionary<string, SymbolKlineSet> Klines { get; set; } = new Dictionary<string, SymbolKlineSet>();

        /// <summary>
        /// 24小时价格变动，按成交量排序
        /// </summary>
        public static List<PriceChange> PriceChanges { get; set; } = new List<PriceChange>();
    }
}
