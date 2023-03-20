using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailChecker
{
    public static class ConstEnv
    {
        public static readonly string PROXY_TYPE_HTTP = "http";
        public static readonly string PROXY_TYPE_SOCKS4 = "socks4";
        public static readonly string PROXY_TYPE_SOCKS5 = "socks5";

        public static readonly int STATE_BASE = 0;
        public static readonly int STATE_BROWSER_RESTART = 1;
        public static readonly int STATE_KILL_THREAD = 2;
        public static readonly int STATE_SUCCESS = 3;

        public static string proxy_type = "http";
    }
}
