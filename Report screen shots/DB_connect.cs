using System.Data.Odbc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Npgsql;


namespace Report_screen_shots
{
    class DB_connect
    {
        private NpgsqlConnection connection;
        List<string> dates;

        public DB_connect(string server, string port, string username , string password , string DBName)
        {
            string connstring = String.Format("Server={0};Port={1};" +
                    "User Id={2};Password={3};Database={4};Command Timeout=0;Pooling=false",
                    server, port, username,
                    password, DBName);
            this.connection = new NpgsqlConnection(connstring);
        }

        public bool open_connection()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool close_connection()
        {
            try
            {
                this.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public NpgsqlConnection get_connection()
        {
            return this.connection;
        }

        public List<Report_data> get_reports(string query)
        {
            try
            {
                List<Report_data> all_reports = new List<Report_data>();
                this.open_connection();                           
                NpgsqlCommand cmd = new NpgsqlCommand(query, get_connection());
                NpgsqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Report_data report = new Report_data();
                    report.campaign_name = dataReader["campaign_name"].ToString();
                    report.advertiser_name = dataReader["advertiser_name"].ToString();
                    report.connection_url = dataReader["connection_url"].ToString();
                    report.login_pass = dataReader["login_pass"].ToString();
                    report.login_user = dataReader["login_user"].ToString();
                    all_reports.Add(report);
                }
                if (this.connection.State == ConnectionState.Open)
                    this.close_connection();
                return all_reports;
            }
            catch (Exception e)
            {
                if (this.connection.State == ConnectionState.Open)
                    this.close_connection();
                return null;
            }
        }
    }
}
