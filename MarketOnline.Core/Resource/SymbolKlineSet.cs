using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core.Resource
{
    public class SymbolKlineSet
    {
        public string Symbol { get; private set; }

        public ConcurrentDictionary<string, List<List<object>>> IntervalKline { get; set; } = new ConcurrentDictionary<string, List<List<object>>>();

        public SymbolKlineSet(string symbol)
        {
            Symbol = symbol;
        }


    }
}
