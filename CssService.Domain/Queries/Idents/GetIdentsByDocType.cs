using CssService.Domain.Models;

namespace CssService.Domain.Queries.Idents
{
    public class GetIdentsByDocType : IQuery<IEnumerable<Ident>>
    {
        public string AcDocType { get; set; }

        public GetIdentsByDocType(string acDocType)
        {
            AcDocType = acDocType;
        }
    }
}