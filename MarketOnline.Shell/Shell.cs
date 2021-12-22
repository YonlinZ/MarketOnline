using CommonHelper;
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
    public partial class Shell : Form
    {
        public Shell()
        {
            InitializeComponent();
        }

        private void klineMenu_Click(object sender, EventArgs e)
        {
            var form = MemoryCacheUtil.GetCacheItem<Form>(nameof(FormKline));
            panel1.Controls.Add(form);
            form.Show();

        }
    }
}
