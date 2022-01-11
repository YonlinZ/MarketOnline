using MarketOnline.Core.Resource;
using MarketOnline.DB;
using MarketOnline.Shell.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketOnline.Shell
{
    public partial class FormAnalysis : Form
    {
        const float BORDER_WIDTH = 2f;                                          // specify the width of selection border  
        const string DEFAULTTEXT = "过滤";
        private string _lastText = string.Empty;
        public FormAnalysis()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            txtFilter.Text = DEFAULTTEXT;
            txtFilter.ForeColor = SystemColors.ControlLight;
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
            if ((double)dgv.Rows[e.RowIndex].Cells["Low/Close"].Value < -0.39999)
            {
                dgv.Rows[e.RowIndex].Cells["Low/Close"].Style.BackColor = Color.Tomato;
            }
            // 现价低于最低价 40%
            if (((double)dgv.Rows[e.RowIndex].Cells["Price"].Value) / ((double)dgv.Rows[e.RowIndex].Cells["Close"].Value) < 0.6)
            {
                dgv.Rows[e.RowIndex].Cells["Price"].Style.BackColor = Color.Tomato;
                dgv.Rows[e.RowIndex].Cells["Price/Close"].Style.BackColor = Color.Tomato;
            }

            // 选中行样式
            Rectangle rowRect;                                                 // a selection rectangle  
            DataGridViewElementStates state;

            state = e.State & DataGridViewElementStates.Selected;              // only paint on selected row  
            if (state == DataGridViewElementStates.Selected)
            {
                int iBorder = Convert.ToInt32(BORDER_WIDTH);                   // calculate columns width  
                int columnsWidth = dgv.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);
                //int xStart = dgv.RowHeadersWidth;
                int xStart = 0;

                // need do calculate the clipping rectangle, because you can't use e.RowBounds  
                rowRect =
                   new Rectangle
                   (
                       xStart,                                                 // start after the row header  
                       e.RowBounds.Top + iBorder - 1,                          // at the top of the row  
                       columnsWidth - dgv.HorizontalScrollingOffset + 1,  // get the visible part of the row  
                       e.RowBounds.Height - iBorder                            // get the row's height  
                    );

                // draw the border  
                using (Pen pen = new Pen(Color.Black, BORDER_WIDTH))
                {
                    e.Graphics.DrawRectangle(pen, rowRect);                    // you can't use e.RowBounds here!  
                }
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
                            row["Price/Close"] = (double.Parse(price) - (double)row["Close"]) / (double)row["Close"];

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
                Utils.SetStatus("WebSocket 关闭成功。" + e.Reason);
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
                                row["Price/Close"] = (double.Parse(price) - (double)row["Close"]) / (double)row["Close"];
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

        private void dgv_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            DataGridViewElementStates state;

            state = e.State & DataGridViewElementStates.Selected;

            if (state == DataGridViewElementStates.Selected)
            {
                e.PaintParts &=                                                // prevent the grid from automatically      
               ~(                                                              // painting the selection background     
                   DataGridViewPaintParts.Focus |
                   DataGridViewPaintParts.SelectionBackground
                );
            }
        }

        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var name = dgv.Columns[e.ColumnIndex].Name;
            if (name == "Close" || name == "Low" || name == "High" || name == "Price")
            {
                e.CellStyle.Format = "#########0.##########";


            }
            else if (name == "Low/Close" || name == "High/Close" || name == "Price/Close")
            {
                e.CellStyle.Format = "P";

            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            var dt = dgv.DataSource as DataTable;
            if (dt == null) return;
            if (string.IsNullOrWhiteSpace(_lastText) && txtFilter.Text != DEFAULTTEXT)//突出显示
            {
                txtFilter.ForeColor = Color.Black;
                _lastText = DEFAULTTEXT;
                txtFilter.Text = txtFilter.Text.Replace(DEFAULTTEXT, string.Empty);
                txtFilter.SelectionStart = txtFilter.TextLength;

            }

            if (_lastText == DEFAULTTEXT && string.IsNullOrWhiteSpace(txtFilter.Text))//恢复默认
            {
                txtFilter.Text = DEFAULTTEXT;
                txtFilter.ForeColor = SystemColors.ControlLight;
                _lastText = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(txtFilter.Text.Trim()) || txtFilter.Text.Trim() == DEFAULTTEXT)
            {
                dt.DefaultView.RowFilter = "";
            }
            else
            {
                dt.DefaultView.RowFilter = $"交易对 LIKE '*{txtFilter.Text}*'";
            }
        }

        private void txtFilter_Click(object sender, EventArgs e)
        {
            if (txtFilter.Text == DEFAULTTEXT)
            {
                txtFilter.SelectAll();
            }
        }
    }
}
