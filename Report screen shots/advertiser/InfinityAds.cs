using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Report_screen_shots.advertiser
{
    class InfinityAds
    {
         private string login_url;
        private string folder_path;
        private string file_path;

        public InfinityAds(string path)
        {
            this.login_url = "http://member.yesadvertising.com";
            this.folder_path = path + "\\InfinityAds";
            this.file_path = path + "\\InfinityAds\\InfinityAds_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            DateTime today = DateTime.Now;
            DateTime lastMonthFirstDay = new DateTime(today.Year, today.Month, 1);
            lastMonthFirstDay = lastMonthFirstDay.AddMonths(-1);
            DateTime lastMonthLastDay = new DateTime(today.Year, today.Month, 1);
            lastMonthLastDay = lastMonthLastDay.AddDays(-1);

            try
            {
                login(driver, report.login_user, report.login_pass);
                if (Func.check_login(driver, login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    driver.Navigate().GoToUrl("http://member.yesadvertising.com/publisher/report/traffic?dateFrom=" + lastMonthFirstDay.GetDateTimeFormats()[5] + "&dateTo=" + lastMonthLastDay.GetDateTimeFormats()[5] + "&site_id=&zone_id=&group_by=zone_id&sub_group_by=site_id&display_cpx=0&items_per_page=10");

                    Thread.Sleep(5000);
                    int document_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.scrollHeight").ToString());
                    int client_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.clientHeight").ToString());
                    int number_of_screen_shot = (document_height / client_height) + 1;
                    for (int i = 0; i < number_of_screen_shot; i++)
                    {
                        Func.take_screenshot(report.campaign_name, folder_path, i + 1);
                        js.ExecuteScript("window.scrollTo(0 ," + client_height + ")");
                        Thread.Sleep(2000);
                    }
                    Func.take_screenshot(report.campaign_name, folder_path);
                    Func.clear_local_storage(driver);
                    driver.Navigate().GoToUrl("http://member.yesadvertising.com/member/auth/login");
                    tw.Close();

                }
                else
                {
                    Func.clear_local_storage(driver);
                    tw.WriteLine(report.campaign_name + " cannot login");
                    tw.Close();
                }
            }
            catch (Exception e)
            {
                driver.Navigate().GoToUrl("http://member.yesadvertising.com/member/auth/login");
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                Func.clear_local_storage(driver);
                tw.Close();
            }

        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.Id("username"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Id("password"))).SendKeys(password);
            wait_login.Until(pass => pass.FindElement(By.Id("password"))).Submit();
        }
    }
}
