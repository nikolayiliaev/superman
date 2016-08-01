using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Report_screen_shots
{
    class AdsTerra
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public AdsTerra(string path)
        {
            this.login_url = "http://publishers.adsterra.com";
            this.folder_path = path + "\\AdsTerra";
            this.file_path = path + "\\AdsTerra\\AdsTerra_log.txt";
        }
        public void Get_reports_screen_shots(IWebDriver driver, Report_data report,string from,string to)
        {
            if (!File.Exists(folder_path + "\\" + report.campaign_name + ".png"))
            {
                Func.check_if_folder_exists(folder_path);
                TextWriter tw = Func.check_if_log_file_exists(file_path);
                try
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    driver.Navigate().GoToUrl(login_url);
                    js.ExecuteScript("document.querySelector(\"input[type='text']\").value=\""+report.login_user+"\"");
                    js.ExecuteScript("document.querySelector(\"input[type='password']\").value=\"" + report.login_pass + "\"");
                    js.ExecuteScript("document.querySelector(\"input[type='submit']\").click()");
                    Thread.Sleep(5000);
                    driver.Navigate().GoToUrl("http://publishers.adsterra.com/stats/");

                    string from_slash = from.Replace(@"-", "/");
                    string to_slash = to.Replace(@"-", "/");

                    IWebElement start = driver.FindElement(By.CssSelector("body > div:nth-child(1) > div.page.statistics > form > span:nth-child(6) > span > input[type='text']"));
                    js.ExecuteScript("$(arguments[0]).prop('readonly',false)",start);
                    js.ExecuteScript("arguments[0].value = arguments[1];", start, from_slash);
                    IWebElement finish = driver.FindElement(By.CssSelector("body > div:nth-child(1) > div.page.statistics > form > span:nth-child(7) > span > input[type='text']"));
                    js.ExecuteScript("$(arguments[0]).prop('readonly',false)", finish);
                    js.ExecuteScript("arguments[0].value = arguments[1];", finish, to_slash);
                    Thread.Sleep(3500);

                    driver.FindElement(By.XPath("/html/body/div[1]/div[2]/form/span[6]/span/input[2]")).SendKeys(Keys.ArrowDown + Keys.Enter);
                    driver.FindElement(By.XPath("html/body/div[1]/div[2]/form/span[8]/input")).Click();
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
                    driver.Navigate().GoToUrl("http://publishers.adsterra.com/");
                    tw.Close();
                }
                catch (Exception e)
                {
                    Func.clear_local_storage(driver);
                    tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                    tw.Close();
                }
            }

        }


    }
}
