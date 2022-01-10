using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MarketOnline.Shell
{
    public sealed class WebSocketClient
    {
        public static WebSocket Client;
        private WebSocketClient()
        {

        }

        static WebSocketClient()
        {
            Start();
        }

        public static void ReStart()
        {
            Start();
        }

        private static void Start()
        {
            var proxyUrl = ConfigurationManager.AppSettings["ProxyUrl"];
            Client = new WebSocket(ConfigurationManager.AppSettings["WebSocketBaseUrl"]);
            if (!string.IsNullOrWhiteSpace(proxyUrl))
            {
                Client.SetProxy(ConfigurationManager.AppSettings["ProxyUrl"], ConfigurationManager.AppSettings["ProxyUsername"], ConfigurationManager.AppSettings["ProxyPassword"]);
                //_ = Ping();
            }
        }
        public static void Stop()
        {
            Client.Close();
        }
        private static async Task Ping()
        {
            //await Task.Factory.StartNew(async () =>
            //{
            //    while (true)
            //    {
            //        await Task.Delay(1000 * 10);
            //        System.Diagnostics.Debug.WriteLine("Ping 一下！");
            //        Client.p();
            //    }
            //}, TaskCreationOptions.LongRunning);
        }
    }
}
