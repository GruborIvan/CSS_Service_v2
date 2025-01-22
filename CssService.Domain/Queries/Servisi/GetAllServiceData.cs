using CssService.Domain.Models.ServisCollections;

namespace CssService.Domain.Queries.Servisi
{
    public class GetAllServiceData : IQuery<ServisReturn>
    {
        public string AcDocTypeService { get; set; }

        public GetAllServiceData(string acDocTypeService)
        {
            AcDocTypeService = acDocTypeService;
        }
    }
}