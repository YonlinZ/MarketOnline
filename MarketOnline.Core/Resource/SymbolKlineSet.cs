using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MarketOnline.Core.Resource
{
    public class SymbolKlineSet
    {
        public string Symbol { get; private set; }

        /// <summary>
        /// key: kline interval
        /// value: kline 
        /// </summary>
        public ConcurrentDictionary<string, List<object[]>> IntervalKline { get; set; } = new ConcurrentDictionary<string, List<object[]>>();

        public SymbolKlineSet(string symbol)
        {
            Symbol = symbol;
        }


    }
}
