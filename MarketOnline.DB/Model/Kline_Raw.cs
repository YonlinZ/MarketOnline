using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.DB.Model
{
    public class Kline_Raw
    {
        public int Id { get; set; }
        public long OpenTimestamp { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
        public long CloseTimestamp { get; set; }
        public string QuoteAssetVolume { get; set; }
        public long NumberOfTrades { get; set; }
        public string TakerBuyBaseAssetVolume { get; set; }
        public string TakerBuyQuoteAssetVolume { get; set; }
        public string Ignore { get; set; }
    }
}
