using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailChecker
{
    public class MetaStatus
    {
        public string ip;
        public string isp;
        public string status;
        public string country;
        public string code;

        public MetaStatus(string n, string s, string t, string o, string c)
        {
            ip = n;
            isp = s;
            status = t;
            country = o;
            code = c;
        }
    }
}
