using MarketOnline.Core.Entity;
using MarketOnline.Core.Infrastructure;
using MarketOnline.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarketOnline.Test
{
    public class RequestLimitTest
    {
        [Fact]
        public void Test()
        {
            var ratelimit = new Ratelimit()
            {
                rateLimitType = "REQUEST_WEIGHT",
                interval = "MINUTE",
                intervalNum = 1,
                limit = 1200
            };

            StaticResource.ExchangeInfo.rateLimits = new Ratelimit[] { ratelimit };

            while (true)
            {
                var random = new Random();
                var r = random.Next(50);
                RequestLimitUtil.RequestBlock(r);
                Thread.Sleep(1000);
            }


        }
    }
}
