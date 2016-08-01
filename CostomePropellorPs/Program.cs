using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Report_screen_shots.advertiser;
using System.Globalization;

namespace Report_screen_shots
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Queries queries = new Queries();
            DB_connect db = new DB_connect("dbase.web-pick.com", "3306", "bizdev_new", "Bizdev1234", "dbase");
            List<string> folders_name = new List<string>() { "\\Propeller"};
            List<string> all_queries = queries.get_all_queries(); 
            List<Report_data> all_reports = new List<Report_data>();
            IWebDriver driver = get_browser();
           
            for(int i = 0; i < all_queries.Count; i++)
            {
                all_reports = db.get_reports(all_queries[i]);
               
                if (all_reports != null)
                {
                    Directory.CreateDirectory(path + folders_name[i]);
                    TextWriter tw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folders_name[i] +"\\app_log.txt");
                    foreach (Report_data report in all_reports)
                    {
                                Propellerads propellerads = new Propellerads(path + folders_name[i]);
                                propellerads.Get_reports_screen_shots(driver, report);
                                break;
                    }
                }
                else
                    Console.WriteLine("data not load");
            }
            driver.Quit();
        }

        public static IWebDriver get_browser()
        {
            try
            {
                var path = System.AppDomain.CurrentDomain.BaseDirectory;
                IWebDriver driver = new ChromeDriver(/*@"C:\chrome driver"*/path);
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                driver.Manage().Window.Maximize();
                return driver;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        
        
    }
}
