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
    class Ad6media
    {
        public string login_url;
        public string folder_path;
        public string file_path;

        public Ad6media(string path)
        {
            this.login_url = "http://www.ad6media.co.uk/";
            this.folder_path = path + "\\Ad6media";
            this.file_path = path + "\\Ad6media\\Ad6media_log.txt";
        }
        public void Get_reports_screen_shots(IWebDriver driver, Report_data report)
        { 
         if (!File.Exists(folder_path+"\\"+report.campaign_name+".png"))
            {
                Func.check_if_folder_exists(folder_path);
                TextWriter tw = Func.check_if_log_file_exists(file_path);
                try
                {
                    var wait_login = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    driver.Navigate().GoToUrl(login_url);
                    wait_login.Until(user_name => user_name.FindElement(By.Name("user_name"))).SendKeys(report.login_user);
                    wait_login.Until(password => password.FindElement(By.Name("user_pass"))).SendKeys(report.login_pass);
                    Thread.Sleep(1500);
                    wait_login.Until(sign_in => sign_in.FindElement(By.Name("submit"))).Click();
                    if (check_login_A6Media(driver, login_url))
                    {
                        var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(3));
                        driver.Navigate().GoToUrl("http://www.ad6media.co.uk/publishers/statistics");
                        //wait.Until(date_per_month => date_per_month.FindElement(By.Id("type_rapport"))).FindElement(By.XPath([@id="type_rapport"]/option[2]))
                        var selectElement = new SelectElement(driver.FindElement(By.Id("type_rapport")));
                        selectElement.SelectByText("Per month");
                        ChooseCorrectCampaignUI(driver, report);
                        Thread.Sleep(5000);
                        driver.Navigate().GoToUrl("http://www.ad6media.co.uk/deconnexion");
                        tw.Close();

                    }

                }
                catch (Exception e)
                {
                    Func.clear_local_storage(driver);
                    tw.WriteLine(report.campaign_name + " Exception while screen shot try again");
                    tw.Close();
                }
            }
            
        }

        public void ChooseCorrectCampaignUI(IWebDriver driver, Report_data report)
        {
            switch (report.campaign_name)
            {
                case "Ad6Media_Pop_D_FR_4":
                    {
                        var selectElement = new SelectElement(driver.FindElement(By.Id("site")));
                        selectElement.SelectByText("ad-maven FR 4");
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        break;
                    }
                case "Ad6Media_Pop_D_FR_5":
                    {
                        var selectElement = new SelectElement(driver.FindElement(By.Id("site")));
                        selectElement.SelectByText("ad-maven. FR 5");
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        break;
                    }
                case "Ad6Media_Pop_D_BE_3":
                    {
                        var selectElement = new SelectElement(driver.FindElement(By.Id("site")));
                        selectElement.SelectByText("ad-maven BE 3");
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        break;
                    }
                case "Ad6Media_Pop_D_BE_4":
                    {
                        var selectElement = new SelectElement(driver.FindElement(By.Id("site")));
                        selectElement.SelectByText("ad-maven BE 4");
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        break;
                    }
                case "Ad6Media_Pop_D_IT":
                    {
                        var selectElement = new SelectElement(driver.FindElement(By.Id("site")));
                        selectElement.SelectByText("ad-maven Italy 1");
                        Func.take_screenshot(report.campaign_name, folder_path);
                        Func.clear_local_storage(driver);
                        break;
                    }
                default:
                    {
                    break;
                    }

            }


        }

        public bool check_login_A6Media(IWebDriver driver, string url)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            try
            {
                wait.Until(check_login => check_login.FindElement(By.Id("menuC")));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}   