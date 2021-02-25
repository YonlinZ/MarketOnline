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
        public static object Theading { get; private set; }

        static void Main(string[] args)
        {
            InitialEngine.Start();
            Console.ReadLine();
        }
    }
}
