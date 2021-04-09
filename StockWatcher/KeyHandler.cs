using System;
using System.Windows.Forms;

namespace StockWatcher
{
    public class KeyHandler
    {
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
