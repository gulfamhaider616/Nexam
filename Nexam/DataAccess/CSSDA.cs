using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Nexam.DataAccess
{
	public class CSSDA
	{
        public DataSet SaveCSSNewPaper(PaperDO paper)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("SaveCSSPaper");
                command.Parameters.AddWithValue("@PaperName", paper.PaperName);
                command.Parameters.AddWithValue("@PaperDetail", paper.PaperDetail);
                command.Parameters.AddWithValue("@QuestionsText", paper.QuestionsText);
                command.Parameters.AddWithValue("@PaperSlug", paper.PaperName.Replace(' ','-'));
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", paper,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        //public DataSet SaveCSSQuestion(QuestionDO question)
        //{
        //    DataSet set = new DataSet();
        //    try
        //    {
        //        SqlCommand command = new DBConnection().DbSqlConnection("SaveCSSQuestion");
        //        command.Parameters.AddWithValue("@PaperID", question.PaperID);
        //        command.Parameters.AddWithValue("@QuestionStatement", question.QuestionStatement);
        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        command.Connection.Close();
        //        adapter.Fill(set);
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
        //        throw new Exception(string.Concat("Error executing the ", question,
        //            " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
        //    }
        //    return set;
        //}
        public DataSet GetAllCSSPaper()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("GetAllCSSPaper");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ",
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public DataSet GetCompleteCSSPaperByID(int paperid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("GetCompleteCSSPaperByID");
                command.Parameters.Add("@PaperID", SqlDbType.Int).Value = paperid;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", paperid,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public DataSet GetCSSQuestionById(int questionid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("GetCSSQuestionById");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionid;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", questionid,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public int DeleteCSSQuestionById(int questionid)
        {
            int result = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("DeleteCSSQuestionById");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionid;
                command.Connection.Close();
                result = new DBConnection().ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", questionid,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return result;
        }
    }
}