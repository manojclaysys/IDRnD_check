using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace IDRnD_check.Helper
{
    public class DAL
    {
        SqlConnection sqlConn;
        SqlCommand sqlcomm = new SqlCommand();

        public DAL()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString;
            sqlConn = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Insert Call information into database.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="GatherInfo"></param>
        public void InsertEnrollmentData(string userID, string template)
        {
            sqlConn.Open();
            sqlcomm.Connection = sqlConn;
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.CommandText = Constants.InsertEnrollment;
            sqlcomm.Parameters.Add(new SqlParameter("@EmpCode", SqlDbType.VarChar)).Value = userID;
            sqlcomm.Parameters.Add(new SqlParameter("@Template", SqlDbType.VarChar)).Value = template;
            sqlcomm.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
            sqlcomm.Parameters.Add(new SqlParameter("@LastUpdatedDate", SqlDbType.DateTime)).Value = DateTime.Now;

            sqlcomm.ExecuteNonQuery();
            sqlcomm.Dispose();
            sqlConn.Close();
        }

        public void VerifyHistory(string userID, string template1, string template2, int type)
        {
            sqlConn.Open();
            sqlcomm.Connection = sqlConn;
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.CommandText = Constants.sp_voiceHistory;
            sqlcomm.Parameters.Add(new SqlParameter("@EmpCode", SqlDbType.VarChar)).Value = userID;
            sqlcomm.Parameters.Add(new SqlParameter("@Template1", SqlDbType.VarChar)).Value = template1;
            sqlcomm.Parameters.Add(new SqlParameter("@Template2", SqlDbType.VarChar)).Value = template2;
            sqlcomm.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
            sqlcomm.Parameters.Add(new SqlParameter("@LastUpdatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
            sqlcomm.Parameters.Add(new SqlParameter("@SourceType", SqlDbType.Int)).Value = type;

            sqlcomm.ExecuteNonQuery();
            sqlcomm.Dispose();
            sqlConn.Close();
        }

        public string GetTemplate(string UserID)
        {
            DataTable _dtCallInfo = new DataTable { TableName = "Template" };
            sqlConn.Open();
            sqlcomm.Connection = sqlConn;
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.CommandText = Constants.sp_GetTemplate;


            if (!string.IsNullOrEmpty(UserID))
                sqlcomm.Parameters.Add(new SqlParameter("@EmpCode", SqlDbType.VarChar)).Value = UserID;

            IDbDataParameter templateParameter = sqlcomm.CreateParameter();
            templateParameter.ParameterName = "@returnValue";
            templateParameter.Direction = ParameterDirection.Output;
            templateParameter.DbType = DbType.String;
            templateParameter.Size = int.MaxValue;
            sqlcomm.Parameters.Add(templateParameter);

            sqlcomm.ExecuteNonQuery();
           
            sqlcomm.Dispose();
            sqlConn.Close();

            string result =Convert.ToString(templateParameter.Value);

            return result;
        }

    }
}