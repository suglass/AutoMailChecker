using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

namespace MailChecker
{
    public class Google : IGoogle
    {       
        public Google(string proxy)
        {
            m_must_terminate = false;

            m_proxy = proxy;

            public_url = new string[] { "", "" };
            m_incognito = false;
            m_dis_js = false;
            m_dis_webrtc = false;
            m_status = ConstEnv.STATE_BASE;
            //m_check_status = ConstEnv.STATUS_CHECK_START;
            m_is_checked = false;
        }

        public async Task<bool> refresh()
        {
            Driver.Navigate().Refresh();
            return true;
        }
        public async Task<bool> open_browser()
        {
            try
            {
                m_status = ConstEnv.STATE_BASE;

                if (!await Start())
                {
                    MainApp.log_error("Chrome starting failed");
                    return false;
                }

                if (m_must_terminate)
                {
                    return false;
                }

                if (!await Navigate(MainApp.g_setting.first_url))
                {
                    MainApp.log_error($"# navi to first url {MainApp.g_setting.first_url} failed.");
                    //Driver.Close();
                    return false;
                }
//                 Driver.Navigate().GoToUrl(MainApp.g_setting.first_url);
//                 MainApp.log_error($"#{m_ID} - Went to first url.");
//                 Driver.Navigate().Refresh();
//                 MainApp.log_error($"#{m_ID} - Refresh finished.");

                if (m_must_terminate)
                {
                    return false;
                }

                string xpath = "//input[@title='Search']";
                
                if (!await WaitToPresentByPath(xpath))
                {
                    string xpath1 = "//input[@title='Google Search']";
                    if (await WaitToPresentByPath(xpath1))
                        m_status = ConstEnv.STATE_BROWSER_RESTART;
                    else
                        MainApp.log_error($"#{m_ID} - Google page is not shown.");
                    //Driver.Close();
                    return false;
                }

                if (await WaitToVisibleByPath("//div[@id='recaptcha']") || await WaitToPresentByPath("//div[@id='recaptcha']"))
                {
                    MainApp.log_error($"#Thread - {m_ID} - Captcha was appeared.");
                    m_status = ConstEnv.STATE_KILL_THREAD;
                    return false;
                }

                MainApp.log_info($"first url navigated successfully.");

                if (m_must_terminate)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                MainApp.log_error(ex.Message + "\n" + ex.StackTrace);
                //Driver.Close();
                return false;
            }
        }
        public async Task<bool> check_mail(string mail)
        {
            public_url = new string[] { "", "" };
            //public_url[0] = "NULL";
            await RandomWait();

            try
            {
                //m_browser_status = ConstEnv.BROWSER_OPEN;
                m_is_checked = false;

                MainApp.log_info($"Thread #{m_ID} - Checking started. mail = {mail}");
       
                int timeout = MainApp.g_setting.delay_time * 1000;

                string xpath = "//input[@title='Search']";
                
                string input_temp = "\"" + mail + "\"";

                if (m_must_terminate)
                {
                    return false;
                }

                if (!await WaitToPresentByPath(xpath))
                {
                    string xpath1 = "//input[@title='Google Search']";
                    if (await WaitToPresentByPath(xpath1))
                        m_status = ConstEnv.STATE_BROWSER_RESTART;
                    else
                        MainApp.log_error($"Thread #{m_ID} - Google page is not shown.");
                    //Driver.Close();
                    return false;
                }

                await TryEnterText_by_xpath(xpath, input_temp, "value", 3000, true);
                Driver.FindElementByXPath(xpath).SendKeys(Keys.Return);

                await WaitUrlChange(MainApp.g_setting.first_url);
                await TaskDelay(2000);

                if (await WaitToVisibleByPath("//div[@id='recaptcha']") || await WaitToPresentByPath("//div[@id='recaptcha']"))
                {
                    MainApp.log_error($"#Thread - {m_ID} - Captcha was appeared.");
                    m_status = ConstEnv.STATE_KILL_THREAD;
                    return false;
                }
                if (m_must_terminate)
                {
                    return false;
                }

                string domain = mail.Substring(mail.IndexOf("@") + 1).Replace(" ", "").Split('.')[0];

                string no_1 = "No results found for";
                string no_2 = "did not match any documents.";

                if (Driver.FindElement(By.TagName("body")).Text.IndexOf(no_1) != -1 || Driver.FindElement(By.TagName("body")).Text.IndexOf(no_2) != -1)
                {
                    public_url[0] = "";
                    m_status = ConstEnv.STATE_SUCCESS;
                    m_is_checked = true;
                    return true;
                }

                //bool test_flag = false;
                while (true)
                {
                    string xpath_1 = "//div[@class='r']";
                    string xpath_2 = "//div[@class='ad_cclk']";
                    string xpath_0 = "";

                    if (await WaitToPresentByPath(xpath_1))
                        xpath_0 = xpath_1;
                    else if (await WaitToPresentByPath(xpath_2))
                        xpath_0 = xpath_2;

                    if (xpath_0 == "")
                        return false;

                    m_status = ConstEnv.STATE_SUCCESS;

                    foreach (IWebElement url_tag in Driver.FindElementsByXPath(xpath_0))
                    {
                        /*if (test_flag == false)
                        {
                            public_url[0] = "";
                            test_flag = true;
                        }*/
                        string temp = url_tag.FindElements(By.TagName("a"))[0].GetAttribute("href");

                        if (get_domain(temp) == null)
                            continue;
                        if (get_domain(temp).Contains(domain, StringComparer.InvariantCultureIgnoreCase))
                            continue;

                        if (m_must_terminate)
                        {
                            return false;
                        }

                        if (public_url[0] == "")
                        {
                            public_url[0] = temp;
                            continue;
                        }

                        if (check_domain_same(temp, public_url[0]) == false && public_url[1] == "")
                        {
                            public_url[1] = temp;
                            break;
                        }
                    }
                    if (public_url[0] != "" && public_url[1] != "")
                        break;

                    try
                    {
                        string next_url = Driver.FindElementByXPath("//a[@id='pnnext']").GetAttribute("href");
                        await Navigate(next_url);
                        await TaskDelay(2000);
                    }
                    catch(Exception ex)
                    {
                        break;
                    }

                    if (m_must_terminate)
                    {
                        return false;
                    }
                }

                m_is_checked = true;

                await Task.Delay(2000);
                
                return true;
            }
            catch (Exception ex)
            {
                MainApp.log_info(string.Format("Catch Exception: {0}", ex.Message));
                return false;
            }
        }
    
