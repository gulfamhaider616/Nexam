using Nexam.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Nexam.DataAccess
{
    public class ForumDA
    {
        /// <summary>
        /// Save new Question in Database
        /// </summary>
        /// <param name="qustion"></param>
        /// <returns></returns>
        public DataSet SaveQuestion(AskQuestionDO qustion)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveForumQuestion");
                command.Parameters.AddWithValue("@UserName",qustion.UserName);
                command.Parameters.AddWithValue("@Email",qustion.Email);
                command.Parameters.AddWithValue("@Country",qustion.Country);
                command.Parameters.AddWithValue("@Subject",qustion.Subject);
                command.Parameters.AddWithValue("@Explanation",qustion.Explanation);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", qustion,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        /// <summary>
        /// Get All Questions
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllQuestions()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllForumQuestions");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        /// <summary>
        /// Get All Published Questions
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllPublishedQuestions()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllPublishQuestions");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        /// <summary>
        /// Get All UnPublished Questions
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllUnPublishedQuestions()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllUnPublishQuestions");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        /// <summary>
        /// Delete Question
        /// </summary>
        /// <returns></returns>
        public int DeleteQuestions(int QuestionID)
        {
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.DeleteQuestions");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;
                return new DBConnection().ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
        }
        /// <summary>
        /// Publish Question By Admin
        /// </summary>
        /// <returns></returns>
        public int PublishQuestionByAdmin(int QuestionID)
        {
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.PublishQuestionByAdmin");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;
                return new DBConnection().ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
        }
        /// <summary>
        /// GET Question by QuestionID
        /// </summary>
        /// <returns></returns>
        public DataSet GetSingleQuestion(int QuestionID)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetSingleQuestion");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", "",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
    }
}