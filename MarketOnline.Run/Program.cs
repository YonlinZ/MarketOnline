using MarketOnline.Core.Infrastructure;
using MarketOnline.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketOnline.Run
{
    class Program
    {
        //public static object Theading { get; private set; }

        static async Task Main(string[] args)
        {
            await InitialEngine.Start();
            var tss = new List<Task>();
            foreach (var symbol in PreloadResource.AllSymbols)
            {
                //var ts = InitialEngine.GetKline(symbol, "1d");
                //tss.Add(ts);
                InitialEngine.GetKline(symbol, "1d").Wait();
            }
            //Task.WaitAll(tss.ToArray());

            foreach (var kline in PreloadResource.Klines)
            {
                if (kline.Value.IntervalKline["1d"].Count < 1000)
                {
                    Console.WriteLine($"当前交易对：{kline.Key}");
                    var c = kline.Value.IntervalKline["1d"].Select(o => double.Parse(o[4].ToString()));
                    var c1 = c.First();
                    var cmin = c.Min();
                    var cmax = c.Max();
                    var cmin_1 = (cmin - c1) / c1;
                    var cmax_1 = (cmax - c1) / c1;
                    Console.WriteLine($@"上市收盘价：{c1:10}，最低价：{cmin:10}，最高价{cmax:10}，跌幅：{cmin_1:10}，涨幅：{cmax_1:10}");
                }
            }

            Console.ReadLine();
        }
    }
}
