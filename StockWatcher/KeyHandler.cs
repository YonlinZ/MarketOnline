using Gma.UserActivityMonitor;
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
        public static void Start(Control control)
        {

            HookManager.KeyDown += (sender, e) =>
            {
                //Console.WriteLine(e.KeyCode);
                if ((e.KeyCode == Keys.LControlKey || e.KeyCode == Keys.RControlKey) && _keyCache == 0)
                {
                    _keyCache = e.KeyCode;
                    return;
                }
                if ((_keyCache == Keys.LControlKey && e.KeyCode == Keys.LControlKey) || (_keyCache == Keys.RControlKey && e.KeyCode == Keys.RControlKey))
                {
                    Util.Log("按下了老板键");
                    control.Invoke((Action)(() => control.Visible = !control.Visible));
                    _keyCache = 0;
                }
                else
                    _keyCache = 0;

            };
            //按下按键LControlKey
            //按下按键B
        }
    }
}
