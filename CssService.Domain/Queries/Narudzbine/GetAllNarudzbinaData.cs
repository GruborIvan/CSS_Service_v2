using CssService.Domain.Models.NarudzbinaReturn;

namespace CssService.Domain.Queries.Narudzbine
{
    public class GetAllNarudzbinaData : IQuery<NarudzbinaReturn>
    {
        public string AcDocType { get; set; }
        public string AcDocTypeService { get; set; }

        public GetAllNarudzbinaData(string acDocType, string acDocTypeService)
        {
            AcDocType = acDocType;
            AcDocTypeService = acDocTypeService;
        }
    }
}