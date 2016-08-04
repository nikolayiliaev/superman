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
    class MarimediaEpom
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public MarimediaEpom(string path, string url, string advertiser)
        {
            Func.check_if_folder_exists(path + "\\Epom");
            this.folder_path = path + "\\Epom\\" + advertiser;
            this.file_path = path + "\\Epom\\" + advertiser + "\\" + advertiser + ".txt";
            this.login_url = url;
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            try
            {
                login(driver, "Webpic Support", "d09f8fc0");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
                Thread.Sleep(5000);
                string url = login_url + "/account/home.do#|analytics";
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("this.document.location = arguments[0]", url);
                wait.Until(element => element.FindElement(By.Id("analytics-range-inputEl"))).SendKeys(Keys.ArrowDown + Keys.ArrowDown + Keys.ArrowDown + Keys.ArrowDown +
                    Keys.ArrowDown + Keys.ArrowDown + Keys.ArrowDown + Keys.Enter);
                wait.Until(element => element.FindElement(By.Id("analytics-groupBy-inputEl"))).SendKeys(Keys.ArrowDown + Keys.ArrowDown + Keys.ArrowDown + Keys.Enter);
                Thread.Sleep(2000);
                wait.Until(element => element.FindElement(By.LinkText("Run Report"))).Click();
                Thread.Sleep(5000);
                wait.Until(element => element.FindElement(By.XPath("//*[contains(text(), 'Query details')]"))).Click();
                Thread.Sleep(2000);
                Func.take_screenshot(report.campaign_name, folder_path);
                Func.clear_local_storage(driver);
                js.ExecuteScript("document.getElementById('logout-form').submit();");
                tw.Close();
            }

            catch (Exception e)
            {
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
            driver.FindElement(By.XPath("//*[@id='loginForm']/button")).Submit();
            Thread.Sleep(3000);
        }
    }
}
