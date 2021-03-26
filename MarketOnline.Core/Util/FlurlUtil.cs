using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Core.Util
{
    public static class FlurlUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="weight">权重</param>
        /// <returns></returns>
        public static Task<IFlurlResponse> GetAsync(this string url, int weight)
        {
            RequestLimitUtil.RequestBlock(weight);
            return url.GetAsync();
        }

        public static Task<T> GetJsonAsync<T>(this string url, int weight)
        {
            RequestLimitUtil.RequestBlock(weight);
            return url.GetJsonAsync<T>();
        }
    }
}
