using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Report_screen_shots.advertiser
{
    class Revenue_Hits
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public Revenue_Hits(string path)
        {
            this.login_url = "http://www.revenuehits.com/publishers/login.jsf";
            this.folder_path = path + "\\Revenue_Hits";
            this.file_path = path + "\\Revenue_Hits\\Revenue_Hits_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            try
            {
                login(driver, report.login_user, report.login_pass);
                if (Func.check_login(driver, login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(0.5));
                    wait.Until(payments_tab => payments_tab.FindElement(By.Id("paytab"))).FindElement(By.TagName("a")).Click();
                    wait.Until(report_show => report_show.FindElement(By.Id("money")));
                    wait.Until(last_month => last_month.FindElement(By.XPath(".//*[@id='mainForm:j_idt1125']"))).Click();
                    Thread.Sleep(1500);
                    Func.take_screenshot(report.campaign_name, folder_path);
                    Func.clear_local_storage(driver);
                    tw.Close();
                }
                else
                {
                    Func.clear_local_storage(driver);
                    tw.WriteLine(report.campaign_name + " cannot login");
                    tw.Close();
                }
            }
            catch(Exception e)
            {
                Func.clear_local_storage(driver);
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                tw.Close();
            }
        }

        public void clear_fields(IWebElement user , IWebElement password)
        {
            user.Clear();
            password.Clear();
        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            IWebElement user = wait_login.Until(username => username.FindElement(By.Id("loginForm:email")));
            IWebElement pass = wait_login.Until(Pass => Pass.FindElement(By.Id("loginForm:inputPassword")));
            clear_fields(user, pass);
            user.SendKeys(user_name);
            pass.SendKeys(password);
            Thread.Sleep(1500);
            wait_login.Until(sign_in => sign_in.FindElement(By.Id("loginForm:loginBtn"))).Click();
        }
    }
}
