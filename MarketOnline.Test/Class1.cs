﻿using System;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using WebSocketSharp;

namespace Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var ws = new WebSocket("wss://stream.binance.com:9443/ws"))
            {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine("Laputa says: " + e.Data);
                ws.OnOpen += (sender, e) => Console.WriteLine("连接成功！");
                ws.SetProxy("http://127.0.0.1:10809", "", "");
                ws.Connect();
                ws.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""btcusdt@bookTicker""], ""id"": 1 }");
                Console.ReadKey(true);
            }
        }
    }
}