using HtmlAgilityPack;
using MaterialSkin;
using MaterialSkin.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MailChecker
{
    public partial class MainFrm : MaterialForm
    {
        MaterialSkinManager skinman;
        List<string> m_account = new List<string>();
        List<string> m_proxy = new List<string>();
        List<string> m_nordvpn_server_list = new List<string>();

        //List<Google> googles = new List<Google>();
        List<Google> googles = new List<Google>();
        List<string> m_work_mail_list = new List<string>();
        List<string> m_failed_checked_mail_list = new List<string>();

        int m_thread_num = 0;
        int m_last_used_proxy_idx = 0;
        bool[] m_proxy_dead = null;
        bool m_must_close = true;
        bool m_must_finish = false;

        object m_lock = new object();

        int g_finish_thread;
        int g_check_mails_count;
        bool g_must_change_server;
        public MainFrm()
        {
            InitializeComponent();
            skinman = MaterialSkinManager.Instance;
            skinman.AddFormToManage(this);
            skinman.Theme = MaterialSkinManager.Themes.DARK;
            skinman.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.Blue200, Accent.Teal200, TextShade.WHITE);
            g_finish_thread = 0;
            g_check_mails_count = 0;
            g_must_change_server = false;
        }

        private void btn_open_acc_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Open a file containing e-mail address";
                dlg.Filter = "CSV files|*.CSV|All files|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txt_acc_path.Text = dlg.FileName;
                    MainApp.log_info($"Google account file selected: {dlg.FileName}");
                    load_accounts(dlg.FileName);
                }
            }
            catch (Exception ex)
            {
                MainApp.log_error(ex.Message + "\n" + ex.StackTrace, true);
            }
        }

        private bool load_accounts(string filename)
        {
            m_account = File.ReadAllLines(filename).ToList();
            if (m_account.Count == 0)
            {
                MainApp.log_error("The file does not contain any e-mails.", true);
                return false;
            }
            m_failed_checked_mail_list.Clear();
                
            MainApp.log_info($"{m_account.Count} accounts are loaded.");
            return true;
        }
        private bool load_server_list(string filename)
        {
            m_nordvpn_server_list = File.ReadAllLines(filename).ToList();
            if (m_nordvpn_server_list.Count == 0)
            {
                MainApp.log_error("The file does not contain any servers.", true);
                return false;
            }

            MainApp.log_info($"{m_nordvpn_server_list.Count} servers are loaded.");
            return true;
        }

        private void load_free_proxy_list()
        {
            lock(m_proxy)
            {
                WebClient wc = new WebClient();

                var page = wc.DownloadString("https://free-proxy-list.net/");
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(page);
                HtmlNode w_node = doc.DocumentNode;

                HtmlNodeCollection w_proxies = w_node.SelectSingleNode("//tbody").SelectNodes(".//tr");

                foreach (var proxy in w_proxies)
                {
                    string ip = proxy.SelectNodes(".//td")[0].InnerText;
                    string port = proxy.SelectNodes(".//td")[1].InnerText;
                    m_proxy.Add(ip + ":" + port);
                }
            }
        }

        private bool load_proxies(string filename)
        {
            m_proxy = File.ReadAllLines(filename).ToList();
            if (m_proxy.Count == 0)
            {
                MainApp.log_error("The file does not contain any proxies.", true);
                return false;
            }
            foreach (string s in m_proxy)
            {
                string[] x = s.Split(':');
                if (x.Length != 2)
                {
                    MainApp.log_error("The file is not a valid proxy file. -> " + x, true);
                    m_proxy.Clear();
                    return false;
                }
            }

            frmProxyType dlg = new frmProxyType();
            if (dlg.ShowDialog() != DialogResult.OK)
                return false;

            MainApp.log_info($"{m_proxy.Count} proxies are loaded.");
            return true;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            //if(System.Diagnostics.Debugger.IsAttached)
            {
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\account.csv"))
                {
                    txt_acc_path.Text = Path.GetDirectoryName(Application.ExecutablePath) + "\\account.csv";
                    load_accounts(txt_acc_path.Text);
                }
                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "\\proxy.txt"))
                {
                    txt_proxy_path.Text = Path.GetDirectoryName(Application.ExecutablePath) + "\\proxy.txt";
                    load_proxies(txt_proxy_path.Text);
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "disconnect_vpn.bat") || !File.Exists(AppDomain.CurrentDomain.BaseDirectory + "run_vpn.bat"))
                {
                    MessageBox.Show("No bat files.");
                    System.Windows.Forms.Application.Exit();
                }
            }
            MainApp.log_info($"first url {MainApp.g_setting.first_url}");
            MainApp.log_info($"delat time #{MainApp.g_setting.delay_time} (s)");
        }

        public void update_log(string log)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                txt_last_log.Text = log;
                txt_log.Text = log + "\n" + txt_log.Text;
            });
        }

        private void btn_open_proxy_Click(object sender, EventArgs e)
        {
            try
            {
                return;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "Open a text file containing proxies";
                dlg.Filter = "TXT files|*.txt";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (!load_proxies(dlg.FileName))
                        return;
                    txt_proxy_path.Text = dlg.FileName;
                    MainApp.log_info($"Proxy file selected: {dlg.FileName}");
                }
            }
            catch (Exception ex)
            {
                MainApp.log_error(ex.Message + "\n" + ex.StackTrace, true);
            }
        }
        
        
        private string take_from_work_mail_list()
        {
            string mail = "";

            lock (m_work_mail_list)
            {
                if (m_work_mail_list.Count > 0)
                {
                    mail = m_work_mail_list[0];
                    m_work_mail_list.RemoveAt(0);
                }
            }
            return mail;
        }

        private void add_unsuccess_checked_mails(string mail)
        {

        }
        private string take_from_work_proxy_list()
        {
            string proxy = "";

            lock (m_proxy)
            {
                if (m_proxy.Count > 0)
                {
                    proxy = m_proxy[0];
                    m_proxy.RemoveAt(0);
                }
                else
                {
                    WebClient wc = new WebClient();

                    var page = wc.DownloadString("https://free-proxy-list.net/");
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(page);
                    HtmlNode w_node = doc.DocumentNode;

                    HtmlNodeCollection w_proxies = w_node.SelectSingleNode("//tbody").SelectNodes(".//tr");

                    if (w_proxies == null)
                        return proxy;

                    foreach (var item in w_proxies)
                    {
                        if (item.SelectNodes(".//td")[4].InnerText == "elite proxy" && item.SelectNodes(".//td")[6].InnerText == "yes")
                        {
                            string ip = item.SelectNodes(".//td")[0].InnerText;
                            string port = item.SelectNodes(".//td")[1].InnerText;
                            m_proxy.Add(ConstEnv.PROXY_TYPE_HTTP + ":" + ip + ":" + port);
                        }
                    }

                    page = wc.DownloadString("https://www.socks-proxy.net");
                    doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(page);
                    w_node = doc.DocumentNode;

                    w_proxies = w_node.SelectSingleNode("//tbody").SelectNodes(".//tr");

                    if (w_proxies == null)
                        return proxy;

                    foreach (var item in w_proxies)
                    {
                        string ip = item.SelectNodes(".//td")[0].InnerText;
                        string port = item.SelectNodes(".//td")[1].InnerText;
                        m_proxy.Add(ConstEnv.PROXY_TYPE_SOCKS4 + ":" + ip + ":" + port);                           
                    }
                }
            }
            return proxy;
        }

        private string take_from_nordvpn_server_list()
        {
            string server_name = "";

            lock (m_nordvpn_server_list)
            {
                if (m_nordvpn_server_list.Count > 0)
                {
                    //server_name = m_nordvpn_server_list[0];
                    //m_nordvpn_server_list.RemoveAt(0);
                    int count = m_nordvpn_server_list.Count;
                    Random random = new Random();
                    int index = random.Next(0, count - 1);
                    server_name = m_nordvpn_server_list[index];
                    m_nordvpn_server_list.RemoveAt(index);
                }
                else
                {
                    add_server_list();
                }
            }
            return server_name;
        }

        private bool change_vpn_server(string server)
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "run_vpn.bat"))
                {
                    Process proc = null;

                    string _batDir = AppDomain.CurrentDomain.BaseDirectory;
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = _batDir;
                    proc.StartInfo.FileName = "run_vpn.bat";
                    proc.StartInfo.CreateNoWindow = false;
                    proc.StartInfo.Arguments = $"\"{server}\"";
                    
                    proc.Start();
                    proc.WaitForExit();

                    //ExitCode = proc.ExitCode;
                    proc.Close();                    

                    return true;
                }
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Connection vpn server failed - {ex.Message}");
            }
            return false;
        }
        private void disconnect_vpn_server()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "disconnect_vpn.bat"))
                {
                    Process proc = null;

                    string _batDir = AppDomain.CurrentDomain.BaseDirectory;
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = _batDir;
                    proc.StartInfo.FileName = "disconnect_vpn.bat";
                    proc.StartInfo.CreateNoWindow = false;
                    proc.Start();
                    proc.WaitForExit();

                    //ExitCode = proc.ExitCode;
                    proc.Close();
                }
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Disconnection vpn server failed - {ex.Message}");
            }
        }

        private async void btn_launch_Click(object sender, EventArgs e)
        {
            string message = "Did you run and log in to nordvpn?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.No)
                return;
            m_must_finish = false;
            this.WindowState = FormWindowState.Minimized;
            
            try
            {
                string folder_name = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("dd-MM-yyyy.hh.mm.ss");
                Directory.CreateDirectory(folder_name);

                string public_mail_dir = folder_name + "\\" + MainApp.g_setting.public_mail;
                string unpublic_mail_dir = folder_name + "\\" + MainApp.g_setting.unpublic_mail;

                if (FileInUse(public_mail_dir) || FileInUse(unpublic_mail_dir))
                {
                    MessageBox.Show("Close public.csv and unpublic.csv. And then, try again.");
                    return;
                }

                g_finish_thread = 0;
                g_check_mails_count = 0;

                //this.Visible = false;

                MainApp.log_info("Launching browsers started.");
                if (googles.Count > 0)
                {
                    MessageBox.Show("Please close all the working browsers.");
                    return;
                }

                m_thread_num = 0;
                if (!int.TryParse(txt_thread_num.Text, out m_thread_num) || m_thread_num < 0)
                {
                    MessageBox.Show("Please set the valid thread number.");
                    return;
                }

                if (m_account.Count == 0)
                {
                    MessageBox.Show("No account info.");
                    return;
                }

                if (m_thread_num > m_account.Count)
                {
                    m_thread_num = m_account.Count;
                    txt_thread_num.Text = m_thread_num.ToString();
                    MainApp.log_info(string.Format("Thread number has been adjusted as {0}", m_thread_num));
                }

                m_must_close = true;

                foreach (var g in googles)
                {
                    if (g != null)
                        await g.Quit();
                }
                googles.Clear();

                // Init proxy info.

                // Assign work param list.

                m_work_mail_list.Clear();
                for (int i = 0; i < m_account.Count; i++)
                {
                    m_work_mail_list.Add(m_account[i]);
                }

                // Create threads.

                m_must_close = false;

                MainApp.g_window_cnt = m_thread_num;

                bool is_check_finished = false;

                g_check_mails_count = 0;

                while (!m_must_close && !is_check_finished)
                {
                    m_thread_num = Math.Min(m_thread_num, m_work_mail_list.Count);
                    MainApp.log_info($"Thread counts = {m_thread_num}");

                    if (!await set_vpn_connection_no_check())
                    {
                        MainApp.log_error("VPN connection failed");
                        return;
                    }

                    bool is_list_blank = false;
                    //bool must_thread_end = false;

                    g_finish_thread = 0;
                    for (int i = 0; i < m_thread_num; i ++)
                    {
                        int tid = i;

                        new Thread((ThreadStart)(async () =>
                        {
                            MainApp.log_info($"Thread #{tid} started.");

                            Google g = new Google("");
                            g.m_ID = tid;
                            googles.Add(g);

                            //string mail = take_from_work_mail_list();

                            while (!m_must_close && !is_list_blank)
                            {
                                bool success = await g.open_browser();

                                if (success)
                                {
                                    while (!m_must_close)
                                    {
                                        /*if (must_thread_end)
                                            break;*/

                                        string mail = take_from_work_mail_list();

                                        if (mail == "")
                                        {
                                            is_list_blank = true;
                                            break;
                                        }

                                        if (await g.check_mail(mail))
                                        {
                                            MainApp.log_info($"Thread #{g.m_ID} - {mail} check finished successfully.");
                                            g_check_mails_count++;
                                            MainApp.log_info($"{g_check_mails_count} mails check finished until now.");

                                            string new_line = mail;

                                            if (g.public_url[0] != "")
                                            {
                                                lock (m_lock)
                                                {
                                                    using (StreamWriter stream = File.AppendText(public_mail_dir))
                                                    {
                                                        foreach (string item in g.public_url)
                                                            if (item != "")
                                                            {
                                                                new_line = new_line + "," + item;
                                                            }
                                                        stream.WriteLine(new_line);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lock (m_lock)
                                                {
                                                    using (StreamWriter stream = File.AppendText(unpublic_mail_dir))
                                                        stream.WriteLine(new_line);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            lock (m_lock)
                                            {
                                                m_work_mail_list.Add(mail);
                                                MainApp.log_info($"Thread #{g.m_ID} - {mail} is added to work_mail_list again.");
                                            }
                                            break;
                                        }
                                    }
                                }

                                /*if (must_thread_end)
                                    break;*/

                                if (g.m_status == ConstEnv.STATE_BROWSER_RESTART)
                                {
                                    MainApp.log_info($"Thread #{g.m_ID} - {g.m_status} - Browser restart.");
                                    await g.Quit_undelete_data();
                                }
                                else
                                {
                                    //must_thread_end = true;
                                    break;
                                }
                            }

                            MainApp.log_info($"Browser on thread #{g.m_ID} finished.");
                            await g.Quit_undelete_data();
                            g_finish_thread++;
                            MainApp.log_info($"{g_finish_thread} threads finished.");
                        })).Start();
                    }

                    await Task.Run(async () =>
                    {
                        while (g_finish_thread < m_thread_num)
                        {
                            await Task.Delay(100);                            
                        }
                        await disconnect_vpn_no_check();
                        IGoogle.KillAllChromeDriverProcess();
                        if (m_work_mail_list.Count == 0)
                        {
                            is_check_finished = true;

                            Invoke(new Action(() =>
                            {
                                this.WindowState = FormWindowState.Normal;
                                MainApp.log_info($"{g_check_mails_count} mails check finished finally.");
                            }));

                            try
                            {
                                Directory.Delete("ChromeData", true);
                                MainApp.log_info("Successfully deleted folder.");
                            }
                            catch (Exception ex)
                            {
                                MainApp.log_info("Unsuccessfully deleted folder. It will be deleted at the next time.");
                            }                            
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MainApp.log_error($"Error catched in main function - {ex.Message}");
            }
        }
                

        private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            m_must_close = true;
            m_must_finish = true;
            foreach (var g in googles)
            {
                if (g != null)
                    g.m_must_terminate = true;
            }
            //IGoogle.KillAllChromeDriverProcess();
            MainApp.g_setting.Save();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            m_must_close = true;
            m_must_finish = true;
            foreach (var g in googles)
            {
                if (g != null)
                    g.m_must_terminate = true;
            }
            googles.Clear();
            //IGoogle.KillAllChromeDriverProcess();
            MainApp.log_info("All browsers are closed.");
        }

        private bool Update_Account_Text(string mail)
        {
            lock (m_account)
            {
                try
                {
                    for (int k = 0; k < m_account.Count; k++)
                    {
                        MainApp.log_info(m_account[k]);
                        if (m_account[k] == mail)
                        {
                            m_account.RemoveAt(k);
                            System.IO.File.WriteAllLines(txt_acc_path.Text, m_account);
                            MainApp.log_info($"account : {mail} deleted..");
                            break;
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    MainApp.log_info(string.Format("Catch Exception: {0}", e.Message));
                    return false;
                }

            }
        }

        public bool FileInUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    bool temp = fs.CanWrite;
                }
                return false;
            }
            catch (IOException ex)
            {
                return true;
            }
        }

        public MetaStatus get_json_from_server()
        {
            try
            {
                string url = "https://nordvpn.com/api/vpn/check/full";
                var w = new WebClient();
                string json_data = w.DownloadString(url);
                MainApp.log_info($"Json response - {json_data}");
                MetaStatus meta_status = JsonConvert.DeserializeObject<MetaStatus>(json_data);
                return meta_status;
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error catched in get_json_from_server - {ex.Message}");
                return null;
            }
        }

        public void add_server_list()
        {
            try
            {
                WebClient wc = new WebClient { Encoding = System.Text.Encoding.UTF8 };
                string page = wc.DownloadString("https://api.nordvpn.com/v1/servers?limit=16384%22%20|%20jq%20--raw-output%20%27.[].hostname%27%20|%20sort%20--version-sort");
                string str_key = "\"name\":\"";

                int counts = 0;
                while (page.IndexOf(str_key) != -1)
                {
                    page = page.Substring(page.IndexOf(str_key) + str_key.Length);
                    string w_str_server_name = page.Substring(0, page.IndexOf("\""));
                    if (w_str_server_name.IndexOf("#") != -1)
                    {
                        m_nordvpn_server_list.Add(w_str_server_name);
                        counts++;
                        lock (m_lock)
                        {
                            File.Delete(MainApp.g_setting.server_list_fname);
                            using (StreamWriter stream = File.AppendText(MainApp.g_setting.server_list_fname))
                            {
                                stream.WriteLine(w_str_server_name);
                            }
                        }
                    }
                }

                if (counts == 0 && File.Exists(AppDomain.CurrentDomain.BaseDirectory + MainApp.g_setting.server_list_fname))
                {
                    load_server_list(AppDomain.CurrentDomain.BaseDirectory + MainApp.g_setting.server_list_fname);
                }
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error catched in add _server_list - {ex.Message}");
            }
        }

        private async Task<bool> set_vpn_connection_no_check()
        {
            try
            {
                string server_name = "";
                while (true)
                {
                    if (m_must_close == true)
                        break;
                    server_name = take_from_nordvpn_server_list();
                    MainApp.log_info($"NordVPN server : {server_name} loaded.");
                    if (server_name != "")
                        break;
                    await Task.Delay(2000);
                }
                MainApp.log_info("Please wait for 60 seconds.");
                change_vpn_server(server_name);
                await Task.Delay(60000);
                MainApp.log_info("Successfully connected.");
                return true;
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error in set_vpn_connection_no_check - {ex.Message}");
            }
            return false;
        }
        private async Task<bool> disconnect_vpn_no_check()
        {
            try
            {
                disconnect_vpn_server();
                MainApp.log_info("Please wait for 60 seconds.");
                await Task.Delay(60000);
                MainApp.log_info("Successfully disconnected.");
                return true;
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error catched in disconnect_vpn_no_check - {ex.Message}");
            }
            return false;
        }

        private async Task<bool> set_vpn_connection()
        {
            string server_name = "";

            try
            {
                int counts = 0;
                bool set_proxy = false;
                while (counts <= 5)
                {
                    while(true)
                    {
                        server_name = take_from_nordvpn_server_list();
                        if (server_name != "")
                            break;
                        await Task.Delay(2000);
                    }                     

                    MainApp.log_info($"NordVPN server {server_name} is loaded.");
                    counts++;
                    Stopwatch wt = new Stopwatch();
                    wt.Start();
                    while (wt.ElapsedMilliseconds < 120000)
                    {
                        bool flag = change_vpn_server(server_name);
                        if (m_must_close == true)
                            break;
                        MetaStatus state = get_json_from_server();
                        if(state != null)
                        {
                            MainApp.log_info($"NordVPN connection state : {state.status}");
                            if (state.status == "Protected")
                            {
                                set_proxy = true;
                                break;
                            }
                        }                        
                        await Task.Delay(10000);
                    }
                    if (set_proxy)
                        return true;
                }
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error catched in task set vpn connection - {ex.Message}");
            }
            return false;
        }

        private async Task<bool> disconnect_current_vpn_connection()
        {
            int counts = 0;
            try
            {
                bool is_disconnected = false;
                while (counts <= 5)
                {
                    if (m_must_close == true)
                        break;
                    counts ++;
                    disconnect_vpn_server();
                    Stopwatch wt = new Stopwatch();
                    wt.Start();
                    while (wt.ElapsedMilliseconds < 120000)
                    {
                        MetaStatus state = get_json_from_server();
                        MainApp.log_info($"NordVPN connection state : {state.status}");
                        if (state.status == "Unprotected")
                        {
                            is_disconnected = true;
                            break;
                        }
                        await Task.Delay(10000);
                    }
                    if (is_disconnected)
                        return true;
                }
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error in task disconnect current connection - {ex.Message}");
            }
            return false;   
        }
    }
}
