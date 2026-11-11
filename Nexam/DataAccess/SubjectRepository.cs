using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class SubjectRepository : DapperRepository, ISubjectRepository
{
    public SubjectRepository(IDbConnectionFactory factory) : base(factory) { }

    public Task<List<Subject>> GetAllAsync() => QueryProcAsync<Subject>("GetAllSubjects");

    public Task<bool> SaveAsync(Subject subject) =>
        ProcReturnsRowAsync("SaveSubject", new { subject.SubjectName, subject.SubjectDetail });
}
