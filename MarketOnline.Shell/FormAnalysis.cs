using MarketOnline.Core.Resource;
using MarketOnline.DB;
using MarketOnline.Shell.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketOnline.Shell
{
    public partial class FormAnalysis : Form
    {
        public FormAnalysis()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {

        }

        private async void FormAnalysis_Shown(object sender, EventArgs e)
        {
            await GetAnaData();
            StartWS();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await GetAnaData();
        }

        private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            // 上币第一天的
            if (dgv.Rows[e.RowIndex].Cells["OpenTime"].Value.ToString() == dgv.Rows[e.RowIndex].Cells["Low_Time"].Value.ToString())
            {
                dgv.Rows[e.RowIndex].Cells["Low/Close"].Style.BackColor = Color.Gray;
                return;
            }
            // 跌幅大于40%
            if (double.Parse(dgv.Rows[e.RowIndex].Cells["Low/Close"].Value.ToString()) < -39.999)
            {
                dgv.Rows[e.RowIndex].Cells["Low/Close"].Style.BackColor = Color.Tomato;
            }
        }
        /// <summary>
        /// 获取分析数据
        /// </summary>
        /// <returns></returns>
        private async Task GetAnaData()
        {
            try
            {
                Utils.Shell.Status.Text = "正在获取所有分析数据...";
                var dt = await GetAllKlineAna();
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        var symbol = row["交易对"].ToString();
                        var price = LoadedResource.PriceChanges.FirstOrDefault(t => t.symbol == symbol)?.lastPrice;
                        if (price != null)
                        {
                            row["Price"] = double.Parse(price);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Utils.SetStatus($" 交易对数据处理异常：{ex.Message}");
                    }
                }
                dgv.DataSource = dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                Utils.Shell.Status.Text = "已获取所有分析数据。";
            }
        }

        private async Task<DataTable> GetAllKlineAna()
        {
            var ds = new DataSet();
            var err = string.Empty;
            foreach (var symbol in LoadedResource.AllSymbols)
            {
                try
                {
                    var data = await DBHelper.GetKlineAna(symbol, "1d");
                    ds.Tables.Add(data);
                }
                catch (Exception ex)
                {
                    err += ex.Message + Environment.NewLine;
                }

            }
            DataTable newdt = ds.Tables[0].Clone();

            foreach (DataTable dt in ds.Tables)
            {
                newdt.ImportRow(dt.Rows[0]);
            }
            if (!string.IsNullOrWhiteSpace(err))
            {
                MessageBox.Show(err);
            }

            return newdt;
        }

        private void StartWS()
        {
            WebSocketClient.Client.OnOpen += (sender, e) => Utils.SetStatus("WebSocket 连接成功。");
            WebSocketClient.Client.OnError += (sender, e) =>
            {
                try
                {
                    Utils.SetStatus($"WebSocket 异常：{e.Exception.Message}");
                    WebSocketClient.Client.Close();
                    button2.Text = "连接WebSocket";
                }
                catch (Exception ex)
                {
                    var err = e.Message + Environment.NewLine;
                    err += ex.Message + Environment.NewLine;
                    Utils.SetStatus(err);
                }
            };
            WebSocketClient.Client.OnClose += (sender, e) =>
            {
                Utils.SetStatus("WebSocket 关闭成功。");
                button2.Text = "连接WebSocket";
            };

            WebSocketClient.Client.OnMessage += (sender, e) =>
            {
                try
                {
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<Tiker>>(e.Data);
                    var dt = dgv.DataSource as DataTable;
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            var symbol = row["交易对"].ToString();
                            var price = data.FirstOrDefault(t => t.s == symbol)?.c;
                            if (price != null)
                            {
                                row["Price"] = double.Parse(price);
                            }
                        }
                        catch (Exception ex)
                        {
                            Utils.SetStatus($"WebSocket 交易对数据处理异常：{ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Utils.SetStatus($"WebSocket 数据处理异常：{ex.Message}");
                }
            };

            WebSocketClient.Client.Connect();
            WebSocketClient.Client.Send(@"{ ""method"": ""SUBSCRIBE"", ""params"": [ ""!miniTicker@arr""], ""id"": 1 }");
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (WebSocketClient.Client.IsAlive)
            {
                WebSocketClient.Client.Close();
                button2.Text = "连接WebSocket";
            }
            else
            {
                WebSocketClient.ReStart();
                StartWS();
                button2.Text = "关闭WebSocket";
            }
        }

    }
}
