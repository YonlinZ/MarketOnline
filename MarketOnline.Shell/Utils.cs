using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Shell
{
    public class Utils
    {
        public static Shell Shell { get; set; }

        public static void SetStatus(string msg)
        {
            Shell.BeginInvoke((Action)(() =>
            {
                Shell.Status.Text = msg;
            }));
        }
    }
}
