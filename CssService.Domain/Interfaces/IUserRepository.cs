using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
    }
}