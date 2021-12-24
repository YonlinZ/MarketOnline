using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketOnline.Shell.Model
{

    public class MiniTiker
    {
        public Tiker[] Tikers { get; set; }
    }

    public class Tiker
    {
        public string e { get; set; }
        public long E { get; set; }
        public string s { get; set; }
        public string c { get; set; }
        public string o { get; set; }
        public string h { get; set; }
        public string l { get; set; }
        public string v { get; set; }
        public string q { get; set; }
    }

}
