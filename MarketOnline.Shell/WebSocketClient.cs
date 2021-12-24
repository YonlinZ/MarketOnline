using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MarketOnline.Shell
{
    public class WebSocketClient
    {
        public static WebSocket Client;

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
            }
        }
    }
}
