using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_screen_shots
{
    class Queries
    {
        public string propellorAdsList;
        
        public Queries()
        {
            this.propellorAdsList = @"select * from  from ad_maven.PropellerAdsUsersListShahaf";

        }

        public List<string> get_all_queries()
        {
            List<string> all_queries = new List<string>();
            all_queries.Add(this.propellorAdsList);
            
            return all_queries;
        }
     }
}
