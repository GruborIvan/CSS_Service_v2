using CssService.Domain.Models;

namespace CssService.Domain.Queries.Skladiste
{
    public class GetSkladisteByDocType : IQuery<Models.Skladiste>
    {
        public string AcDocType { get; set; }

        public GetSkladisteByDocType(string acDocType)
        {
            AcDocType = acDocType;
        }
    }
}