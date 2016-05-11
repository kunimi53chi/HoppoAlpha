using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace VisualFormTest
{
    public static class UserAgent
    {
        
        [DllImport("urlmon.dll", CharSet = CharSet.Ansi)]
        private static extern int UrlMkSetSessionOption(int dwOption, string str, int nLength, int dwReserved);
        const int URLMON_OPTION_USERAGENT = 0x10000001;
        static string ua = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

        //UserAgentの変更
        public static void SetUserAgent()
        {
            UrlMkSetSessionOption(URLMON_OPTION_USERAGENT, ua, ua.Length, 0);
        }
    }
}
