using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IReferentRepository
    {
        Task<IEnumerable<Referent>> GetReferentiAsync();
    }
}