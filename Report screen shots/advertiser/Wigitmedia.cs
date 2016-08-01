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
    class Wigitmedia
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public Wigitmedia(string path)
        {
            this.login_url = "https://panel.wigetmedia.com/Sign-In";
            this.folder_path = path + "\\Wigitmedia";
            this.file_path = path + "\\Wigitmedia\\Wigitmedia_log.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                driver.Navigate().GoToUrl(login_url);
                wait_login.Until(user_name => user_name.FindElement(By.Id("signin_username"))).SendKeys(report.login_user);
                wait_login.Until(password => password.FindElement(By.Name("signin_password"))).SendKeys(report.login_pass);
                wait_login.Until(sign_in_button => sign_in_button.FindElement(By.ClassName("sign-in"))).FindElement(By.TagName("input")).Submit();
                if(Func.check_login(driver , login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    wait.Until(report_tab => report_tab.FindElement(By.Id("side_reports"))).Click();
                    IList<IWebElement> search_box = wait.Until(group_by => group_by.FindElements(By.ClassName("search-field")));
                    search_box[0].FindElement(By.TagName("input")).Click();
                    IList<IWebElement> chosen_results = wait.Until(group_by_option => group_by_option.FindElements(By.ClassName("chosen-results")));
                    IList<IWebElement> options = chosen_results[0].FindElements(By.TagName("li"));
                    options[2].Click();
                    driver.FindElement(By.Id("publisher")).FindElement(By.TagName("input")).Click();
                    IWebElement date_range = driver.FindElement(By.Id("date_range_chosen"));
                    date_range.Click();
                    IList<IWebElement> date_range_options = date_range.FindElements(By.TagName("li"));
                    date_range_options[6].Click();
                    driver.FindElement(By.CssSelector("#form-quickreport > div > input:nth-child(1)")).Click();
                    wait.Until(report_ui => report_ui.FindElement(By.Id("queue")));
                    Thread.Sleep(1000);
                    js.ExecuteScript("window.scrollTo(document.querySelectorAll('.container .tab-container[data-tab=reports] .col-right')[0].scrollTop += 1000 , 0)");
                    Thread.Sleep(500);
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
    }
}
