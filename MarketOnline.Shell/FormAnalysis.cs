using MarketOnline.DB;
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
            //var dt = DBHelper.GetAllKlineAna().ConfigureAwait(false).GetAwaiter().GetResult();
            //Invoke((Action)(()=>{ dgv.DataSource = dt; }));
            //dgv.DataSource = dt;
        }

        private async void FormAnalysis_Shown(object sender, EventArgs e)
        {
            ConstVar.Shell.Status.Text = "正在获取所有分析数据...";
            var dt = await DBHelper.GetAllKlineAna();
            dgv.DataSource = dt;
            ConstVar.Shell.Status.Text = "已获取所有分析数据。";
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            ConstVar.Shell.Status.Text = "正在获取所有分析数据...";
            var dt = await DBHelper.GetAllKlineAna();
            dgv.DataSource = dt;
            ConstVar.Shell.Status.Text = "已获取所有分析数据。";
        }

        private void dgv_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            if (dgv.Rows[e.RowIndex].Cells["OpenTime"].Value.ToString() == dgv.Rows[e.RowIndex].Cells["Low_Time"].Value.ToString())
            {
                dgv.Rows[e.RowIndex].Cells["Low/Close"].Style.BackColor = Color.Gray;

            }
        }
    }
}
