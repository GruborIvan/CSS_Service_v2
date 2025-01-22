using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Domain.Models.ServisCollections;
using PdfSharpCore.Pdf;

namespace CssService.Domain.Interfaces
{
    public interface IPdfRepository
    {
        PdfDocument GeneratePdfDocument(ServisAdd servis, string subjectAddress, List<NarudzbinaItemPost> narudzbinaItems);
    }
}