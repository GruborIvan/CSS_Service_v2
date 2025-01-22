using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Domain.Models.ServisCollections;
using MediatR;

namespace CssService.Domain.Commands.Servis
{
    public class InsertServisCommand : IRequest<Guid>
    {
        public List<ServisAdd> Servisi {  get; set; }
        public List<NarudzbinaItemPost> StavkeServisa { get; set; }

        public string DocType { get; set; }
        public string DocTypeStockTranfer { get; set; }
        public string DocTypeWarehouseIssuer { get; set; }
        public string DocTypeWarehouseReceiver { get; set; }


        public string Mail { get; set; }
        public string Password { get; set; }
    }
}