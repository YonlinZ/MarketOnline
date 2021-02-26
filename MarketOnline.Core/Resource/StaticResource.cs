using MarketOnline.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core.Resource
{
    /// <summary>
    /// 预加载的资源
    /// </summary>
    public class StaticResource
    {
        /// <summary>
        /// 所有的交易对
        /// </summary>
        public static List<string> AllSymbols { get; private set; } = new List<string>();
        public static Dictionary<string, List<List<object>>> Kline { get; set; } = new Dictionary<string, List<List<object>>>();


    }
}
