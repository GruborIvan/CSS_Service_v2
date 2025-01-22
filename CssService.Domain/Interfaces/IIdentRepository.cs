using CssService.Domain.Models;

namespace CssService.Domain.Interfaces
{
    public interface IIdentRepository
    {
        Task<IEnumerable<Ident>> GetIdentsByDocTypeAsync(string acDocType, bool isBulkCall);
    }
}