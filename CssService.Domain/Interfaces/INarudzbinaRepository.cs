using CssService.Domain.Models;
using CssService.Domain.Models.NarudzbinaCollections;

namespace CssService.Domain.Interfaces
{
    public interface INarudzbinaRepository
    {
        Task<IEnumerable<Narudzbina>> GetNarudzbineByDocTypeAsync(string docType);
        Task<IEnumerable<NarudzbinaItem>> GetNarudzbinaItemsByDocTypeAsync(string docType);
        Task<string> PostNarudzbinaAsync(string acReciever, string adDate, int userId, string acDocType);
        Task PostNarudzbinaItemAsync(NarudzbinaItemPost orderItem);
        Task UpdateNarudzbinaPriceAsync(string acKey, double anValueIncrease, double anForPayIncrease);
        Task UpdateNarudzbina(double anValue, string acKey);
    }
}