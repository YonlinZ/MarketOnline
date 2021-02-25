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
                    var result = await "https://api.binance.com/api/v3/ticker/price".GetJsonAsync<List<SymbolPrice>>();
                    var symbols = result.Select(s => s.symbol);
                    var except = symbols.Except(StaticResource.AllSymbols);
                    if (except.Any())
                    {
                        StaticResource.AllSymbols.AddRange(except);
                    }
                    //Console.WriteLine($"Request {++counter}");
                    //Console.WriteLine(string.Join(",", StaticResource.AllSymbols.ToArray()));
                    Thread.Sleep(5000);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private static async Task GetKline()
        {
            var result = await "https://api.binance.com/api/v3/klines?symbol=BTCUSDT&interval=4h".GetJsonAsync<List<List<object>>>();
            StaticResource.Kline["BTCUSDT"] = result;



        }

    }
}
