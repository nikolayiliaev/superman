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
    class Adsupply
    {
        private string login_url;
        private string folder_path;
        private string file_path;

        public Adsupply(string path)
        {
            this.login_url = "http://ui.adsupply.com/Account/LogIn";
            this.folder_path = path + "\\Adsupply";
            this.file_path = path + "\\Adsupply\\Adsupply_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            DateTime today = DateTime.Now;
            DateTime month = new DateTime(today.Year, today.Month, 1);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int lastMonth = month.AddMonths(-1).Month;
            int yearOfLastMonth = month.AddMonths(-1).Year;
            int lastDayOfLastMonth = month.AddDays(-1).Day;

            try
            {
                login(driver, report.login_user, report.login_pass);
                if (Func.check_login(driver, login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    driver.Navigate().GoToUrl("http://ui.adsupply.com/PublicPortal/Publisher/259/Report?SqlCommandId=&ExportToExcel=False&DateFilter=" + lastMonth + "%2F1%2F" + yearOfLastMonth + "+-+" + lastMonth +"%2F" + lastDayOfLastMonth + "%2F" + yearOfLastMonth + "&TimeZoneId=UTC&Grouping=3&DimPublisher.Value=259~Ad-Maven+Ltd.&DimPublisher.IsActive=True&DimSiteName.Value=&DimSiteName.IsActive=True&DimCountry.Value=&DimCountry.IsActive=False&DimMediaType.Value=&DimMediaType.IsActive=False&DimPublisherSubId.IsActive=False");

                    Thread.Sleep(5000);
                    int document_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.scrollHeight").ToString());
                    int client_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.clientHeight").ToString());
                    //if you need to scroll down the page and take enother screenshot write + 1  to number_of_screen_shote
                    int number_of_screen_shot = (document_height / client_height);
                    for (int i = 0; i < number_of_screen_shot; i++)
                    {
                        Func.take_screenshot(report.campaign_name, folder_path, i + 1);
                        js.ExecuteScript("window.scrollTo(0 ," + client_height + ")");
                        Thread.Sleep(2000);
                    }
                  
                    Func.clear_local_storage(driver);
                    driver.Navigate().GoToUrl("http://ui.adsupply.com/Account/LogOut");
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
                driver.Navigate().GoToUrl("http://ui.adsupply.com/Account/LogOut");
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                Func.clear_local_storage(driver);
                tw.Close();
            }

        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.Id("email"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Id("password"))).SendKeys(password);
            wait_login.Until(pass => pass.FindElement(By.Id("password"))).Submit();
        }
    }
}