        public bool check_domain_same(string url, string url1)
        {
            List<string> lst_domain1 = get_domain(url);
            List<string> lst_domain2 = get_domain(url1);

            if (lst_domain1 == null || lst_domain2 == null || lst_domain1 == lst_domain2 || lst_domain1[0] == lst_domain2[0])
                return true;
            try
            {
                if (lst_domain1[0] == lst_domain2[1])
                    return true;
            }
            catch(Exception ex) { }

            try
            {
                if (lst_domain1[1] == lst_domain2[0])
                    return true;
            }
            catch (Exception ex) { }

            return false;
        }

        public List<string> get_domain(string url)
        {
            List<string> domain_list= new List<string>();

            string temp = "";
            string domain = "";
            string[] arr_domain = null;
            url = url.Replace(" ", "");

            try
            {
                if (url.IndexOf("https://www.") != -1)
                    temp = "https://www.";
                else if (url.IndexOf("http://www.") != -1)
                    temp = "http://www.";
                else if (url.IndexOf("http://") != -1)
                    temp = "http://";
                else if (url.IndexOf("https://") != -1)
                    temp = "https://";
                else if (url.IndexOf("www.") != -1)
                    temp = "www.";

                if (temp != "")
                {
                    domain = url.Substring(url.IndexOf(temp) + temp.Length);
                    if (url.IndexOf("/") != -1)
                        domain = domain.Substring(0, domain.IndexOf("/"));
                    arr_domain = domain.Split('.');
                }
                else
                {
                    if (url.IndexOf("/") != -1)
                        domain = url.Substring(0, url.IndexOf("/"));
                    arr_domain = url.Split('.');
                }
                domain_list = arr_domain.ToList();
                return domain_list;
            }
            catch(Exception ex)
            {
                MainApp.log_error($"Error catched in get_domain - {ex.Message}");
            }
            return null;
        }

        
    }
}
