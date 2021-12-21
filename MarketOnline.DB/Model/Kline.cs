using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.DB.Model
{
    public class Kline
    {
        public int Id { get; set; }
        public DateTime OpenTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public DateTime CloseTime { get; set; }
        public double QuoteAssetVolume { get; set; }
        public long NumberOfTrades { get; set; }
        public double TakerBuyBaseAssetVolume { get; set; }
        public double TakerBuyQuoteAssetVolume { get; set; }
        public string Ignore { get; set; }
    }
}
