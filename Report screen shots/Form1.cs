using System.Windows.Forms;
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
using System.Diagnostics;

namespace Report_screen_shots
{   
    public partial class Form1 : Form
    {
        //
        // Attributes
        //
        private IWebDriver driver;
        private Queries queries;
        private DB_connect db = new DB_connect("rs-hot.web-pick.com", "5439", "admin", "<Tfknktsyv123!!", "dw");
        private List<string> adv_list = new List<string>();
        // Flag attribute to avoid screenshot duplications
        private int pubDirecteSameUser = 0;
        private int adsupplySameUser = 0;
        private int revtopSameUser = 0;
        private int exoclickSameUser = 0;
        private int infinitySameUser = 0;
        private int popunderSameUser = 0;
        private int adSterraSameUser = 0;

        // A Key-value dict for adcash only
        private Dictionary<String, Report_data> adcash_users_no_dups = new Dictionary<String, Report_data>();
        // A Key-value dict for adsupply only
        private Dictionary<String, Report_data> adsupply_users_no_dups = new Dictionary<String, Report_data>();
        private string from;
        private string to;
        private List<string> dates = new List<string>();
        private string[] month_formatted;

        //
        // Constructor
        //
        public Form1()
        {

            InitializeComponent();
            this.Text = "Reports Screenshots";
            this.loadList();

        }

