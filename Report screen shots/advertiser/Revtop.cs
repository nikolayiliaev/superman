using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Report_screen_shots.advertiser
{
    class Revtop
    {
        private string login_url;
        private string folder_path;
        private string file_path;

        public Revtop(string path)
        {
            this.login_url = "http://pub.revtop.com";
            this.folder_path = path + "\\Revtop";
            this.file_path = path + "\\Revtop\\Revtop_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            DateTime today = DateTime.Now;
            DateTime month = new DateTime(today.Year, today.Month, 1);
            int lastMonth = month.AddMonths(-1).Month;
            int yearOfLastMonth = month.AddMonths(-1).Year;
            int lastDayOfLastMonth = month.AddDays(-1).Day;
            string FirstdayOfMonthToUser = "01";
            string lastMonthString;

            if (lastMonth <= 9)
            {
                lastMonthString = "0" + lastMonth.ToString();
            }
            else
            {
                lastMonthString = lastMonth.ToString();
            }

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            try
            {
                login(driver, report.login_user, report.login_pass);
                if (Func.check_login(driver, login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    driver.Navigate().GoToUrl("http://pub.revtop.com/#/statistics");
                    Thread.Sleep(5000);
                    wait.Until(element => element.FindElement(By.ClassName("advanced-datepicker"))).Click();
                    IList<IWebElement> allDatesOption = wait.Until(element => element.FindElements(By.ClassName("ranges"))[1]).FindElement(By.TagName("ul")).FindElements(By.TagName("li"));
                    allDatesOption[5].Click();
                    Thread.Sleep(10000);
                    IList<IWebElement> allSitesOption = wait.Until(element => element.FindElement(By.ClassName("wrapper-label"))).FindElements(By.TagName("a"));
                    allSitesOption[2].Click();
                    Thread.Sleep(10000);
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
                    driver.Navigate().GoToUrl("http://pub.revtop.com/#/login");
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
                driver.Navigate().GoToUrl("http://pub.revtop.com/#/login");
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                Func.clear_local_storage(driver);
                tw.Close();
            }

        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.Name("uname"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Name("upass"))).SendKeys(password);
            wait_login.Until(submit => submit.FindElement(By.Name("loginForm"))).Submit();
            Thread.Sleep(5000);
        }
    }
}
