using Nexam.DataAccess.Interfaces;
using Nexam.DataAccess.Rows;
using Nexam.Models.Entities;

namespace Nexam.DataAccess;

public sealed class ContactRepository : DapperRepository, IContactRepository
{
    public ContactRepository(IDbConnectionFactory factory) : base(factory) { }

    public async Task<List<ContactMessage>> GetAllAsync()
    {
        var rows = await QueryProcAsync<ContactRow>("GetContacts");
        return rows.Select(r => new ContactMessage
        {
            ContactID = r.Id,
            Name = r.Name,
            Email = r.Email,
            Subject = r.Subject,
            ContactDate = r.ContactDate.ToString()
        }).ToList();
    }

    public Task<bool> SaveAsync(ContactMessage contact) =>
        ProcReturnsRowAsync("SaveContactUs", new { contact.Name, contact.Email, contact.Subject });
}
