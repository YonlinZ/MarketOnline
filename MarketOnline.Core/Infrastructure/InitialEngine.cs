using Flurl.Http;
using MarketOnline.Core.Entity;
using MarketOnline.Core.Resource;
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
        public static async void Start()
        {
            await GetAllSymbols();
            await GetKline();
            await GetPriceChange();
        }

        /// <summary>
        /// 获取所有的交易对
        /// </summary>
        /// <returns></returns>
        private async static Task GetAllSymbols()
        {
            //var counter = 0;
            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    var res = await $"{ConstVar.BaseUrl}/exchangeInfo".GetAsync();
                    switch (res.StatusCode)
                    {
                        case 200:
                            StaticResource.ExchangeInfo = await res.GetJsonAsync<ExchangeInfo>();
                            var symbols = StaticResource.ExchangeInfo.symbols
                                    .Where(s => s.symbol.EndsWith("USDT"))
                                    .Select(s => s.symbol);
                            var except = symbols.Except(StaticResource.AllSymbols);
                            if (except.Any())
                            {
                                StaticResource.AllSymbols.AddRange(except);
                            }
                            Thread.Sleep(60 * 1000);
                            break;
                        case 429:
                            break;
                        case 418:
                            break;
                    }
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
                var res = await $"{ConstVar.BaseUrl}/ticker/24hr".GetJsonAsync<List<PriceChange>>();
                StaticResource.PriceChanges = res;
                // 对交易对排序
                StaticResource.AllSymbols = StaticResource.AllSymbols.OrderByDescending(s =>
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
                    var res = await $"{ConstVar.BaseUrl}/klines?symbol=BTCUSDT&interval={interval}".GetAsync();
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
            StaticResource.Klines[kline.Symbol] = kline;
        }

    }
}
