using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Nexam.DataAccess
{
    public class NTSDA
    {
        public DataSet SaveNTSNewPaper(PaperDO paper)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveNTSPaper");
                command.Parameters.AddWithValue("@PaperName",paper.PaperName);
                command.Parameters.AddWithValue("@PaperDetail",paper.PaperDetail);
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
        public DataSet SaveNTSQuestion(QuestionDO question)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveNTSQuestion");
                command.Parameters.AddWithValue("@PaperID", question.PaperID);
                command.Parameters.AddWithValue("@QuestionStatement", question.QuestionStatement);
                command.Parameters.AddWithValue("@Choices", question.Choices);
                command.Parameters.AddWithValue("@CorrectAnswer", question.CorrectAnswer);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", question,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public DataSet GetAllNTSPaper()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllNTSPaper");
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
        public DataSet GetAllNTSQuestionsByPaperID(int paperid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllNTSQuestionsByPaperID");
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
        public DataSet GetNTSQuestionById(int questionid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetNTSQuestionById");
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
        public int DeleteNTSQuestionById(int questionid)
        {
            int result = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.DeleteNTSQuestionById");
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