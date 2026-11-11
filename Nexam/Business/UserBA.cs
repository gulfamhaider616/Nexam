using Nexam.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam.Business
{
    public class UserBA
    {
        public bool SaveUser(UserDO user)
        {
            DataSet dataset = new UserDA().SaveUser(user);
            var UserID = "";
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow r = dataset.Tables[0].Rows[0];
                    UserID = r["Id"].ToString();
                }
            }
            if (UserID == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public UserDO VerifyUser(string email, string password)
        {
            DataSet dataset = new UserDA().VerifyUser(email, password);
            UserDO user = new UserDO();
            //string user = "";
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow r = dataset.Tables[0].Rows[0];
                    user.FName = r["FirstName"].ToString();
                    user.Id = Convert.ToInt32(r["Id"]);
                }
            }
            return user;
        }
        public List<UserDO> GetAllUsers()
        {
            List<UserDO> list = new List<UserDO>();
            DataSet dataset = new UserDA().GetAllUsers();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    UserDO user = new UserDO();
                    user.Id = dr.Field<int>("Id");
                    user.FName = dr.Field<string>("FirstName");
                    user.LName = dr.Field<string>("LastName");
                    user.Email = dr.Field<string>("Email");
                    user.Password = dr.Field<string>("Password");
                    user.PNumber = dr.Field<string>("PhoneNumber");
                    user.Country = dr.Field<string>("Country");
                    user.City = dr.Field<string>("City");
                    list.Add(user);
                }
            }
            return list;
        }
        public UserDO GetUserProfile(int id)
        {
            UserDO user = new UserDO();
            DataSet dataset = new UserDA().GetUserProfile(id);
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dataset.Tables[0].Rows[0];
                    user.Id = dr.Field<int>("Id");
                    user.FName = dr.Field<string>("FirstName");
                    user.LName = dr.Field<string>("LastName");
                    user.Email = dr.Field<string>("Email");
                    user.Password = dr.Field<string>("Password");
                    user.PNumber = dr.Field<string>("PhoneNumber");
                    user.Country = dr.Field<string>("Country");
                    user.City = dr.Field<string>("City");
                }
            }
            return user;
        }
        public List<QuizDO> GetUserQuiz(int userID)
        {
            List<QuizDO> list = new List<QuizDO>();
            DataSet dataset = new UserDA().GetUserQuiz(userID);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuizDO user = new QuizDO();
                    user.QuizID = dr.Field<int>("QuizID");
                    user.SubjectName = dr.Field<string>("SubjectName");
                    user.Title = dr.Field<string>("Title");
                    user.Score = Convert.ToInt32(dr.Field<string>("Score"));
                    user.TotalQuestions = dr.Field<int>("TotalQuestions");
                    user.InsertDate = Convert.ToString(dr.Field<DateTime>("InsertDate").ToString());
                    user.UpdateDate = Convert.ToString(dr.Field<DateTime>("UpdateDate").ToString());
                    user.TotalAttempts = dr.Field<int>("TotalAttempt");
                    user.AllowedAttempts = dr.Field<int>("AllowedAttempt");
                    user.TimeLimit = dr.Field<int>("TimeLimit");
                    list.Add(user);
                }
            }
            return list;
        }
    }
}