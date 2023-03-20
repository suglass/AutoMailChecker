using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Interactions.Internal;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OS_Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Cookie = System.Net.Cookie;
using Size = System.Drawing.Size;
using System.Net.Http;
using Microsoft.Win32;
using MailChecker;
using Newtonsoft.Json;
using System.Text;
using System.Drawing.Imaging;

namespace MailChecker
{
    public class IGoogle
    {
        public bool m_must_terminate;
        
        public object m_locker = new object();
        public int m_ID;
        public Guid m_guid;
        public object m_chr_data_dir = new object();
        public object m_selen_locker = new object();
        public string m_chr_user_data_dir = "";
        public string m_chr_extension_dir = Environment.CurrentDirectory + "\\ChromeExtension";
        
        public ChromeDriver Driver;
        public IJavaScriptExecutor m_js;
 
        public string m_proxy;

        public IEnumerable<int> PID = null;

        public string m_err_str = "##$$##$$";
        public bool m_incognito = true;
        public bool m_dis_webrtc = false;
        public bool m_dis_cache = false;
        public bool m_dis_js = false;
        
        public int m_status;
        //public int m_check_status;

        public bool m_is_checked;

        public string[] public_url;

        public void DeleteCurrentChromeData()
        {
            try
            {
                Directory.Delete(m_chr_user_data_dir, true);
                return;
            }
            catch (Exception ex)
            {
                MainApp.log_error($"#{m_ID} - Deleting chrome data dir failed. {ex.Message}");
            }
        }

