using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace Nexam.DataAccess
{
    public class DBConnection
    {
        public SqlCommand DbSqlConnection(string query)
        {
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            //string conString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ahmadshair\Dropbox\Nexam\Nexam\Nexam\App_Data\NEXAM.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(conString);
            SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            return command;
        }
        /// <summary>
        /// Method for executing an INSERT/UPDATE/DELETE statements passed as
        /// sql command using the ConnectionClass connection
        /// </summary>
        /// <param name="objValue">Sql command value that contains command text and command type information.</param>
        /// <c>Version 1.0</c>
        public int ExecuteNonQuery(SqlCommand objValue)
        {
            int res = 0;
            try
            {
                //string conString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=C:\Users\Ahmadshair\Dropbox\Nexam\Nexam\Nexam\App_Data\NEXAM.mdf;Integrated Security=True";
                string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(conString);
                objValue.Connection = connection;
                objValue.Connection.Open();
                res= objValue.ExecuteNonQuery();
                objValue.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", objValue.CommandText,
                    " in Execute.ExecuteNonQuery(SqlCommand) method."), ex);
            }
            return res;
        }
    }
}
