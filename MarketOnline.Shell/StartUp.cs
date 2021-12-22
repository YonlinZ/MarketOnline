using CommonHelper;
using MarketOnline.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarketOnline.Shell
{
    static class StartUp
    {
        public static Shell Shell { get; set; }

        internal static async Task Start()
        {
            await InitialEngine.Start();
            Shell = new Shell();


            CacheForm<FormKline>();
            Application.Run(Shell);

        }
        private static Form CacheForm<T>() where T : Form, new()
        {
            return MemoryCacheUtil.GetOrAddCacheItem<Form>(typeof(T).Name, () =>
            {
                var form = new T();
                form.Owner = Shell;
                form.Dock = DockStyle.Fill;
                form.TopLevel = false;
                return form;

            }, null, DateTime.MaxValue);
        }


    }
}
