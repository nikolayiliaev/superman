using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_screen_shots
{
    class Queries
    {
        public string lightbox_direct_old_pop;
        public string banner;
        public string new_pop_yield_rtb_only;
        public Queries(List<string> dates, string advertiser)
        {
            String query = null;
            if (advertiser.Contains("ALL"))
            {
                query = "";
            }
            else
            {
                query = "where m.advertiser_name = " + advertiser;
                if (advertiser.Contains("Ply2C"))
                    if (advertiser.Contains("Velis"))
                        query = "where m.advertiser_name = 'Velis' and m.connection_url like '%ply2c%' ";
                    else
                        query = "and m.connection_url like '%ply2c%' ";
                if (advertiser.Contains("Epom"))
                    query = "and m.connection_url like '%epom%' ";
                    if (advertiser.Contains("snw"))
                        query = "where m.advertiser_name = 'snw' and m.connection_url like '%epom%' ";
                    if (advertiser.Contains("mari_media"))
                        query = "where m.advertiser_name = 'mari_media' and m.connection_url like '%epom%' ";
            }

            Console.WriteLine(query);
            this.lightbox_direct_old_pop = @"select m.campaign_name, m.login_user, m.login_pass, m.advertiser_name,m.connection_url
                                            from
                                            (
                                            select report_Date, campaign_id, sum(impressions) as Imps
                                            from ad_maven.daily_pop_impression_aggregation
                                            where report_Date BETWEEN '" + dates[0] + "' and '" + dates[1] + @"' and campaign_id >= 600000
                                            group by 1, 2) as pops left join bidding.campaigns_mongo m on pops.campaign_id = m.campaign_id " + query + " group by 1, 2, 3, 4, 5 order by campaign_name";

            this.banner = @"select m.advertiser_name, m.campaign_name, m.login_user, m.login_pass, m.connection_url
                            from
                            (
                             select report_Date, campaign_id , sum(impressions) as Imps
                             from ad_maven.daily_display_creative_impression_aggregation
                             where report_Date BETWEEN '" + dates[0] + "' and '" + dates[1] + @"' and campaign_id >= 600000
                             group by 1,2
                            ) as pops left join bidding.campaigns_mongo m on pops.campaign_id = m.campaign_id " + query + " GROUP BY 1,2,3,4,5 order by campaign_name";

            this.new_pop_yield_rtb_only = @"select m.advertiser_name,m.campaign_name, m.login_user, m.login_pass, m.connection_url
                                            from
                                            (
                                             select report_Date, campaign_id , 
	                                            sum(case when flow_type = 1 then 1 else 0 end) as yield_imps,
	                                            sum(case when flow_type = 2  then 1 else 0 end) as rtb_imps
                                             from ad_maven.rtb_action_pixel
                                             where report_Date BETWEEN '" + dates[0] + "' and '" + dates[1] + @"' and campaign_id >= 600000    
                                             group by 1,2
                                            ) as pops left join bidding.campaigns_mongo m on pops.campaign_id = m.campaign_id " + query + " GROUP BY 1,2,3,4,5 order by campaign_name";
        }
        public List<string> get_all_queries()
        {
            List<string> all_queries = new List<string>();
            all_queries.Add(this.lightbox_direct_old_pop);
            all_queries.Add(this.banner);
            all_queries.Add(this.new_pop_yield_rtb_only);
            return all_queries;
        }

        public string get_banner()
        {
            return this.banner;
        }

        public string get_new_pop()
        {
            return this.new_pop_yield_rtb_only;
        }

        public string get_lightbox()
        {
            return this.lightbox_direct_old_pop;
        }
    }
}
