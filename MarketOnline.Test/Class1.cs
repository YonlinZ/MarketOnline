using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using WebSocketSharp;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ws = new WebSocket("wss://stream.binance.com:9443/ws"))
            {
                var count = 0;
                ws.OnMessage += (sender, e) =>
                {
                    Console.WriteLine(e.Data);
                    count++;
                    if (count == 2) ws.Close();

                };
                ws.OnOpen += (sender, e) => Console.WriteLine("连接成功！");
                ws.SetProxy("http://127.0.0.1:10809", "", "");
                ws.Connect();
                //ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""btcusdt@bookTicker""], ""id"": 1 }");
                ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""!miniTicker@arr""], ""id"": 1 }");
                //ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""btcusdt@kline_4h""], ""id"": 1 }");
                //ws.Close();
                Console.ReadKey(true);
            }
        }
    }
}