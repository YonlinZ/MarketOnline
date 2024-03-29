﻿using MarketOnline.Core.Resource;
using System;
using System.Linq;
using System.Threading;

namespace MarketOnline.Core.Util
{
    public class RequestLimitUtil
    {
        /// <summary>
        /// 周期内累计请求权重
        /// </summary>
        static int _requestWeight = 0;
        /// <summary>
        /// 周期开始时间
        /// </summary>
        static DateTime _requestTime;
        static object _lock = new object();
        /// <summary>
        /// 阻塞请求
        /// </summary>
        /// <param name="weight">请求权重</param>
        public static void RequestBlock(int weight)
        {
            lock (_lock)
            {
                if (_requestWeight == 0)
                {
                    _requestTime = DateTime.Now;
                    _requestWeight = weight;
                    Console.WriteLine($"#####周期开始时间：{_requestTime}, 累计权重：{_requestWeight}, 请求权重：{weight}");
                    return;
                }
                var rateLimit = LoadedResource.ExchangeInfo.rateLimits.FirstOrDefault(x => x.rateLimitType == "REQUEST_WEIGHT");
                var limit = rateLimit.limit - 100;
                if (_requestTime.AddMinutes(rateLimit.intervalNum) < DateTime.Now)
                {
                    _requestTime = DateTime.Now;
                    _requestWeight = weight;
                    Console.WriteLine($"######周期开始时间：{_requestTime}, 累计权重：{_requestWeight}, 请求权重：{weight}");
                    return;
                }
                if (_requestWeight + weight < limit)
                {
                    _requestWeight += weight;
                    Console.WriteLine($"######周期开始时间：{_requestTime}, 累计权重：{_requestWeight}, 请求权重：{weight}");
                    return;
                }
                if (_requestWeight + weight > limit)
                {
                    Console.WriteLine("超出请求限制：");
                    Console.WriteLine($"周期开始时间：{_requestTime}, 累计权重：{_requestWeight}, 请求权重：{weight}");
                    while (_requestTime.AddMinutes(rateLimit.intervalNum) >= DateTime.Now)
                    {
                        Console.WriteLine($"Sleep Start: {DateTime.Now}");
                        Thread.Sleep((_requestTime.AddMinutes(rateLimit.intervalNum) - DateTime.Now) + new TimeSpan(0, 0, 5));
                        Console.WriteLine($"Sleep End: {DateTime.Now}");
                        continue;
                    }
                    _requestTime = DateTime.Now;
                    _requestWeight = weight;
                    Console.WriteLine($"########周期开始时间：{_requestTime}, 累计权重：{_requestWeight}, 请求权重：{weight}");
                    return;
                }
                return;

            }
        }

    }
}
