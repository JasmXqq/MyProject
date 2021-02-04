using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace graduationDesign
{
    static class sqlDataManager
    {
        public static string getConn()
        {
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["test"].ConnectionString;
            return constr;
        }
    }
}
