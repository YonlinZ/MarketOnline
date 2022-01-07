using MarketOnline.Core.Resource;
using MarketOnline.DB;
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
    public partial class FormKline : Form
    {
        //初始化绑定默认关键词（此数据源可以从数据库取）
        List<string> listInit = new List<string>();
        //输入key之后，返回的关键词
        List<string> listNew = new List<string>();
        public FormKline()
        {
            InitializeComponent();
            listInit = LoadedResource.AllSymbols;
            BindComboBox();
            InitControls();
        }

        private void InitControls()
        {
            BindComboBox();
            foreach (var item in Core.Resource.ConstVar.KlineIntervals)
            {
                if (item == "1d")
                {

                    var btn = new Button();
                    btn.Text = item;
                    btn.Click += btnQuery_Click;
                    flowLayoutPanel2.Controls.Add(btn);
                }
            }
        }

        private void BindComboBox()
        {
            comboBox1.Items.AddRange(listInit.ToArray());
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
            //清空combobox
            this.comboBox1.Items.Clear();
            //清空listNew
            listNew.Clear();
            //遍历全部备查数据
            foreach (var item in listInit)
            {
                if (item.Contains(this.comboBox1.Text.ToUpper()))
                {
                    //符合，插入ListNew
                    listNew.Add(item);
                }
            }
            //combobox添加已经查到的关键词
            this.comboBox1.Items.AddRange(listNew.ToArray());
            //设置光标位置，否则光标位置始终保持在第一列，造成输入关键词的倒序排列
            this.comboBox1.SelectionStart = this.comboBox1.Text.Length;
            //保持鼠标指针原来状态，有时候鼠标指针会被下拉框覆盖，所以要进行一次设置。
            Cursor = Cursors.Default;
            //自动弹出下拉框
            this.comboBox1.DroppedDown = true;
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            BindComboBox();
        }

        private async void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                var symbol = comboBox1.Text.Trim();
                if (string.IsNullOrWhiteSpace(symbol))
                {
                    return;
                }
                if (LoadedResource.AllSymbols.Contains(symbol))
                {
                    var klines = await DBHelper.GetKlineDataTable(symbol, btn.Text);
                    //var klines = await DBHelper.GetKline(symbol, btn.Text);
                    //var k = new BindingList<DB.Model.Kline>(klines.ToList());

                    dgv.DataSource = klines;
                }
                else
                {
                    var result = MessageBox.Show(this, $"交易对：{symbol}/{btn.Text} 无数据！是否更新？", "提示", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        await DBHelper.UpdateKline(symbol, btn.Text);
                        var klines = await DBHelper.GetKlineDataTable(symbol, btn.Text);
                        dgv.DataSource = klines;
                    }
                }
                Utils.Shell.Status.Text = $"当前交易对：{symbol}_1d。";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            Task ts = null;
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                Enabled = false;
                ts = Task.Run(() =>
                {
                    var tslist = new List<Task>();
                    foreach (var symbol in LoadedResource.AllSymbols)
                    {
                        var temp = DBHelper.UpdateKline(symbol, "1d").ContinueWith(ac =>
                        {
                            Utils.SetStatus($"交易对：{symbol}_1d 更新完成");
                        });
                        tslist.Add(temp);
                    }
                    Task.WhenAll(tslist);
                    sw.Stop();
                });
            }
            finally
            {
                ts.ContinueWith(ac => Invoke((Action)(() =>
                {
                    Enabled = true;
                    Debug.WriteLine($"耗时：{sw.ElapsedMilliseconds / 1000.0} 秒。");
                })));
            }
        }

        private async void btnUpdateOne_Click(object sender, EventArgs e)
        {
            var symbol = comboBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(symbol))
            {
                MessageBox.Show("请选择选择交易对！");
                comboBox1.Focus();
                return;
            }
            Utils.Shell.Status.Text = $"正在更新交易对：{symbol}_1d";

            await DBHelper.UpdateKline(symbol, "1d");
            var klines = await DBHelper.GetKlineDataTable(symbol, "1d");
            dgv.DataSource = klines;
            Utils.Shell.Status.Text = $"交易对：{symbol}_1d 更新完成。";
        }

        private void dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //dgv.Sort(dgv.Columns[e.ColumnIndex], ListSortDirection.Ascending);
        }

    }
}
