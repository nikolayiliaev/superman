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
    class Velis
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public Velis(string path)
        {
            this.login_url = "http://velismedia.convertmedia.com/dashboard#dashboard";
            this.folder_path = path + "\\Velis";
            this.file_path = path + "\\Velis\\Velis_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
           
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            try
            {
                var wait_login = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                driver.Navigate().GoToUrl(login_url);
                wait_login.Until(user_name => user_name.FindElement(By.Id("memberId"))).SendKeys(report.login_user);
                wait_login.Until(password => password.FindElement(By.Name("password"))).SendKeys(report.login_pass);
                Thread.Sleep(1500);
                wait_login.Until(sign_in_button => sign_in_button.FindElement(By.Id("login"))).Click();
                if (check_login_velis(driver, login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    wait.Until(date_range => date_range.FindElement(By.Id("ctrlAnalytics_inptDateRange"))).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.ClassName("ui-daterangepicker-ThepreviousMonth")).Click();
                    Thread.Sleep(1500);
                    driver.FindElement(By.XPath(".//*[@id='ctrlAnalytics_rdoGraphTableAnalytics']/label[2]/span")).Click();
                    Thread.Sleep(1000);
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

        public bool check_login_velis(IWebDriver driver, string url)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                wait.Until(check_login => check_login.FindElement(By.ClassName("controlBox")));
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
