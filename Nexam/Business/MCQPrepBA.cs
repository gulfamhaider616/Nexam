using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Nexam
{
    public class MCQPrepBA
    {
        #region Admin

        public string VerifyAdmin(string adminemail, string adminpassword)
        {
            DataSet dataset = new MCQPrepDA().VerifyAdmin(adminemail, adminpassword);
            string AdminName = "";
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataRow r = dataset.Tables[0].Rows[0];
                    AdminName = r["AdminName"].ToString();
                }
            }
            return AdminName;
        }

        public bool SaveSubject(SubjectDO subject)
        {
            DataSet set = new MCQPrepDA().SaveSubject(subject);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool SaveQuestion(QuestionDO question)
        {
            DataSet set = new MCQPrepDA().SaveQuestion(question);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<ContactDO> GetContacts()
        {
            List<ContactDO> list = new List<ContactDO>();
            DataSet dataset = new MCQPrepDA().GetContacts();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    ContactDO contact = new ContactDO();
                    contact.ContactID = dr.Field<int>("Id");
                    contact.Name = dr.Field<string>("Name");
                    contact.Email = dr.Field<string>("Email");
                    contact.Subject = dr.Field<string>("Subject");
                    contact.ContactDate = dr.Field<DateTime>("ContactDate").ToString();
                    list.Add(contact);
                }
            }
            return list;
        }

        public QuestionDO GetQuestionById(int questionid)
        {
            DataSet dataset = new MCQPrepDA().GetQuestionById(questionid);
            QuestionDO question = new QuestionDO();
            if (dataset.Tables.Count > 0)
            {
                DataRow dr = dataset.Tables[0].Rows[0];
                question.QuestionID = dr.Field<int>("QuestionID");
                question.SubjectID = dr.Field<int>("SubjectID");
                question.SubjectName = dr.Field<string>("SubjectName");
                question.QuestionStatement = dr.Field<string>("QuestionStatement");
                question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
                question.Choices = dr.Field<string>("Choices");
                question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
            }
            return question;
        }


        public bool SaveEditQuestion(QuestionDO question)
        {
            DataSet set = new MCQPrepDA().SaveEditQuestion(question);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool SaveEditPPSCQuestion(QuestionDO question)
        {
            DataSet set = new MCQPrepDA().SaveEditPPSCQuestion(question);
            if (set.Tables.Count > 0)
            {
                if (set.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool DeleteQuestion(int questionid)
        {
            int result = new MCQPrepDA().DeleteQuestionById(questionid);
            if (result > 0)
            {
                 return true;
            }
            else
            {
                return false;
            }
        }
        #endregion Admin

        #region User

        public List<SubjectDO> GetAllSubjects()
        {
            List<SubjectDO> list = new List<SubjectDO>();
            DataSet dataset = new MCQPrepDA().GetAllSubjects();
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    SubjectDO subject = new SubjectDO();
                    subject.SubjectID = dr.Field<int>("SubjectID");
                    subject.SubjectName = dr.Field<string>("SubjectName");
                    subject.SubjectDetail = dr.Field<string>("SubjectDetail");
                    list.Add(subject);
                }
            }
            return list;
        }

        public List<SubCategoryDO> GetSubCategoryBySubjectID(int subjectid)
        {
            List<SubCategoryDO> list = new List<SubCategoryDO>();
            DataSet dataset = new MCQPrepDA().GetSubCategoryBySubjectID(subjectid);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    SubCategoryDO subject = new SubCategoryDO();
                    subject.SubCategoryID = dr.Field<int>("SubjectSubCategoryID");
                    subject.SubjectID = dr.Field<int>("SubjectID");
                    subject.Name = dr.Field<string>("Name");
                    subject.Detail = dr.Field<string>("Detail");
                    list.Add(subject);
                }
            }
            return list;
        }

        public QuestionsListSubjectDO GetAllQuestionsBySubjectID(int subjectid)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new MCQPrepDA().GetAllQuestionsBySubjectID(subjectid);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuestionDO question = new QuestionDO();
                    question.QuestionID = dr.Field<int>("QuestionID");
                    question.SubjectID = dr.Field<int>("SubjectID");
                    question.PaperID = dr.Field<int>("PaperID");
                    question.QuestionStatement = dr.Field<string>("QuestionStatement");
                    question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
                    //question.Choices = dr.Field<string>("Choices");
                    if(!String.IsNullOrEmpty(dr.Field<string>("Choices")))
                    {
                        string allChoices = dr.Field<string>("Choices");
                        string[] choices = allChoices.Split(',','،');
                        List<OptionsDO> optionList = new List<OptionsDO>();
                        foreach (var choice in choices)
                        {
                            OptionsDO option = new OptionsDO();
                            option.Choice = choice;
                            optionList.Add(option);
                        }
                        question.Options = optionList;
                    }
                    question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
                    list.Add(question);
                }
                if(dataset.Tables[0].Rows.Count>0)
                obj.Subject= dataset.Tables[0].Rows[0].Field<string>("SubjectName");
            }
            obj.questions = list;
            obj.SubjectList = GetAllSubjects();
            return obj;
        }

        public bool SaveContact(ContactDO contact)
        {
            DataSet dataset = new MCQPrepDA().SaveContact(contact);
            if (dataset.Tables.Count > 0)
            {
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public QuestionsListSubjectDO SearchResult(string search)
        {
            QuestionsListSubjectDO obj = new QuestionsListSubjectDO();
            List<QuestionDO> list = new List<QuestionDO>();
            DataSet dataset = new MCQPrepDA().SearchResult(search);
            if (dataset.Tables.Count > 0)
            {
                foreach (DataRow dr in dataset.Tables[0].Rows)
                {
                    QuestionDO question = new QuestionDO();
                    question.QuestionID = dr.Field<int>("QuestionID");
                    //question.SubjectID = dr.Field<int>("SubjectID");
                    question.QuestionStatement = dr.Field<string>("QuestionStatement");
                    //question.NumberOfChoices = dr.Field<int>("NumberOfChoices");
                    //question.Choices = dr.Field<string>("Choices");
                    if (!String.IsNullOrEmpty(dr.Field<string>("Choices")))
                    {
                        string allChoices = dr.Field<string>("Choices");
                        string[] choices = allChoices.Split(',', '،');
                        List<OptionsDO> optionList = new List<OptionsDO>();
                        foreach (var choice in choices)
                        {
                            OptionsDO option = new OptionsDO();
                            option.Choice = choice;
                            optionList.Add(option);
                        }
                        question.Options = optionList;
                    }
                    question.CorrectAnswer = dr.Field<string>("CorrectAnswer");
                    list.Add(question);
                }
            }
            obj.questions = list;
            obj.SubjectList = GetAllSubjects();
            return obj;
        }

        #endregion User
    }
}