using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Nexam.DataAccess
{
    public class QuizDA
    {
        public DataSet SaveQuiz(QuizDO quiz)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("INSERTQUIZ");
                command.Parameters.AddWithValue("@TITLE", quiz.Title);
                command.Parameters.AddWithValue("@USERID", quiz.UserID);
                command.Parameters.AddWithValue("@SUBJECTID", quiz.SubjectID);
                command.Parameters.AddWithValue("@SCORE", quiz.Score);
                command.Parameters.AddWithValue("@TOTALQUESTIONS", quiz.TotalQuestions);
                command.Parameters.AddWithValue("@TIMELIMIT", quiz.TimeLimit);
                command.Parameters.AddWithValue("@TOTALATTEMPT", quiz.TotalAttempts);
                command.Parameters.AddWithValue("@ALLOWEDATTEMPT", quiz.AllowedAttempts);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                command.Connection.Close();
                adapter.Fill(set);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", quiz,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return set;
        }

        public int SaveQuizQuestions(QuizQuestionDO quizQues)
        {
            int res = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("INSERTQUIZQUESTIONS");
                command.Parameters.AddWithValue("@QUIZID", quizQues.QuizID);
                command.Parameters.AddWithValue("@QUESTIONID", quizQues.QuestionID);
                command.Parameters.AddWithValue("@USERANSWER", quizQues.UserAnswer);
                command.Parameters.AddWithValue("@CORRECTANSWER", quizQues.CorrectAnswer);
                command.Parameters.AddWithValue("@ISCORRECT", quizQues.IsCorrect);
                res= command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", quizQues,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return res;
        }

        public int UpdateQuiz(int QuizID, int score)
        {
            int res = 0;
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("UPDATEQUIZ");
                command.Parameters.AddWithValue("@QUIZID", QuizID);
                command.Parameters.AddWithValue("@SCORE", score);
                res = command.ExecuteNonQuery();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Concat("Error executing the ", QuizID,
                    " in Execute.ExecuteDataSet(SQLCommand) method."), ex);
            }
            return res;
        }
        public DataSet GeTQuizById(int quizID)
        {
            DataSet set = new DataSet();
            try
            {
                SqlCommand command = new DBConnection().DbSqlConnection("GETQuizByID");
                command.Parameters.AddWithValue("@QUIZID", quizID);
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

        //static void InsertQuestions(IEnumerable<QuizQuestionDO> settings)
        //{
        //    using (SqlConnection oConnection = new SqlConnection("Data Source=(local);Initial Catalog=Wip;Integrated Security=True"))
        //    {
        //        oConnection.Open();
        //        using (SqlTransaction oTransaction = oConnection.BeginTransaction())
        //        {
        //            using (SqlCommand oCommand = oConnection.CreateCommand())
        //            {
        //                oCommand.Transaction = oTransaction;
        //                oCommand.CommandType = CommandType.Text;
        //                oCommand.CommandText = "INSERT INTO [Setting] ([Key], [Value]) VALUES (@key, @value);";
        //                oCommand.Parameters.Add(new SqlParameter("@key", SqlDbType.NChar));
        //                oCommand.Parameters.Add(new SqlParameter("@value", SqlDbType.NChar));
        //                try
        //                {
        //                    foreach (var oSetting in settings)
        //                    {
        //                        oCommand.Parameters[0].Value = oSetting.Key;
        //                        oCommand.Parameters[1].Value = oSetting.Value;
        //                        if (oCommand.ExecuteNonQuery() != 1)
        //                        {
        //                            //'handled as needed, 
        //                            //' but this snippet will throw an exception to force a rollback
        //                            throw new InvalidProgramException();
        //                        }
        //                    }
        //                    oTransaction.Commit();
        //                }
        //                catch (Exception)
        //                {
        //                    oTransaction.Rollback();
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}