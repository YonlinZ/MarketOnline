using Flurl.Http;
using MarketOnline.Core.Entity;
using MarketOnline.Core.Resource;
using MarketOnline.Core.Util;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketOnline.Core.Infrastructure
{
    public class InitialEngine
    {
        /// <summary>
        /// 启动
        /// </summary>
        public static async Task Start()
        {
            await GetAllSymbols();
            GetAllSymbolsLoops();
            //System.Console.WriteLine(PreloadResource.AllSymbols.Count);
        }

        /// <summary>
        /// 获取所有的交易对
        /// 请求权重 10
        /// </summary>
        /// <returns></returns>
        private async static Task GetAllSymbols()
        {
            //var counter = 0;

            var res = await $"{ConstVar.BaseUrl}/exchangeInfo".GetAsync(10);
            switch (res.StatusCode)
            {
                case 200:
                    PreloadResource.ExchangeInfo = await res.GetJsonAsync<ExchangeInfo>();
                    var symbols = PreloadResource.ExchangeInfo.symbols
                            .Where(s => s.symbol.EndsWith("USDT") && s.status == "TRADING")
                            .Select(s => s.symbol);
                    var except = symbols.Except(PreloadResource.AllSymbols);
                    if (except.Any())
                    {
                        PreloadResource.AllSymbols.AddRange(except);
                    }
                    break;
                case 429:
                    break;
                case 418:
                    break;
            }


        }


        private static void GetAllSymbolsLoops()
        {
            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(60 * 60 * 1000);// 一小时查询一次
                    await GetAllSymbols();
                }
            }, TaskCreationOptions.LongRunning);
        }
        /// <summary>
        /// 获取24小时价格变动
        /// 权重 40
        /// </summary>
        /// <returns></returns>
        private async static Task GetPriceChange()
        {
            await Task.Run(async () =>
            {
                var res = await $"{ConstVar.BaseUrl}/ticker/24hr".GetJsonAsync<List<PriceChange>>(40);
                PreloadResource.PriceChanges = res;
                // 对交易对排序
                PreloadResource.AllSymbols = PreloadResource.AllSymbols.OrderByDescending(s =>
                    {
                        var item = res.FirstOrDefault(pc => pc.symbol == s);
                        if (item != null)
                        {
                            return double.Parse(item.quoteVolume);
                        }
                        return 0;
                    }).ToList();
            });
        }
        /// <summary>
        /// 获取所有k线数据
        /// </summary>
        /// <returns></returns>
        private static async Task GetKline()
        {
            var kline = new SymbolKlineSet("BTCUSDT");
            var taskList = new List<Task>();
            foreach (var interval in ConstVar.KlineIntervals)
            {
                var task = Task.Run(async () =>
                {
                    var res = await $"{ConstVar.BaseUrl}/klines?symbol=BTCUSDT&interval={interval}".GetAsync(1);
                    if (res.StatusCode == 200)
                    {
                        var result = await res.GetJsonAsync<List<object[]>>();
                        kline.IntervalKline[interval] = result;
                    }
                    else if (res.StatusCode == 429)
                    {
                        //break;
                    }
                });
                taskList.Add(task);
            }
            Task.WaitAll(taskList.ToArray());
            PreloadResource.Klines[kline.Symbol] = kline;
        }

        

    }
}
