using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;

namespace WebApp.DatabaseMethods
{
   
    public class DBMethods
    {
        static string sSqlConstr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        DataSet dsResult = null;
        DataTable dtResult = null;
        DataSet dtencounter = null;
        public DataSet GetTimezoneOffsetval(string TimeZoneVal)
        {
            //, out string TimeZoneOffsetval
            //TimeZoneOffsetval = string.Empty;
            try
            {
             
                  
                    SqlParameter[] spc = new SqlParameter[2];
                    spc[0] = new SqlParameter("@TimezoneName", TimeZoneVal);
                    //spc[1] = new SqlParameter("@TimezoneOffsetval", SqlDbType.VarChar, 100);
                    //spc[1].Direction = ParameterDirection.Output;
                   dsResult=SqlHelper.ExecuteDataset(sSqlConstr, CommandType.StoredProcedure, "Udp_GetTimezoneOffsetValue", spc);
                //TimeZoneOffsetval = Convert.ToString(spc[1].Value);
              
            }
            catch (Exception exlog)
            {

            }
            finally
            {

            }
            return dsResult;
        }
    }
}