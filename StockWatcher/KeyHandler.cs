using System;
using System.Windows.Forms;

namespace StockWatcher
{
    public class KeyHandler
    {
        private static Keys _keyCache = 0;
        private static DateTime _dt = DateTime.MinValue;
        public static void Start(Control control)
        {
            try
            {
                var hook = new KeyboardHook();
                hook.KeyDownEvent += (sender, e) =>
                {
                    try
                    {
                        if (e.KeyValue == (int)Keys.A && (int)Control.ModifierKeys == (int)Keys.Control)
                        {
                            IntPtr i = control.Handle;
                            control.Invoke((Action)(() => control.Visible = !control.Visible));
                            Util.Log("按下老板键 Ctrl + A");
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.Log(ex.Message);
                    }
                };
                hook.Start();
            }
            catch (Exception ex)
            {
                Util.Log(ex.Message);
            }
        }
    }
}
