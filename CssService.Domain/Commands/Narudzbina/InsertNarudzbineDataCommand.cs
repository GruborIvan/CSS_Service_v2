using CssService.Domain.Models.NarudzbinaCollections;
using MediatR;

namespace CssService.Domain.Commands.Narudzbina
{
    public class InsertNarudzbineDataCommand : IRequest<Guid>
    {
        public List<NarudzbinaPost> Narudzbine { get; set; }
        public List<NarudzbinaItemPost> StavkeNarudzbine { get; set; }
        public string DocType { get; set; }
        public string DocTypeService { get; set; }

        public InsertNarudzbineDataCommand(List<NarudzbinaPost> narudzbine, List<NarudzbinaItemPost> stavkeNarudzbine, string docType, string docTypeService)
        {
            Narudzbine = narudzbine;
            StavkeNarudzbine = stavkeNarudzbine;
            DocType = docType;
            DocTypeService = docTypeService;
        }
    }
}