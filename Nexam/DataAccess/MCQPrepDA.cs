using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexam
{
    public class MCQPrepDA
    {
        #region Admin
        public DataSet VerifyAdmin(string adminemail, string adminpassword)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.VerifyAdmin");
                command.Parameters.Add("@AdminEmail", SqlDbType.VarChar).Value = adminemail;
                command.Parameters.Add("@AdminPassword", SqlDbType.VarChar).Value = adminpassword;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", adminemail,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public DataSet SaveQuestion(QuestionDO question)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveQuestion");
                command.Parameters.AddWithValue("@SubjectID",question.SubjectID);
                command.Parameters.AddWithValue("@QuestionStatement",question.QuestionStatement);
                command.Parameters.AddWithValue("@NumberOfChoices", question.NumberOfChoices);
                command.Parameters.AddWithValue("@Choices",question.Choices);
                command.Parameters.AddWithValue("@CorrectAnswer",question.CorrectAnswer);
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

        public DataSet SaveSubject(SubjectDO subject)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveSubject");
                command.Parameters.AddWithValue("@SubjectName",subject.SubjectName);
                command.Parameters.AddWithValue("@SubjectDetail",subject.SubjectDetail);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", subject,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public DataSet GetContacts()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetContacts");
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
        public DataSet GetQuestionById(int questionid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetQuestionById");
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

        public DataSet SaveEditQuestion(QuestionDO question)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveEditQuestion");
                command.Parameters.AddWithValue("@QuestionID",question.QuestionID);
                command.Parameters.AddWithValue("@SubjectID",question.SubjectID);
                command.Parameters.AddWithValue("@QuestionStatement",question.QuestionStatement);
                command.Parameters.AddWithValue("@NumberOfChoices",question.NumberOfChoices);
                command.Parameters.AddWithValue("@Choices",question.Choices);
                command.Parameters.AddWithValue("@CorrectAnswer",question.CorrectAnswer);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", question,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public DataSet SaveEditPPSCQuestion(QuestionDO question)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveEditPPSCQuestion");
                command.Parameters.AddWithValue("@QuestionID", question.QuestionID);
                command.Parameters.AddWithValue("@SubjectID", question.SubjectID);
                command.Parameters.AddWithValue("@QuestionStatement", question.QuestionStatement);
                command.Parameters.AddWithValue("@NumberOfChoices", question.NumberOfChoices);
                command.Parameters.AddWithValue("@Choices", question.Choices);
                command.Parameters.AddWithValue("@CorrectAnswer", question.CorrectAnswer);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", question,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public int DeleteQuestionById(int questionid)
        {
            int result = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.DeleteQuestionById");
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionid;
                
                result = new DBConnection().ExecuteNonQuery(command);
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", questionid,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return result;
        }

        #endregion Admin

        #region User
        public DataSet GetAllQuestionsBySubjectID(int subjectid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllQuestionsBySubjectID");
                command.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectid;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                //logger.Debug("Error executing " + objValue.CommandText + ": " + ex.ToString());
                throw new Exception(string.Concat("Error executing the ", subjectid,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }


        /// <summary>
        /// Get All Subjects
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllSubjects()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllSubjects");
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

        /// <summary>
        /// Get all categories for a subject
        /// </summary>
        /// <param name="subjectid"></param>
        /// <returns></returns>
        public DataSet GetSubCategoryBySubjectID(int subjectid)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetSubCategoryBySubjectID");
                command.Parameters.Add("@SubjectID", SqlDbType.Int).Value = subjectid;
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
        public DataSet SaveContact(ContactDO contact)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SaveContactUs");
                command.Parameters.Add("@Name", SqlDbType.VarChar).Value = contact.Name;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = contact.Email;
                command.Parameters.Add("@Subject", SqlDbType.VarChar).Value = contact.Subject;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", contact,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public DataSet SearchResult(string search)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.SearchResult");
                command.Parameters.Add("@Search", SqlDbType.VarChar).Value = search;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", search,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }
        public DataSet ShowAllBlogs()
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("ahmedshair.GetAllSubjects");
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

        #endregion User
    }
}
