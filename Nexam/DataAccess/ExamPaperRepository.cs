using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

/// <summary>Names of the stored procedures an exam body's papers/questions live behind.</summary>
public sealed record ExamProcedures(
    string GetPapers,
    string SavePaper,
    string GetQuestionsByPaper,
    string GetQuestion,
    string SaveQuestion,
    string DeleteQuestion);

/// <summary>NTS and FPSC share an identical shape; only the procedure names differ.</summary>
public abstract class ExamPaperRepository : DapperRepository, IExamPaperRepository
{
    private readonly ExamProcedures _procedures;

    protected ExamPaperRepository(IDbConnectionFactory factory, ExamProcedures procedures) : base(factory) =>
        _procedures = procedures;

    public Task<List<Paper>> GetPapersAsync() => QueryProcAsync<Paper>(_procedures.GetPapers);

    public Task<bool> SavePaperAsync(Paper paper) =>
        ProcReturnsRowAsync(_procedures.SavePaper, new { paper.PaperName, paper.PaperDetail });

    public Task<List<Question>> GetQuestionsByPaperAsync(int paperId) =>
        QueryProcAsync<Question>(_procedures.GetQuestionsByPaper, new { PaperID = paperId });

    public Task<Question> GetQuestionByIdAsync(int questionId) =>
        QueryFirstProcAsync<Question>(_procedures.GetQuestion, new { QuestionID = questionId });

    public Task<bool> SaveQuestionAsync(Question q) =>
        ProcReturnsRowAsync(_procedures.SaveQuestion, new { q.PaperID, q.QuestionStatement, q.Choices, q.CorrectAnswer });

    public async Task<bool> DeleteQuestionAsync(int questionId) =>
        await ExecuteProcAsync(_procedures.DeleteQuestion, new { QuestionID = questionId }) > 0;
}

public sealed class NtsRepository : ExamPaperRepository, INtsRepository
{
    public NtsRepository(IDbConnectionFactory factory) : base(factory, new ExamProcedures(
        GetPapers: "GetAllNTSPaper",
        SavePaper: "SaveNTSPaper",
        GetQuestionsByPaper: "GetAllNTSQuestionsByPaperID",
        GetQuestion: "GetNTSQuestionById",
        SaveQuestion: "SaveNTSQuestion",
        DeleteQuestion: "DeleteNTSQuestionById")) { }
}

public sealed class FpscRepository : ExamPaperRepository, IFpscRepository
{
    public FpscRepository(IDbConnectionFactory factory) : base(factory, new ExamProcedures(
        GetPapers: "GetAllFPSCPaper",
        SavePaper: "SaveFPSCPaper",
        GetQuestionsByPaper: "GetAllFPSCQuestionsByPaperID",
        GetQuestion: "GetFPSCQuestionById",
        SaveQuestion: "SaveFPSCQuestion",
        DeleteQuestion: "DeleteFPSCQuestionById")) { }
}
