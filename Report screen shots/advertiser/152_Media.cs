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
    class _152_Media
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public _152_Media(string path, string url, string advertiser)
        {
            Func.check_if_folder_exists(path + "\\Ply2c");
            this.folder_path = path + "\\Ply2c\\" + advertiser;
            this.file_path = path + "\\Ply2c\\" + advertiser + "\\" + advertiser + ".txt";
            this.login_url = url;
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
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    int camp_id = get_campaign_id(report.campaign_name);
                    if (camp_id != -1)
                    {
                        Thread.Sleep(5000);
                        string url = login_url + "/publisher#/" + camp_id + "/reports/Appearances/Last%20Month";
                        driver.Navigate().GoToUrl(url);
                        wait.Until(report_table => report_table.FindElement(By.CssSelector("div[id*='gridpanel']")));
                        Thread.Sleep(1500);
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        tw.Close();
                    }
                    else
                    {
                        tw.WriteLine(report.campaign_name + " campaign name not include the id in the end");
                        Func.clear_local_storage(driver);
                        tw.Close();
                    }
                }
                else
                {
                    tw.WriteLine(report.campaign_name + " cannot login");
                    Func.clear_local_storage(driver);
                    tw.Close();
                }
            }
            catch (Exception e)
            {
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                Func.clear_local_storage(driver);
                tw.Close();
            }
        }

        public int get_campaign_id(string campaign_name)
        {
            string camp = campaign_name.Split('_').Last();
            int camp_id;
            bool result = Int32.TryParse(camp, out camp_id);
            if (result)
                return camp_id;
            else return -1;
        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.ClassName("email"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Id("passwordField"))).SendKeys(password);
            Thread.Sleep(1500);
            driver.FindElement(By.ClassName("adk2-login-button")).Click();
        }
    }
}
