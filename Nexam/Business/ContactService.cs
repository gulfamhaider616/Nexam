using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;

namespace Nexam.Business;

public sealed class ContactService(IContactRepository contacts) : IContactService
{
    public Task<List<ContactMessage>> GetAllAsync() => contacts.GetAllAsync();

    public Task<bool> SaveAsync(ContactMessage contact) => contacts.SaveAsync(contact);
}
