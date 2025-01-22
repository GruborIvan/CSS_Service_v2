using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IContactPersonRepository
    {
        Task<IEnumerable<ContactPerson>> GetAllContactPersonsAsync();
        Task AddContactPersonAsync(string name, string subject, string email, string phoneNo);
    }
}