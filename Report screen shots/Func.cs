using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Report_screen_shots
{
    static class Func
    {
        public static void clear_local_storage(IWebDriver driver)
        {
            Thread.Sleep(2000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("localStorage.clear()");
            js.ExecuteScript("sessionStorage.clear()");
            driver.Manage().Cookies.DeleteAllCookies();
            Thread.Sleep(3000);
        }

        public static void take_screenshot(String campaignName , string folderPath , int num = 0)
        {
            ScreenCapture screen = new ScreenCapture();
            Image image = screen.CaptureScreen();
            string temp_name = campaignName.Replace(@"/", "");
            if (num == 0)
            {
                image.Save(folderPath + "\\" + temp_name + ".png", ImageFormat.Jpeg);
            }
            else
                image.Save(folderPath + "\\" + temp_name + "_" + num + ".png", ImageFormat.Jpeg);
        }

        public static void check_if_folder_exists(string path)
        {
            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
                System.IO.Directory.CreateDirectory(path);
        }

        public static TextWriter check_if_log_file_exists(string path)
        {
            return new StreamWriter(path, true);
        }

        public static bool check_login(IWebDriver driver, string url)
       {
            for (int i = 0; i <= 2; i++)
            {
                if (driver.Url.Equals(url))
                {
                    i++;
                    Thread.Sleep(5000);
                }
                else
                    return true;
            }
            if (driver.Url.Equals(url))
                return false;
            else
                return true;
        }

        public static bool check_login_ply2c(IWebDriver driver, string url)
        {
            try
            {
                driver.FindElement(By.XPath(".//*[@id='component-1024']"));
                return true;
            }

            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
