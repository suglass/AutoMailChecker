using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailChecker
{
    static class MainApp
    {
        public static System.Object g_locker = new object();
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static MainFrm g_main_frm = null;
        public static UserSetting g_setting = new UserSetting();
        public static int g_window_cnt = 0;
        [STAThread]
        static void Main()
        {
            g_setting = UserSetting.Load();
            if (g_setting == null)
                g_setting = new UserSetting();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            g_main_frm = new MainFrm();
            Application.Run(g_main_frm);
        }

        public static void log_info(string msg, bool msgbox = false)
        {
            lock (g_locker)
            {
                try
                {
                    logger.Info(msg);
                    if (msgbox)
                        MessageBox.Show(msg);
                    if (g_main_frm != null)
                    {
                        msg = DateTime.Now.ToString("dd.MM.yyyy_hh:mm:ss_") + msg;
                        g_main_frm.update_log(msg);
                        if (msgbox)
                            MessageBox.Show(msg);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static void log_error(string msg, bool msgbox = false)
        {
            lock (g_locker)
            {
                try
                {
                    logger.Error(msg);
                    if (msgbox)
                        MessageBox.Show(msg);
                    if (g_main_frm != null)
                    {
                        msg = DateTime.Now.ToString("yyyy.MM.dd hh:mm:ss ") + msg;
                        g_main_frm.update_log(msg);
                        if (msgbox)
                            MessageBox.Show(msg);
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

    }
}
