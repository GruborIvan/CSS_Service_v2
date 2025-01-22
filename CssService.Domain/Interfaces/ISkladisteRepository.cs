using CssService.Domain.Models;
using CssService.Domain.Models.NarudzbinaCollections;

namespace CssService.Domain.Interfaces
{
    public interface ISkladisteRepository
    {
        Task<Skladiste> GetSkladisteByDocTypeAsync(string acDocType);
        Task<string> SaveSkladisteAsync(string docType, string adDate);
        Task SaveDokumentPrenosaAsync(string acKeySkladiste, string adDate, string docTypeStockTranfer, string docTypeWarehouseIssuer, string docTypeWarehouseReceiver);
        Task SaveStavkaDokumentaPrenosaAsync(string acKeySkladiste, NarudzbinaItemPost stavka);
    }
}