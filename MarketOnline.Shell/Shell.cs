using CommonHelper;
using MarketOnline.Shell.Properties;
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
        private Form _currentForm;
        private NotifyIcon _notifyIcon;
        private bool _isClose = false;
        public Shell()
        {
            InitializeComponent();
            InitControls();
        }
        private void InitControls()
        {
            klineMenu.Tag = MemoryCacheUtil.GetCacheItem<Form>(nameof(FormKline));
            AnaMenu.Tag = MemoryCacheUtil.GetCacheItem<Form>(nameof(FormAnalysis));


            ContextMenuStrip contextMenuStrip = new ContextMenuStrip(new Container());
            contextMenuStrip.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem("Exit", null, Exit)
            });

            _notifyIcon = new NotifyIcon()
            {
                Icon = Resources.Bitcoin,
                ContextMenuStrip = contextMenuStrip,
                Visible = true
            };
            _notifyIcon.DoubleClick += new EventHandler(HandleDoubleClick);

        }
        private void MenuClick(object sender, EventArgs e)
        {
            _currentForm?.Hide();
            var menu = sender as ToolStripMenuItem;
            var form = menu.Tag as Form;
            //form.MaximumSize = form.MinimumSize = panel1.Size;
            form.Dock = DockStyle.Fill;
            //form.Size = panel1.Size;
            //form.SuspendLayout();
            //form.Width = panel1.Width;
            //form.Height = panel1.Height;
            panel1.Controls.Add(form);
            form.BringToFront();
            form.Show();
            _currentForm = form;
            //form.ResumeLayout();
        }

        private void Shell_SizeChanged(object sender, EventArgs e)
        {
            //_currentForm.Size = panel1.Size;
        }
        private void Exit(object sender, EventArgs e)
        {
            _isClose = true;
            WebSocketClient.Stop();
            _notifyIcon.Visible = false;
            Application.Exit();
        }

        private void HandleDoubleClick(object Sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }
        private void Shell_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = !_isClose;
        }
    }
}