        //
        // Methods
        //
        private void run_btn_Click(object sender, EventArgs e)
        {
            try
            {
                from = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                to = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                dates.Add(from);
                dates.Add(to);
                month_formatted = from.Split('-');
                string monthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Int32.Parse(month_formatted[1]));
                string advertiser = buildQuery();
                queries = new Queries(dates, advertiser);
                string path = null;
                if (textBox1.Text != null)
                {
                    path = textBox1.Text + @"\LatestEOM " + monthName;
                    System.IO.Directory.CreateDirectory(path);
                }
                else
                {
                    throw new Exception("Please choose a path to save the reports");
                }

                List<string> all_queries = new List<string>();
                
                List<string> folders_name = createFolders(all_queries);
                List<Report_data> all_reports = new List<Report_data>();
                driver = get_browser();
                for (int i = 0; i < all_queries.Count; i++)
                {
                    all_reports = db.get_reports(all_queries[i]);
                    if (all_reports != null)
                    {

                        Directory.CreateDirectory(path + folders_name[i]);
                        TextWriter tw = new StreamWriter(path + folders_name[i] + "\\app_log.txt");
                        TextWriter tew = new StreamWriter(path + folders_name[i] + "\\all_reports.txt");
                        // Writes all the db reports to txt file for tracking
                        foreach (Report_data report in all_reports)
                        {
                            if (adv_list.Contains(report.advertiser_name)) 
                                tew.Write(report.advertiser_name + "\t" + report.campaign_name + "\t" +
                                    report.connection_url + "\t" + report.login_user + "\t" + report.login_pass + Environment.NewLine);
                            
                        }
                        tew.Close();
                        foreach (Report_data report in all_reports)
                        {
                            progressBar.PerformStep();
                            int percent = (int)(((double)progressBar.Value / (double)progressBar.Maximum) * 100);
                            progressBar.CreateGraphics().DrawString(percent.ToString() + "%", new Font("Arial", (float)8.25, FontStyle.Regular), Brushes.Black, new PointF(progressBar.Width / 2 - 10, progressBar.Height / 2 - 7));
                            generate_screenshot(report, path, folders_name[i]);
                        }
                    }
                    else
                        throw new Exception("Error getting reports.");
                       
                }
                MessageBox.Show(new Form { TopMost = true }, "Finished Succefully");
                this.Close();
                driver.Quit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static IWebDriver get_browser()
        {
            try
            {
                IWebDriver driver = new ChromeDriver();
                driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(30));
                driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(30));
                driver.Manage().Window.Maximize();
                return driver;
            }
            catch (Exception e)
            {
                throw new Exception("Browser isnt avaliable");
            }
        }
        private void loadList()
        {
            List<string> advertisers = new List<string>();
            advertisers.Add("ALL");
            advertisers.Add("propellerads");
            advertisers.Add("wigitmedia");
            advertisers.Add("AdCash");
            advertisers.Add("Revenue_hits");
            advertisers.Add("Velis");
            advertisers.Add("Ad6Media");
            advertisers.Add("AdsTerra");
            advertisers.Add("Adsupply");
            advertisers.Add("Pubdirecte");
            advertisers.Add("Revtop");
            advertisers.Add("InfinityAds");
            advertisers.Add("Exoclick");
            advertisers.Add("Popunder");
            advertisers.Add(" ");
            advertisers.Add("Epom");
            advertisers.Add("Adland");
            advertisers.Add("mari_media Epom");
            advertisers.Add("Saludo");
            advertisers.Add("xertive");
            advertisers.Add("snw Epom");
            advertisers.Add(" ");
            advertisers.Add("Ply2C");
            advertisers.Add("152 Media");
            advertisers.Add("ad_maven");
            advertisers.Add("Adplex");
            advertisers.Add("Adstract");
            advertisers.Add("Dmg_Dsnr");
            advertisers.Add("mari_media");
            advertisers.Add("Velis Ply2C");
            advertisers.Add("snw");
            advertisers.Add("Matomy");
            advertisers.Add("Mango");
            advertisers.Add("HarrenMedia");
            advertisers.Add("BabaNetwork");
            advertisers.Add("");
            advertisers.Add("");
            advertisers.Add("Yaniv");
            adv_list = advertisers;
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("Advertiser Name", 100);
            listView1.Columns[0].Width = this.listView1.Width - 4;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            ListViewItem itm;
            foreach (string adv in advertisers)
            {
                itm = new ListViewItem(adv);
                listView1.Items.Add(itm);
            }

        }
        private void browse_btn_Click(object sender, EventArgs e)
        {
            string folderName = null;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }

        }
        public string buildQuery()
        {

            string advertiser = null;
            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem adv = listView1.SelectedItems[0];
                advertiser = "'" + adv.Text + "'";
            }
            if (listView1.SelectedItems.Count == 0)
            {
                throw new Exception("Please choose an advertiser.");
            }
            if (listView1.SelectedItems.Count > 1)
            {
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (listView1.SelectedItems[listView1.SelectedItems.Count - 1] == item)
                    {
                        advertiser += "'" + item.Text + "'";
                    }
                    else
                    {
                        advertiser += "'" + item.Text + "' or m.advertiser_name = ";
                    }

                }
            }
            return advertiser;
        }
        public List<string> createFolders(List<string> all_queries)
        {
            List<string> folders_name = new List<string>();
            if (checkBox_banner.Checked)
            {
                all_queries.Add(queries.get_banner());
                folders_name.Add("\\banner");
            }
            if (checkBox_lightbox.Checked)
            {
                all_queries.Add(queries.get_lightbox());
                folders_name.Add("\\lightbox direct old pop");
            }
            if (checkBox_newpop.Checked)
            {
                all_queries.Add(queries.get_new_pop());
                folders_name.Add("\\new pop - yield + rtb only");
            }
            if (all_queries.Count == 0)
            {
                throw new Exception("Please select a data");
            }
            return folders_name;
        }
        public void generate_screenshot(Report_data report, string path, string folders_name)
        {

            switch (report.advertiser_name)
            {
                case "propellerads":
                    Propellerads propellerads = new Propellerads(path + folders_name);
                    propellerads.Get_reports_screen_shots(driver, report);
                    break;

                case "wigitmedia":
                    Wigitmedia wigitmedia = new Wigitmedia(path + folders_name);
                    wigitmedia.Get_reports_screen_shots(driver, report);
                    break;

                case "AdCash":
                    AdCash adcash = new AdCash(path + folders_name);
                    if (adcash_users_no_dups.Count() == 0)
                    {
                        adcash_users_no_dups[report.login_user] = report;
                        adcash.Get_reports_screen_shots(driver, report);
                        break;
                    }
                    if (adcash_users_no_dups.ContainsKey(report.login_user))
                    {
                        break;
                    }
                    else
                    {
                        adcash_users_no_dups[report.login_user] = report;
                        adcash.Get_reports_screen_shots(driver, report);
                        break;
                    }

                case "Revenue_hits":
                    Revenue_Hits revenue_hits = new Revenue_Hits(path + folders_name);
                    revenue_hits.Get_reports_screen_shots(driver, report);
                    break;

                case "Ad6Media":
                    {
                        Ad6media ad6meida = new Ad6media(path + folders_name);
                        ad6meida.Get_reports_screen_shots(driver, report);
                        break;
                    }

                case "AdsTerra":
                    {
                        if (adSterraSameUser == 0)
                        {
                            AdsTerra adsTerra = new AdsTerra(path + folders_name);
                            adsTerra.Get_reports_screen_shots(driver, report, from, to);
                            adSterraSameUser++;
                        }
                        break;
                    }

                case "Adsupply":
                    {
                        Adsupply adsupply = new Adsupply(path + folders_name);
                        if (adsupply_users_no_dups.Count() == 0)
                        {
                            adsupply_users_no_dups[report.login_user] = report;
                            adsupply.Get_reports_screen_shots(driver, report);
                            break;
                        }
                        if (adsupply_users_no_dups.ContainsKey(report.login_user))
                        {
                            break;
                        }
                        else
                        {
                            adsupply_users_no_dups[report.login_user] = report;
                            adsupply.Get_reports_screen_shots(driver, report);
                            break;
                        }

                    }

                case "Pubdirecte":
                    {
                        if (pubDirecteSameUser == 0)
                        {
                            Pubdirecte pubDirecte = new Pubdirecte(path + folders_name);
                            pubDirecte.Get_reports_screen_shots(driver, report, month_formatted[1]);
                            pubDirecteSameUser++;
                        }
                        break;
                    }

                case "Revtop":
                    {
                        if (revtopSameUser == 0)
                        {
                            Revtop revtop = new Revtop(path + folders_name);
                            revtop.Get_reports_screen_shots(driver, report);
                            revtopSameUser++;
                        }
                        break;
                    }

                case "InfinityAds":
                    {
                        if (infinitySameUser == 0)
                        {
                            InfinityAds infinity = new InfinityAds(path + folders_name) ;
                            infinity.Get_reports_screen_shots(driver, report);
                            infinitySameUser++;
                        }
                        break;
                    }

                case "Exoclick":
                    {
                        if (exoclickSameUser == 0)
                        {
                            Exoclick exoclick = new Exoclick(path + folders_name);
                            exoclick.Get_reports_screen_shots(driver, report, from, to);
                            exoclickSameUser++;
                        }
                        break;

                    }
                case "Popunder":
                    if (popunderSameUser == 0)
                    {
                        Popunder popunder = new Popunder(path + folders_name);
                        popunder.Get_reports_screen_shots(driver, report, from, to);
                        popunderSameUser++;
                    }
                    break;

                case "Velis":
                    if (report.connection_url.IndexOf("ply2c") > -1)
                    {
                        Ply2c velis2C = new Ply2c(path + folders_name, "https://velis.ply2c.com", report.advertiser_name);
                        velis2C.Get_reports_screen_shots(driver, report);
                        break;
                    }

                    Velis velis = new Velis(path + folders_name);
                    velis.Get_reports_screen_shots(driver, report);
                    break;

                case "152 Media":
                    {
                        Ply2c media = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        media.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "ad_maven":
                    {
                        Ply2c admaven = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        admaven.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "Adplex":
                    {
                        Ply2c adplex = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        adplex.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "Adstract":
                    {
                        Ply2c adstract = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        adstract.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "Dmg_Dsnr":
                    {
                        Ply2c dmg = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        dmg.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "snw":
                    {
                        if (report.connection_url.Contains("epom"))
                        {
                            SnwEpom epom = new SnwEpom(path + folders_name, "https://n159.epom.com/", report.advertiser_name);
                            epom.Get_reports_screen_shots(driver, report);
                        }
                        if (report.connection_url.Contains("ply2c"))
                        {
                            Ply2c snw = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                            snw.Get_reports_screen_shots(driver, report);
                        }
                    }
                    break;

                case "Matomy":
                    {
                        Ply2c matomy = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        matomy.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "Mango":
                    {
                        Ply2c mango = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        mango.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "HarrenMedia":
                    {
                        Ply2c harren = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        harren.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "BabaNetwork":
                    {
                        Ply2c baba = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                        baba.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "mari_media":
                    {
                        if (report.connection_url.IndexOf("ply2c") > -1)
                        {
                            Ply2c ply2c = new Ply2c(path + folders_name, report.connection_url, report.advertiser_name);
                            ply2c.Get_reports_screen_shots(driver, report);
                        }
                        if (report.connection_url.IndexOf("epom") > -1)
                        {
                            MarimediaEpom mari = new MarimediaEpom(path + folders_name, "https://n106.epom.com", report.advertiser_name);
                            mari.Get_reports_screen_shots(driver, report);
                        }
                    }
                    break;

                case "Adland":
                    {
                        Adland adland = new Adland(path + folders_name, "https://n152.epom.com", report.advertiser_name);
                        adland.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "Saludo":
                    {
                        Saludo saludo = new Saludo(path + folders_name, "https://n162.epom.com", report.advertiser_name);
                        saludo.Get_reports_screen_shots(driver, report);
                    }
                    break;

                case "xertive":
                    {
                        Xertive xertive = new Xertive(path + folders_name, "https://n80.epom.com", report.advertiser_name);
                        xertive.Get_reports_screen_shots(driver, report);
                    }
                    break;
            }
        }

        // Dummy methods for the form's functionallity
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
        private void listView1_Click(object sender, EventArgs e) { }
        private void checkBox_banner_CheckedChanged(object sender, EventArgs e) { }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

 


