using MarketOnline.Core.Resource;
using MarketOnline.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core
{
    public class Engine
    {
        /// <summary>
        /// 获取指定交易对k线数据，默认获取1000条
        /// </summary>
        /// <returns></returns>
        public static async Task GetKline(string symbol, string interval, int number = 1000)
        {
            var res = await $"{ConstVar.BaseUrl}/klines?limit={number}&symbol={symbol}&interval={interval}".GetAsync(1);
            if (res.StatusCode == 200)
            {
                var result = await res.GetJsonAsync<List<object[]>>();
                if (LoadedResource.Klines.ContainsKey(symbol))
                {
                    LoadedResource.Klines[symbol].IntervalKline[interval] = result;
                }
                else
                {
                    var kline = new SymbolKlineSet(symbol);
                    kline.IntervalKline[interval] = result;
                    LoadedResource.Klines[symbol] = kline;
                }
                LoadedResource.Klines[symbol].UpdateTime = DateTime.Now;

            }
            else if (res.StatusCode == 429)
            {
                //break;
            }
        }
    }
}
