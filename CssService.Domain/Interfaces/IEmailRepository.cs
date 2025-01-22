using CssService.Domain.Models.ServisCollections;
using PdfSharpCore.Pdf;

namespace CssService.Domain.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailConfirmationAsync(ServisAdd servis, PdfDocument pdfDocument, string acDoc1, string emailAddress, string password);
    }
}