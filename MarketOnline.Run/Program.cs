using MarketOnline.Core;
using MarketOnline.Core.Infrastructure;
using MarketOnline.Core.Resource;
using MarketOnline.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketOnline.Run
{
    class Program
    {
        //public static object Theading { get; private set; }

        static async Task Main(string[] args)
        {
            Console.WriteLine("开始！");
            await F2();
            var r = await DBHelper.GetKlineRaw("BTCUSDT", "1d");
            Console.ReadLine();
        }


        static async Task F1()
        {
            await InitialEngine.Start();
            var tss = new List<Task>();
            foreach (var symbol in LoadedResource.AllSymbols)
            {
                //var ts = InitialEngine.GetKline(symbol, "1d");
                //tss.Add(ts);
                Engine.GetKline(symbol, "1d").Wait();
            }
            //Task.WaitAll(tss.ToArray());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"交易对共计：{LoadedResource.AllSymbols.Count}");
            foreach (var kline in LoadedResource.Klines)
            {
                if (kline.Value.IntervalKline["1d"].Count < 1000)
                {
                    Console.WriteLine($"当前交易对：{kline.Key}");
                    var c = kline.Value.IntervalKline["1d"].Select(o => double.Parse(o[4].ToString()));
                    var c1 = double.Parse(kline.Value.IntervalKline["1d"][0][4].ToString());
                    var cmin = kline.Value.IntervalKline["1d"].Select(o => double.Parse(o[3].ToString())).Min();
                    var cmax = kline.Value.IntervalKline["1d"].Select(o => double.Parse(o[2].ToString())).Max();
                    var cmin_1 = (cmin - c1) / c1;
                    var cmax_1 = (cmax - c1) / c1;
                    Console.WriteLine($@"上市收盘价：{c1:F4}，最低价：{cmin:F4}，最高价{cmax:F4}，跌幅：{cmin_1:F4}，涨幅：{cmax_1:F4}");
                }
            }
        }

        static async Task F2()
        {
            await InitialEngine.Start();
            foreach (var symbol in LoadedResource.AllSymbols)
            {
                try
                {
                    DBHelper.UpdateKline(symbol, "1d").Wait();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