        public async Task<bool> Navigate(string target)
        {
            try
            {
                string url = Driver.Url;
                Driver.Navigate().GoToUrl(target);
                return await WaitUrlChange(url);
                //return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Start()
        {
            try
            {
                lock (m_chr_data_dir)
                {
                    m_guid = Guid.NewGuid();
                    m_chr_user_data_dir = $"ChromeData\\selenium_{Thread.CurrentThread.ManagedThreadId}" + m_guid.ToString();
                    Directory.CreateDirectory(m_chr_user_data_dir);
                }

                //MainApp.log_error($"#{m_ID} - Start...");
                try
                {
                    ChromeDriverService defaultService = ChromeDriverService.CreateDefaultService();
                    defaultService.HideCommandPromptWindow = true;
                    ChromeOptions chromeOptions = new ChromeOptions();
                    if (m_incognito)
                    {
                        chromeOptions.AddArguments("--incognito");
                    }

                    //chromeOptions.AddArgument("--start-maximized");
                    //chromeOptions.AddArgument("--auth-server-whitelist");
                    chromeOptions.AddArgument("--ignore-certificate-errors");
                    chromeOptions.AddArgument("--ignore-ssl-errors");
                    chromeOptions.AddArgument("--system-developer-mode");
                    chromeOptions.AddArgument("--no-first-run");
                    //chromeOptions.AddArguments("--disk-cache-size=0");
                    chromeOptions.AddArgument("--load-extension=" + m_chr_extension_dir + "\\proxy helper");
                    chromeOptions.AddArgument("--user-data-dir=" + m_chr_user_data_dir);

                    //chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddArguments("--disable-infobars");
                    chromeOptions.AddArguments("--disable-popup-blocking");

                    chromeOptions.AddArgument("--lang=en-ca");


                    /*chromeOptions.AddArgument("--window-size=1920,1080");
                    chromeOptions.AddArgument("--disable-gpu");*/
                    
                    //chromeOptions.AddArgument("--start-minimized");
                    //chromeOptions.AddArgument("--silent-launch");
                    //chromeOptions.AddArgument("headless");
                    chromeOptions.AddArgument("--window-position=-32000,-32000");

                    if (m_dis_webrtc)
                        chromeOptions.AddExtension(m_chr_extension_dir + "\\WebRTC Protect.crx");
                    if (m_dis_cache)
                        chromeOptions.AddExtension(m_chr_extension_dir + "\\CacheKiller.crx");

                    if (m_dis_js)
                        chromeOptions.AddArgument("--load-extension=" + m_chr_extension_dir + "\\jsoff-master");

                    string randomUserAgent = GetRandomUserAgent();
                    chromeOptions.AddArgument(string.Format("--user-agent={0}", (object)randomUserAgent));


                    chromeOptions.SetLoggingPreference(LogType.Driver, LogLevel.All);
                    chromeOptions.AddAdditionalCapability("useAutomationExtension", false);
                    chromeOptions.AddArgument("--no-sandbox");
                    //chromeOptions.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);

                    string chr_path = "";

                    string reg = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe";
                    RegistryKey registryKey;
                    using (registryKey = Registry.LocalMachine.OpenSubKey(reg))
                    {
                        if(registryKey != null)
                            chr_path = registryKey.GetValue("Path").ToString() + @"\chrome.exe";
                    }
                    if (chr_path == "")
                    {
                        if (Environment.Is64BitOperatingSystem)
                            chr_path = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
                        else
                            chr_path = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";

                        if (!System.IO.File.Exists(chr_path))
                        {
                            chr_path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\Application\chrome.exe";
                        }                       
                    }

                    if (!System.IO.File.Exists(chr_path))
                    {
                        MainApp.log_error($"#{m_ID} - chrome.exe Not found. Perhaps the Google Chrome browser is not installed on this computer.");
                        return false;
                    }
                    chromeOptions.BinaryLocation = chr_path;

                    try
                    {
                        Driver = new ChromeDriver(defaultService, chromeOptions);
                    }
                    catch (Exception ex)
                    {
                        MainApp.log_error($"#{m_ID} - Fail to start chrome.exe. Please make sure any other chrome windows are not opened.\n{ex.Message}");
                        return false;
                    }

                    m_js = (IJavaScriptExecutor)Driver;
                                        

                    //Driver.Navigate().Refresh();
                    //if(m_dis_cache)
                    //{
                    //    await Navigate("chrome-extension://kkmknnnjliniefekpicbaaobdnjjikfp/options.html");
                    //}
                    if (m_proxy != "" && !m_incognito && m_proxy.Split(':').Length == 3) // regular proxy setting
                    {
                        string ip = "";
                        string port = "";
                        string type = "";
                        //string login = "";
                        //string password = "";
                        
                        type = m_proxy.Split(':')[0];
                        ip = m_proxy.Split(':')[1];
                        port = m_proxy.Split(':')[2];
                        //login = m_proxy.Split(':')[2];
                        //password = m_proxy.Split(':')[3];
                           

                        await Navigate("chrome-extension://mnloefcpaepkpmhaoipjkpikbnkmbnic/options.html");
                        m_js.ExecuteScript("$('#http-host').val(\"" + ip + "\")", Array.Empty<object>());
                        m_js.ExecuteScript("$('#http-port').val(\"" + port + "\")", Array.Empty<object>());
                        m_js.ExecuteScript("$('#https-host').val(\"" + ip + "\")", Array.Empty<object>());
                        m_js.ExecuteScript("$('#https-port').val(\"" + port + "\")", Array.Empty<object>());
                        m_js.ExecuteScript("$('#socks-host').val(\"" + ip + "\")", Array.Empty<object>());
                        m_js.ExecuteScript("$('#socks-port').val(\"" + port + "\")", Array.Empty<object>());
                        //m_js.ExecuteScript("$('#username').val(\"" + login + "\")", Array.Empty<object>());
                        //m_js.ExecuteScript("$('#password').val(\"" + password + "\")", Array.Empty<object>());
                        if (type == ConstEnv.PROXY_TYPE_SOCKS5)
                        {
                            m_js.ExecuteScript("var a = document.getElementById(\"socks5\"); a.click();", Array.Empty<object>());
                            MainApp.log_info("Socks5 proxy is set.");
                        }
                        else
                        {
                            m_js.ExecuteScript("var a = document.getElementById(\"socks4\"); a.click();", Array.Empty<object>());
                            MainApp.log_info("Socks4 proxy is set.");
                        }
                        m_js.ExecuteScript("$('#proxy-rule').val(\"singleProxy\");", Array.Empty<object>());
                        m_js.ExecuteScript("save();", Array.Empty<object>());

                        bool is_success = false;
                        while (!is_success)
                        {
                            try
                            {
                                Driver.Navigate().GoToUrl("chrome-extension://mnloefcpaepkpmhaoipjkpikbnkmbnic/popup.html");
                                
                                if (type == ConstEnv.PROXY_TYPE_SOCKS4 || type == ConstEnv.PROXY_TYPE_SOCKS5)
                                    m_js.ExecuteScript("socks5Proxy();", Array.Empty<object>());
                                else if (type == ConstEnv.PROXY_TYPE_HTTP)
                                    m_js.ExecuteScript("httpProxy();", Array.Empty<object>());

                                is_success = true;
                            }
                            catch (Exception ex)
                            {
                                is_success = false;
                                await Task.Delay(100);
                            }
                        }
                    }
                    //if (!m_incognito && m_dis_js) // regular proxy setting
                    //{
                    //    await Navigate("chrome-extension://jfpdlihdedhlmhlbgooailmfhahieoem/options.html");
                    //}
                    
                    
                    //if (m_incognito == false)
                    //    await remove_all_cookies(); //<- not necessary in incogneto mode

                    MainApp.log_info($"#{m_ID} - {m_proxy} - Browser successfully started.");
                    return true;
                }
                catch (Exception ex)
                {
                    MainApp.log_error($"#{m_ID} - Failed to start. Exception:{ex.Message}\n{ex.StackTrace}");
                    try
                    {
                        Driver.Quit();
                    }
                    catch
                    {
                        MainApp.log_error($"#{m_ID} - Failed to quit driver. Exception:{ex.Message}");
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                MainApp.log_error($"#{m_ID} - Exception occured while trying to start chrome driver. Exception:{ex.Message}");
            }
            return false;
        }

  
        public async Task<bool> WaitUrlChange(string url, int timeout = 7000)
        {
            try
            {
                Stopwatch wt = new Stopwatch();
                wt.Start();
                while (wt.ElapsedMilliseconds < timeout)
                {
                    if (Driver.Url != url)
                        return true;
                    await TaskDelay(100);
                }
            }
            catch (Exception ex)
            {
                MainApp.log_error($"#{m_ID} - Failed to wait for url change. Exception:{ex.Message}");
            }
            return false;
        }

        public async Task<bool> TrySelect(By list, By optionToVerify, string textToSelect, int timeout = 5000)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < timeout)
            {
                if (Driver.FindElement(optionToVerify).Text == textToSelect)
                    return true;
                Driver.FindElement(list).SendKeys(textToSelect[0].ToString());
                await TaskDelay(100);
            }
            return false;
        }

  

        public bool IsElementPresent(By by)
        {
            try
            {
                Driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }

        public async Task<bool> WaitToVisible(string classname, int TimeOut = 1000)
        {
            return await WaitToVisible(By.ClassName(classname), TimeOut);
        }
        public async Task<bool> WaitToVisibleByPath(string xpath, int TimeOut = 1000)
        {
            return await WaitToVisible(By.XPath(xpath), TimeOut);
        }
        public async Task<bool> WaitToVisible(By by, int TimeOut = 1000)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < TimeOut)
            {
                if (await IsElementVisible(by))
                    return true;
                Thread.Sleep(100);
            }
            return false;
        }

        public async Task<bool> WaitToUnvisable(string classname, int TimeOut = 1000)
        {
            return await WaitToUnvisable(By.ClassName(classname), TimeOut);
        }
        public async Task<bool> WaitToUnvisable(By by, int TimeOut = 1000)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < TimeOut)
            {
                try
                {
                    if (!await IsElementVisible(by))
                        return true;
                }
                catch
                {
                    return false;
                }
                await TaskDelay(100);
            }
            return false;
        }

     
        public async Task<bool> WaitToPresent(string classname, int TimeOut = 2000)
        {
            return await WaitToPresent(By.ClassName(classname), TimeOut);
        }
        public async Task<bool> WaitToPresentByPath(string xpath, int TimeOut = 2000)
        {
            return await WaitToPresent(By.XPath(xpath), TimeOut);
        }

