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
        internal static async Task Start()
        {
            await InitialEngine.Start();


            CacheForm<FormKline>();
            CacheForm<FormAnalysis>();
            ConstVar.Shell = new Shell();
            Application.Run(ConstVar.Shell);

        }
        private static Form CacheForm<T>() where T : Form, new()
        {
            return MemoryCacheUtil.GetOrAddCacheItem<Form>(typeof(T).Name, () =>
            {
                var form = new T();
                form.Owner = ConstVar.Shell;
                //form.Dock = DockStyle.Fill;
                form.TopLevel = false;
                return form;

            }, null, DateTime.MaxValue);
        }


    }
}
