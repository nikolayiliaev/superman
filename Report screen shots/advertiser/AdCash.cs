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
    class AdCash
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public AdCash(string path)
        {
            this.login_url = "https://www.adcash.com/en/signin.php?url=%2Fconsole%2F";
            this.folder_path = path + "\\AdCash";
            this.file_path = path + "\\AdCash\\AdCash.txt";
        }

        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            Func.check_if_folder_exists(folder_path);
            TextWriter tw = Func.check_if_log_file_exists(file_path);
            try
            {
                login(driver , report.login_user , report.login_pass);     
                if (Func.check_login(driver , login_url))
                {
                    var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                    IList<IWebElement> bar_options = wait.Until(bar => bar.FindElement(By.Id("cssmenu"))).FindElements(By.TagName("a"));
                    bar_options.Cast<IWebElement>().SingleOrDefault(a => a.Text == "Statistics").Click();                    
                    Thread.Sleep(500);
                    var select = new SelectElement(wait.Until(select_element => select_element.FindElement(By.Id("change_date"))));
                    select.SelectByText("Previous Month");
                    driver.FindElement(By.Id("report_filter_submit")).Click();
                    Thread.Sleep(500);
                    int document_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.scrollHeight").ToString());
                    int client_height = Convert.ToInt32((string)js.ExecuteScript("return document.body.clientHeight").ToString());
                    int number_of_screen_shot = (document_height / client_height) + 1;
                    for (int i = 0; i < number_of_screen_shot; i++)
                    {                        
                        Func.take_screenshot(report.campaign_name, folder_path , i + 1);
                        js.ExecuteScript("window.scrollTo(0 ," + client_height + ")");
                        Thread.Sleep(2000);
                    }
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

        public void login(IWebDriver driver , string user_name , string password)
        {
            var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            driver.Navigate().GoToUrl(login_url);
            wait_login.Until(user => user.FindElement(By.Name("login"))).SendKeys(user_name);
            wait_login.Until(pass => pass.FindElement(By.Name("password"))).SendKeys(password);
            wait_login.Until(sign_in_button => sign_in_button.FindElement(By.CssSelector(".field .ui.button"))).Click();
        }
    }
}
