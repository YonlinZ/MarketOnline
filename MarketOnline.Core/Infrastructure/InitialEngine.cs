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
                    //var result = await "https://api.binance.com/api/v3/ticker/price".GetJsonAsync<List<SymbolPrice>>();
                    var res = await "https://api.binance.com/api/v3/exchangeInfo".GetAsync();
                    switch (res.StatusCode)
                    {
                        case 200:
                            var result = await res.GetJsonAsync<ExchangeInfo>();
                            var symbols = result.symbols
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

                    //Console.WriteLine($"Request {++counter}");
                    //Console.WriteLine(string.Join(",", StaticResource.AllSymbols.ToArray()));
                }
            }, TaskCreationOptions.LongRunning);
        }

        private static async Task GetKline()
        {
            var kline = new SymbolKlineSet("BTCUSDT");
            var taskList = new List<Task>();
            foreach (var interval in ConstVar.KlineIntervals)
            {
                var task = Task.Run(async () =>
                {
                    var res = await $"https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval={interval}".GetAsync();
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