        public async Task<bool> WaitToPresent(By by, int TimeOut = 5000)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            do
            {
                if (IsElementPresent(by))
                    return true;
                await Task.Delay(1000);
            }
            while (wt.ElapsedMilliseconds < TimeOut);
            return false;
        }
    
     
        public void OpenNewTab(ChromeDriver driver, string url)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(string.Format("window.open('{0}', '_blank');", url));
        }
        


        public async Task<bool> TryClick_All(string xpath)
        {
            if (await TryClickByPath(xpath, 0))
                return true;
            if (await TryClickByPath(xpath, 1))
                return true;
            if (await TryClickByPath(xpath, 2))
                return true;
            bool ret = false;
            try
            {
                m_js.ExecuteAsyncScript($"document.evaluate('{xpath}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click()");
                ret = true;
            }
            catch (Exception ex)
            {
                ret = false;
            }

            if (ret == false)
                MainApp.log_error($"{m_ID} : Clicking all ways failed. {xpath}");
            return ret;
        }
        public async Task<bool> TryClick(string classname, int mode = 0, int delay = 100)
        {
            try
            {
                await TryClick(By.ClassName(classname), mode);
                await TaskDelay(delay);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> TryClickByPath(string xpath, int mode = 0, int delay = 100)
        {
            try
            {
                await TryClick(By.XPath(xpath), mode);
                await TaskDelay(delay);
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public async Task<bool> TryClick(By by, int mode)
        {
            try
            {
                if (mode == 0)
                {
                    Driver.ExecuteScript("arguments[0].click('');", ((RemoteWebDriver)Driver).FindElement(by));
                }
                else if (mode == 1)
                {
                    Driver.FindElement(by).Click();
                }
                else if (mode == 2)
                {
                    Actions action = new Actions(Driver);
                    action.MoveToElement(Driver.FindElement(by)).Perform();
                    action.Click(Driver.FindElement(by)).Perform();
                }

                return true;
            }
            catch (Exception ex) { }
            return false;
        }

        public async Task<bool> TryEnterText_by_xpath(string xpath, string textToEnter, string atributeToEdit = "value", int TimeOut = 10000, bool manualyEnter = false)
        {
            return await TryEnterText(By.XPath(xpath), textToEnter, atributeToEdit, TimeOut, manualyEnter);
        }
        public async Task<bool> TryEnterText(string classname, string textToEnter, string atributeToEdit = "value", int TimeOut = 10000, bool manualyEnter = false)
        {
            return await TryEnterText(By.ClassName(classname), textToEnter, atributeToEdit, TimeOut, manualyEnter);
        }

        public async Task<bool> TryEnterText(By by, string textToEnter, string atributeToEdit = "value", int TimeOut = 10000, bool manualyEnter = false)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < TimeOut)
            {
                try
                {
                    if (IsElementPresent(by) && await IsElementVisible(by))
                    {
                        Driver.FindElement(by).SendKeys((string)Keys.Control + "a");
                        if (manualyEnter)
                            Driver.FindElement(by).SendKeys(textToEnter);
                        else
                            Driver.ExecuteScript($"arguments[0].value = '{textToEnter}';", ((RemoteWebDriver)Driver).FindElement(by));

                        for (int index = 0; index < 11; ++index)
                        {
                            if ((string)Driver.ExecuteScript("return arguments[0].value;", Driver.FindElement(by)) == textToEnter)
                            {
                                return true;
                            }
                            await TaskDelay(100);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainApp.log_error($"#{m_ID} - Failed to enter text. Exception:{ex.Message}");
                    return false;
                }
                await Task.Delay(100);
            }
            return false;
        }
    

        public async Task<bool> WaitToVisable(string xpath, int TimeOut = 1000)
        {
            return await WaitToVisable(By.XPath(xpath), TimeOut);
        }
        public async Task<bool> WaitToVisable(By by, int TimeOut = 1000)
        {
            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < TimeOut)
            {
                if (await IsElementVisible(by))
                    return true;
                Thread.Sleep(100);
            }
            return false;
        }

        public async Task<bool> TryClickAndWait(string toClick, string toWait, int mode = 0, int TimeOut = 10000)
        {
            return await TryClickAndWait(By.XPath(toClick), By.XPath(toWait), mode, TimeOut);
        }
        public async Task<bool> TryClickAndWait(By toClick, By toWait, int mode = 0, int TimeOut = 10000)
        {
            if (!await WaitToPresent(toClick, 3000))
            {
                MainApp.log_error($"#{m_ID} - Element to be clicked is not present! mode:{mode} By: {toClick}");
                return false;
            }

            Stopwatch wt = new Stopwatch();
            wt.Start();
            while (wt.ElapsedMilliseconds < TimeOut)
            {
                try
                {
                    if (mode == 1)
                    {
                        string script = @"(function(x) {
                            var el = document.evaluate('" + toClick + @"', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
                            let hoverEvent = document.createEvent ('MouseEvents');
                            hoverEvent.initEvent ('mouseover', true, true);
                            el.dispatchEvent (hoverEvent);

                            let downEvent = document.createEvent ('MouseEvents');
                            downEvent.initEvent ('mousedown', true, true);
                            el.dispatchEvent (downEvent);

                            let upEvent = document.createEvent ('MouseEvents');
                            upEvent.initEvent ('mouseup', true, true);
                            el.dispatchEvent (upEvent);

                            let clickEvent = document.createEvent ('MouseEvents');
                            clickEvent.initEvent ('click', true, true);
                            el.dispatchEvent (clickEvent);
                            })();";
                        Driver.ExecuteScript(script);
                        if (!await WaitToPresent(toWait, TimeOut))
                        {
                            MainApp.log_error($"#{m_ID} - Click failed for waiting! mode:{mode} By: {toClick}");
                            return false;
                        }
                        MainApp.log_error($"#{m_ID} - Click success! mode:{mode} By: {toClick}");
                        return true;
                    }
                    else if (mode == 0)
                    {
                        Driver.ExecuteScript("arguments[0].click('');", Driver.FindElement(toClick));
                        if (!await WaitToPresent(toWait, TimeOut))
                        {
                            MainApp.log_error($"#{m_ID} - Click failed for waiting! mode:{mode} By: {toClick}");
                            return false;
                        }

                        MainApp.log_error($"#{m_ID} - Click success! mode:{mode} By: {toClick}");
                        return true;
                    }
                    else if (mode == 2)
                    {
                        Driver.FindElement(toClick).Click();
                        if (!await WaitToPresent(toWait, TimeOut))
                        {
                            MainApp.log_error($"#{m_ID} - Click failed for waiting! mode:{mode} By: {toClick}");
                            return false;
                        }

                        MainApp.log_error($"#{m_ID} - Click success! mode:{mode} By: {toClick}");
                        return true;
                    }
                    else if (mode == 3)
                    {
                        Actions action = new Actions(Driver);
                        action.MoveToElement(Driver.FindElement(toClick)).Perform();
                        action.Click(Driver.FindElement(toClick)).Perform();
                        if (!await WaitToPresent(toWait, TimeOut))
                        {
                            MainApp.log_error($"#{m_ID} - Click failed for waiting! mode:{mode} By: {toClick}");
                            return false;
                        }
                        MainApp.log_error($"#{m_ID} - Click success! mode:{mode} By: {toClick}");
                        return true;
                    }
                }
                catch
                {

                }
            }
            MainApp.log_error($"#{m_ID} - Click failed for waiting! mode:{mode} By: {toClick}");
            return false;
        }

        public async Task<bool> IsElementVisible(By by, int timeout = 0)
        {
            try
            {
                Stopwatch wt = new Stopwatch();
                wt.Start();
                do
                {
                    if (IsElementVisible(Driver.FindElement(by)))
                        return true;
                    await TaskDelay(100);
                } while (wt.ElapsedMilliseconds < timeout);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsElementVisible(IWebElement element)
        {
            return element.Displayed && element.Enabled;
        }

        public async Task<bool> Quit()
        {
            try
            {
                foreach (var hnd in Driver.WindowHandles)
                {
                    Driver.SwitchTo().Window(hnd);
                    Driver.Close();
                }
                Driver.Quit();
                Driver.Dispose();
                DeleteCurrentChromeData();
                return true;
            }

            catch (Exception ex)
            {
                MainApp.log_error($"Thread #{m_ID} - Error catched in Quit() - {ex.Message}");
                return false;
            }
        }
        public async Task<bool> Quit_undelete_data()
        {
            try
            {
                foreach (var hnd in Driver.WindowHandles)
                {
                    Driver.SwitchTo().Window(hnd);
                    Driver.Close();
                }
                Driver.Quit();
                Driver.Dispose();
                return true;
            }

            catch (Exception ex)
            {
                //MainApp.log_error($"Thread #{m_ID} - Error catched in Quit_undelete_data() - {ex.Message}");
                return false;
            }
        }

        public IGoogle()
        {
        }



        public static void KillAllChromeDriverProcess()
        {
            MainApp.log_error("Killing all chrome drivers");
            int id = Process.GetCurrentProcess().Id;
            var list1 = new List<Process>((IEnumerable<Process>)Process.GetProcessesByName("chromedriver"));
            var list2 = new List<Process>((IEnumerable<Process>)Process.GetProcessesByName("chrome"));
            foreach (Process proc in list1)
            {
                if (proc.GetParentID() == id)
                {
                    foreach (Process proc2 in list2)
                    {
                        if (proc2.GetParentID() == proc.Id)
                        {
                            new Thread((ThreadStart)(() =>
                            {
                                try
                                {
                                    proc2.Kill();
                                }
                                catch
                                {
                                }
                            })).Start();
                        }
                    }
                    new Thread((ThreadStart)(() =>
                    {
                        try
                        {
                            proc.Kill();
                        }
                        catch
                        {
                        }
                    })).Start();
                }
            }
        }

        public Process FindLatestChromeProcess()
        {
            Process ret = null;
            foreach (Process process in new List<Process>((IEnumerable<Process>)Process.GetProcessesByName("chrome")))
            {
                if (ret == null || process.StartTime > ret.StartTime)
                    ret = process;
                //if (process.GetParentID() == ParantID)
            }
            return ret;
        }

        public void ClearChromeData()
        {
            try
            {
                string path = "ChromeData";
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            catch
            {
            }
        }

        public static string GetRandomUserAgent()
        {
            string[] strArray = new string[]
            {
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36",
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.157 Safari/537.36",
                "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.90 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36 OPR/43.0.2442.991",
                "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 YaBrowser/17.10.0.2052 Yowser/2.5 Safari/537.36",
                "Mozilla/5.0 (Windows NT 5.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36 OPR/43.0.2442.991",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.89 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
                "Mozilla/5.0 (compatible; U; ABrowse 0.6; Syllable) AppleWebKit/420+ (KHTML, like Gecko)",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/72.0.3626.121 Safari/537.36",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.75 Safari/537.36 OPR/36.0.2130.32",
                "Opera/9.80 (Windows NT 6.1; WOW64) Presto/2.12.388 Version/12.18",
                "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36 OPR/43.0.2442.991",
                "Opera/9.80 (Windows NT 6.0) Presto/2.12.388 Version/12.14",
                "Opera/9.80 (Windows NT 5.1; WOW64) Presto/2.12.388 Version/12.17"
            };
            return strArray[new Random().Next(0, strArray.Length)];
        }
      
        public async Task TaskDelay(int delay)
        {
            await Task.Delay(delay);
        }

        public async Task RandomWait()
        {
            int delay = (new Random()).Next(100, 1500);
            await Task.Delay(delay);
        }
        
    }
}
