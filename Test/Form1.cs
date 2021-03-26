using StockWatcher;
using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var control = new StockPreviewControl(null);
            control.BackColor = Color.FromArgb(DwmManager.ColorizationColor.R, DwmManager.ColorizationColor.G, DwmManager.ColorizationColor.B);
            this.Controls.Add(control);
            control.Location = new Point(20, 20);
            StockWatcher.KeyHandler.Start(control);
            // Call the DwmGetColorizationParameters function to fill in our structure.


            // Convert the colorization color to a .NET color and return it.

        }
        
    }
}
