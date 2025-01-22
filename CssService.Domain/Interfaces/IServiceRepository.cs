using CssService.Domain.Models;
using CssService.Domain.Models.ServisCollections;

namespace CssService.Domain.Interfaces
{
    public interface IServiceRepository
    {
        Task<IEnumerable<Servis>> GetAllServicesAsync(string acDocTypeService);
        Task UpdateAdditionalServiceDataAsync(string acKey, ServisAdd servis);
        Task<bool> CheckIfServiceAlreadyAdded(string acDoc1, string acReceiver);
    }
}