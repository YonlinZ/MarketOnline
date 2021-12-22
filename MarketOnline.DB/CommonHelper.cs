using DapperEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.DB
{
    class CommonHelper
    {
        /// <summary>
        /// 获取生成Kline/Kline_Raw表的sql
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="tableType">Kline/Kline_Raw</param>
        /// <returns></returns>
        internal static string GetTableSql(string symbol, string interval, string tableType, string newName)
        {
            var sql = $"SELECT sql FROM sqlite_master WHERE type='table' and tbl_name = '{tableType}'";

            var sqlT = DapperUtil.GetScalar<string>(DataBaseType.SQLITE, ConstVar.Conn, sql);

            return sqlT.Replace(tableType, newName);

        }

        internal static string GetTableName(string symbol, string interval, bool isRaw = false)
        {
            if (isRaw)
            {
                return $"C_{symbol}_{interval}_Kline_Raw";
            }
            return $"C_{symbol}_{interval}_Kline";
        }

    }
}
