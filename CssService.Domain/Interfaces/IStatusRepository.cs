using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IStatusRepository
    {
        Task<IEnumerable<Status>> GetStatusesAsync(bool isBulkCall);
    }
}