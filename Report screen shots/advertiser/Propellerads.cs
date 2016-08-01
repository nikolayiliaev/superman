using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.IO;
using System.Threading;
using OpenQA.Selenium.Support.UI;

namespace Report_screen_shots.advertiser
{
    class Propellerads
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public Propellerads(string path)
        {
            this.login_url = "http://v2.propellerads.com/#/auth/";
            this.folder_path = path + "\\Propellerads";
            this.file_path = path + "\\Propellerads\\Propellerads_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver , Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);

            try
            {
                login(driver, report.login_user, report.login_pass);
                if (Func.check_login(driver , login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    wait.Until(element => element.FindElement(By.ClassName("sidenav"))).FindElement(By.CssSelector("a[ui-sref='admin.stats']")).Click();
                    wait.Until(element => element.FindElement(By.ClassName("pa-chart__overlay")));
                    wait.Until(element => element.FindElement(By.ClassName("stats-result__header"))).FindElement(By.CssSelector("div[ng-click='t.tabId = 1']")).Click();
                    wait.Until(element => element.FindElement(By.ClassName("query__interval"))).Click();
                    IList<IWebElement> all_dates = wait.Until(element => element.FindElement(By.ClassName("pa-interval__defaults"))).FindElements(By.TagName("div"));
                    all_dates[5].Click();
                    var selectElement = new SelectElement(driver.FindElement(By.ClassName("query__group-by__value")));
                    selectElement.SelectByText("Site");
                    driver.FindElement(By.ClassName("query__toggle")).Click();
                    Thread.Sleep(10000);
                    Func.take_screenshot(report.campaign_name , folder_path);
                    Func.clear_local_storage(driver);
                    driver.FindElement(By.ClassName("header__logout")).Click();
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
                driver.FindElement(By.ClassName("header__logout")).Click();
                tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                Func.clear_local_storage(driver);
                tw.Close();
            }
        }

        public void login(IWebDriver driver, string user_name, string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.Name("username"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Name("password"))).SendKeys(password);
            wait_login.Until(sign_in_button => sign_in_button.FindElement(By.ClassName("auth-form__footer"))).FindElement(By.TagName("button")).Click();
        }
    }
}
