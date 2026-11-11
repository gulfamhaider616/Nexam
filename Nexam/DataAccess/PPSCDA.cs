using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Nexam.DataAccess
{
    public class PPSCDA
    {
        public DataSet SaveNewPaper(PaperDO paper)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SavePPSCPaper");
                command.Parameters.AddWithValue("@PaperYearID", paper.PaperYearID);
                command.Parameters.AddWithValue("@PaperPostID", paper.PaperPostID);
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
        public DataSet UpdatePPSCPaper(PaperDO paper)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.UpdatePPSCPaper");
                command.Parameters.AddWithValue("@PaperID", paper.PaperID);
                command.Parameters.AddWithValue("@PaperYearID", paper.PaperYearID);
                command.Parameters.AddWithValue("@PaperPostID", paper.PaperPostID);
                command.Parameters.AddWithValue("@PaperName", paper.PaperName);
                command.Parameters.AddWithValue("@PaperDetail", paper.PaperDetail);
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
        public DataSet SavePPSCPost(PostDO post)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("SavePPSCPost");
                command.Parameters.AddWithValue("@PostName", post.PostName);
                command.Parameters.AddWithValue("@PostDetail", post.PostDetail);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", post,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public DataSet SavePPSCQuestion(QuestionDO question)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SavePPSCQuestion");
                command.Parameters.AddWithValue("@PaperID", question.PaperID);
                command.Parameters.AddWithValue("@SubjectID", question.SubjectID);
                command.Parameters.AddWithValue("@QuestionStatement", question.QuestionStatement);
                command.Parameters.AddWithValue("@Choices", question.Choices);
                command.Parameters.AddWithValue("@CorrectAnswer", question.CorrectAnswer);
                command.Parameters.AddWithValue("@NumberOfChoices",question.NumberOfChoices);
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
        public DataSet GetAllPPSCPaper()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllPaper");
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
        public DataSet SearchPPSCPaperYearwise(int yearid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SearchPPSCPaperYearwise");
                command.Parameters.AddWithValue("@YearID", yearid);
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
        public DataSet GetPaperYearPost()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetPaperYearPost");
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
        public DataSet GetPPSCPaperDetail(int paperid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("GetPPSCPaperDetail");
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
        public DataSet GetAllQuestionsByPaperID(int paperid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllQuestionsByPaperID");
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
        public DataSet GetPPSCQuestionById(int questionid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetPPSCQuestionById");
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
        public int DeletePPSCQuestionById(int questionid)
        {
            int result = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.DeletePPSCQuestionById");
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