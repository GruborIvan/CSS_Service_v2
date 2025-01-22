using CssService.Domain.Interfaces;
using CssService.Domain.Models.ServisCollections;
using PdfSharpCore.Pdf;
using System.Net.Mail;
using System.Net;
using CssService.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace CssService.Infrastructure.Repositories.ExternalServicesRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly ILogger<EmailRepository> _logger;

        public EmailRepository(ILogger<EmailRepository> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailConfirmationAsync(ServisAdd servis, PdfDocument pdfDocument, string acDoc1, string emailAddress, string password)
        {
            if (emailAddress is null || password is null)
                return;

            string documentName = $"{acDoc1}.pdf";

            // Convert the PdfDocument to a byte array
            byte[] pdfBytes = ConvertPdfDocumentToByteArray(pdfDocument);

            // Should send 3 emails to: SendEmail, AcFieldSD, AcFieldSE.
            var emailAdresses = ExtractEmailAddresses(servis.SendEmail);

            if (!emailAdresses.Contains(servis.AcFieldSD))
                emailAdresses.Add(servis.AcFieldSD);

            if (!emailAdresses.Contains(servis.AcFieldSE))
                emailAdresses.Add(servis.AcFieldSE);

            emailAdresses = emailAdresses.Where(x => IsValidEmail(x)).ToList();

            foreach (var emailTo in emailAdresses)
            {
                var message = CreateMailMessage(
                    from: emailAddress,
                    to: emailTo,
                    subject: EmailConstants.emailMessageSubject,
                    body: EmailConstants.emailMessageBody,
                    pdfDocument: pdfBytes,
                    documentName: documentName
                );

                 await SendEmail(message, emailAddress, password);
            }
        }

        private MailMessage CreateMailMessage(string from, string to, string subject, string body, byte[] pdfDocument, string documentName)
        {
            var message = new MailMessage()
            {
                From = new MailAddress(from),
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            };

            var attachment = new Attachment(new MemoryStream(pdfDocument), "document.pdf", "application/pdf");
            attachment.Name = documentName;

            message.To.Add(new MailAddress(to));
            message.Attachments.Add(attachment);

            return message;
        }

        private byte[] ConvertPdfDocumentToByteArray(PdfDocument pdfDocument)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                pdfDocument.Save(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private List<string> ExtractEmailAddresses(string emailString)
        {
            return emailString.Split(';').ToList();
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private async Task SendEmail(MailMessage message, string email, string password)
        {
            try
            {
                var smtpClient = new SmtpClient("mail.gts-adriatic.rs")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true
                };

                await smtpClient.SendMailAsync(message);
                _logger.LogInformation($"Email successfully sent to: {email}.");
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Failed to send email to: {email}. Sending aborted.");
            }
        }
    }
}
