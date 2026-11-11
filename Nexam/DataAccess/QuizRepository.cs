using System.Data;
using Dapper;
using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class QuizRepository : DapperRepository, IQuizRepository
{
    public QuizRepository(IDbConnectionFactory factory) : base(factory) { }

    public async Task<(int QuizId, List<Question> Questions)> CreateAsync(Quiz quiz)
    {
        using var connection = Factory.Create();
        // INSERTQUIZ returns two result sets: the new quiz id, then the questions to ask.
        using var grid = await connection.QueryMultipleAsync("INSERTQUIZ", new
        {
            TITLE = quiz.Title,
            USERID = quiz.UserID,
            SUBJECTID = quiz.SubjectID,
            SCORE = quiz.Score,
            TOTALQUESTIONS = quiz.TotalQuestions,
            TIMELIMIT = quiz.TimeLimit,
            TOTALATTEMPT = quiz.TotalAttempts,
            ALLOWEDATTEMPT = quiz.AllowedAttempts
        }, commandType: CommandType.StoredProcedure);

        var idRow = await grid.ReadFirstOrDefaultAsync<QuizIdRow>();
        var questions = grid.IsConsumed ? new List<Question>() : (await grid.ReadAsync<Question>()).AsList();
        return (idRow?.QuizID ?? 0, questions);
    }

    public async Task SaveQuestionResultAsync(QuizQuestion q) =>
        await ExecuteProcAsync("INSERTQUIZQUESTIONS", new
        {
            QUIZID = q.QuizID,
            QUESTIONID = q.QuestionID,
            USERANSWER = q.UserAnswer,
            CORRECTANSWER = q.CorrectAnswer,
            ISCORRECT = q.IsCorrect
        });

    public async Task UpdateScoreAsync(int quizId, int score) =>
        await ExecuteProcAsync("UPDATEQUIZ", new { QUIZID = quizId, SCORE = score });

    public async Task<List<QuizQuestion>> GetQuestionsAsync(int quizId)
    {
        // GETQuizByID joins several tables and returns some column names twice (QUIZID, SUBJECTID, CorrectAnswer).
        // Reading by ordinal keeps the FIRST occurrence, which is what the app has always used
        // (the answer stored with the quiz question, not the current one on the question).
        using var connection = Factory.Create();
        using var reader = await connection.ExecuteReaderAsync("GETQuizByID", new { QUIZID = quizId },
            commandType: CommandType.StoredProcedure);

        var list = new List<QuizQuestion>();
        if (!reader.Read()) return list;

        int quizIdCol = reader.GetOrdinal("QUIZID"), subjectCol = reader.GetOrdinal("SubjectName"),
            titleCol = reader.GetOrdinal("TITLE"), scoreCol = reader.GetOrdinal("SCORE"),
            statementCol = reader.GetOrdinal("QuestionStatement"), choicesCol = reader.GetOrdinal("Choices"),
            correctCol = reader.GetOrdinal("CorrectAnswer"), userCol = reader.GetOrdinal("USERANSWER");

        do
        {
            var correct = reader.IsDBNull(correctCol) ? "" : reader.GetString(correctCol);
            var user = reader.IsDBNull(userCol) ? "" : reader.GetString(userCol);
            var scoreText = reader.IsDBNull(scoreCol) ? null : reader.GetString(scoreCol);
            list.Add(new QuizQuestion
            {
                QuizID = reader.GetInt32(quizIdCol),
                Subject = reader.IsDBNull(subjectCol) ? null : reader.GetString(subjectCol),
                Title = reader.IsDBNull(titleCol) ? null : reader.GetString(titleCol),
                Score = int.TryParse(scoreText, out var score) ? score : 0,
                QuestionStatement = reader.IsDBNull(statementCol) ? null : reader.GetString(statementCol),
                Choices = reader.IsDBNull(choicesCol) ? null : reader.GetString(choicesCol),
                CorrectAnswer = correct,
                UserAnswer = user,
                IsCorrect = correct.Trim() == user.Trim()
            });
        } while (reader.Read());

        return list;
    }
}
