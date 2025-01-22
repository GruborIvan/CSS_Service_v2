using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync(bool isBulkCall);
    }
}