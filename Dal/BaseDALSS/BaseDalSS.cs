using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Dal.BaseDALSS
{
    public class BaseDalSS
    {
        public static DataTable EjecutarCrud(string query)
        {
            DataTable returnDT = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
            using (SqlConnection sql = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, sql))
                {
                    DataTable sqlSRData133 = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    sql.Open();
                    da.Fill(sqlSRData133);
                    returnDT = sqlSRData133;
                }
            }
            return returnDT;
        }

    }
}
