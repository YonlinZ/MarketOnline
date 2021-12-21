using MarketOnline.Core.Entity;
using MarketOnline.Core.Resource;
using MarketOnline.Core.Util;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //using (var ws = new WebSocket("wss://stream.binance.com:9443/ws"))
            //{
            //    var count = 0;
            //    ws.OnMessage += (sender, e) =>
            //    {
            //        Console.WriteLine(e.Data);
            //        count++;
            //        if (count == 2) ws.Close();

            //    };
            //    ws.OnOpen += (sender, e) => Console.WriteLine("连接成功！");
            //    ws.SetProxy("http://127.0.0.1:10809", "", "");
            //    ws.Connect();
            //    //ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""btcusdt@bookTicker""], ""id"": 1 }");
            //    ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""!miniTicker@arr""], ""id"": 1 }");
            //    //ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""btcusdt@kline_4h""], ""id"": 1 }");
            //    //ws.Close();
            //    Console.ReadKey(true);
            //}


            var ratelimit = new Ratelimit()
            {
                rateLimitType = "REQUEST_WEIGHT",
                interval = "MINUTE",
                intervalNum = 1,
                limit = 1200
            };
            LoadedResource.ExchangeInfo = new ExchangeInfo();
            LoadedResource.ExchangeInfo.rateLimits = new Ratelimit[] { ratelimit };

            //while (true)
            //{
            //    var random = new Random();
            //    var r = random.Next(50);
            //    RequestLimitUtil.RequestBlock(r);
            //    Thread.Sleep(1000);
            //}


            var tslist = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                var ts = new Task(() =>
                {
                    var random = new Random();
                    var r = random.Next(50);
                    RequestLimitUtil.RequestBlock(r);

                });
                tslist.Add(ts);
                ts.Start();
            }

            Console.ReadLine();
        }
    }




}