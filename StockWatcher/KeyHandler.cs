using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockWatcher
{
    public class KeyHandler
    {
        private static Keys _keyCache = 0;
        private static DateTime _dt = DateTime.MinValue;
        public static void Start(Control control)
        {
            var hook = new KeyboardHook();
            hook.KeyDownEvent += (sender, e) =>
            {
                if (e.KeyValue == (int)Keys.A && (int)Control.ModifierKeys == (int)Keys.Control)
                {
                    control.Invoke((Action)(() => control.Visible = !control.Visible));
                }
            };
            hook.Start();
        }
    }
}
