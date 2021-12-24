using DapperEx;
using MarketOnline.Core;
using MarketOnline.Core.Resource;
using MarketOnline.DB.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.DB
{
    public class DBHelper
    {
        /// <summary>
        /// 更新kline
        /// </summary>
        /// <param name="symbol">交易对</param>
        public static async Task UpdateKline(string symbol, string interval)
        {
            var klineTableName = CommonHelper.GetTableName(symbol, interval);
            var klineRawTableName = CommonHelper.GetTableName(symbol, interval, true);
            var sql1 = $"SELECT count(1) FROM sqlite_master WHERE type='table' and tbl_name ='{klineTableName}'";
            var count = DapperUtil.GetScalar<int>(DataBaseType.SQLITE, ConstVar.Conn, sql1);
            if (count == 0) // Kline 表不存在
            {
                var sql2 = CommonHelper.GetTableSql(symbol, interval, "Kline", klineTableName);
                DapperUtil.Execute(DataBaseType.SQLITE, ConstVar.Conn, sql2);
            }

            sql1 = $"SELECT count(1) FROM sqlite_master WHERE type='table' and tbl_name ='{klineRawTableName}'";
            count = DapperUtil.GetScalar<int>(DataBaseType.SQLITE, ConstVar.Conn, sql1);
            if (count == 0) // Kline_Raw表不存在
            {
                var sql2 = CommonHelper.GetTableSql(symbol, interval, "Kline_Raw", klineRawTableName);
                DapperUtil.Execute(DataBaseType.SQLITE, ConstVar.Conn, sql2);
            }

            await Engine.GetKline(symbol, interval);

            var kline = Core.Resource.LoadedResource.Klines[symbol].IntervalKline[interval];

            using (var connection = ConnectionFactory.GetConnection(DataBaseType.SQLITE, ConstVar.Conn))
            {
                var tran = new DapperExTrans(connection);
                try
                {
                    tran.BeginTransaction();
                    tran.Execute($"delete FROM  {klineTableName}");
                    tran.Execute($"delete FROM  {klineRawTableName}");
                    foreach (var k in kline)
                    {
                        var sql = $@"insert into {klineTableName}(
                                    OpenTime                ,
                                    Open                    ,
                                    High                    ,
                                    Low                     ,
                                    Close                   ,
                                    Volume                  ,
                                    CloseTime               ,
                                    QuoteAssetVolume        ,
                                    NumberOfTrades          ,
                                    TakerBuyBaseAssetVolume ,
                                    TakerBuyQuoteAssetVolume,
                                    Ignore
                                    ) values(datetime('{k[0].ToString().StampToDatetime(true):yyyy-MM-dd HH:mm:ss.fff}'),
                                    '{k[1]}','{k[2]}','{k[3]}','{k[4]}','{k[5]}',
                                    '{k[6].ToString().StampToDatetime(true).ToString("yyyy-MM-dd HH:mm:ss.fff")}',
                                    '{k[7]}','{k[8]}','{k[9]}','{k[10]}','{k[11]}');";
                        tran.Execute(sql);

                        sql = $@"insert into {klineRawTableName}(
                                    OpenTimestamp                ,
                                    Open                    ,
                                    High                    ,
                                    Low                     ,
                                    Close                   ,
                                    Volume                  ,
                                    CloseTimestamp               ,
                                    QuoteAssetVolume        ,
                                    NumberOfTrades          ,
                                    TakerBuyBaseAssetVolume ,
                                    TakerBuyQuoteAssetVolume,
                                    Ignore
                                    ) values('{k[0]}','{k[1]}','{k[2]}','{k[3]}','{k[4]}','{k[5]}','{k[6]}','{k[7]}','{k[8]}','{k[9]}','{k[10]}','{k[11]}');";
                        tran.Execute(sql);
                    }


                    tran.CommitTransaction();
                }
                catch (Exception e)
                {
                    tran.RollbackTransaction();
                    throw new Exception($"更新{symbol}_{interval} k线失败", e);
                }


            }

        }

        public static Task<DataTable> GetKlineDataTable(string symbol, string interval)
        {
            return Task.Run(() =>
            {
                //return DapperUtil.GetInstances<Kline>(DataBaseType.SQLITE, ConstVar.Conn, $"select * from {CommonHelper.GetTableName(symbol, interval)}");
                return DapperUtil.GetTable(DataBaseType.SQLITE, ConstVar.Conn, $"select * from {CommonHelper.GetTableName(symbol, interval)}");
            });
        }

        public static Task<DataTable> GetKlineRawDataTable(string symbol, string interval)
        {
            return Task.Run(() =>
            {
                return DapperUtil.GetTable(DataBaseType.SQLITE, ConstVar.Conn, $"select * from {CommonHelper.GetTableName(symbol, interval, true)}");
            });
        }
        public static Task<IEnumerable<Kline>> GetKline(string symbol, string interval)
        {
            return Task.Run(() =>
            {
                return DapperUtil.GetInstances<Kline>(DataBaseType.SQLITE, ConstVar.Conn, $"select * from {CommonHelper.GetTableName(symbol, interval)}");
            });
        }

        public static Task<IEnumerable<Kline_Raw>> GetKlineRaw(string symbol, string interval)
        {
            return Task.Run(() =>
            {
                return DapperUtil.GetInstances<Kline_Raw>(DataBaseType.SQLITE, ConstVar.Conn, $"select * from {CommonHelper.GetTableName(symbol, interval, true)}");
            });
        }


        public static Task<DataTable> GetKlineAna(string symbol, string interval)
        {
            return Task.Run(() =>
            {
                var tableName = CommonHelper.GetTableName(symbol, interval);

                var sql = $@"select d.DateDiff 上币天数, '{symbol}' 交易对, a.Close, a.OpenTime, b.Low, b.Low_Time,printf(' % .2f', (b.Low-a.Close)/a.Close * 100) [Low/Close], c.High, c.High_Time, printf(' % .2f', (c.High-a.Close)/a.Close * 100) [High/Close], -1.0 Price from (

                select Close, Opentime from {tableName} where opentime =(select  min(opentime) from {tableName})) a cross join
 
                (select Low, opentime   Low_Time  from {tableName} where low =(select  min(low) from {tableName} where id<>(SELECT CASE count(1) WHEN 1 THEN -1 ELSE min(id) END from {tableName})) limit 1) b cross join
 
                (select High, opentime High_Time from {tableName} where high =(select  max(high) from {tableName} where id<>(SELECT CASE count(1) WHEN 1 THEN -1 ELSE min(id) END from {tableName})) limit 1) c cross join

                (select julianday('now','start of day') - (select julianday(min(OpenTime),'start of day') from {tableName}) + 1 DateDiff) d;";


                return DapperUtil.GetTable(DataBaseType.SQLITE, ConstVar.Conn, sql);
            });
        }
        
    }
}
