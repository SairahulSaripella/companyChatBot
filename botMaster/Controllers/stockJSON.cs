using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    [Serializable]
    public class stockJSON
    {
        public string id { get; set; }
        public string t { get; set; }
        public string e { get; set; }
        public string l { get; set; }
        public string l_fix { get; set; }
        public string l_cur { get; set; }
        public string s { get; set; }
        public string ltt { get; set; }
        public string lt_dts { get; set; }
        public string c { get; set; }
        public string c_fix { get; set; }
        public string cp { get; set; }
        public string cp_fix { get; set; }
        public string ccol { get; set; }
        public string pcls_fix { get; set; }

        public stockJSON()
        {
            // empty sontructor
        }


    }
}
