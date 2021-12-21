using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.DB.Test
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            await DBHelper.UpdateKline("BTCUSDT", "1d");





            Console.WriteLine("结束！");
            Console.ReadLine();
        }
    }
}
