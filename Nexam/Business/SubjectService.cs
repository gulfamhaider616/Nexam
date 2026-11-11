using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.Business;

public sealed class SubjectService(ISubjectRepository subjects) : ISubjectService
{
    public Task<List<Subject>> GetAllAsync() => subjects.GetAllAsync();

    public Task<bool> SaveAsync(Subject subject) => subjects.SaveAsync(subject);
}
