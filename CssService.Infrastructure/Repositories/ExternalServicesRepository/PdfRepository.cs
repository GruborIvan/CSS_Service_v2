using CssService.Domain.Interfaces;
using CssService.Domain.Models.NarudzbinaCollections;
using CssService.Domain.Models.ServisCollections;
using CssService.Infrastructure.Constants;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using SixLabors.ImageSharp;

namespace CssService.Infrastructure.Repositories.ExternalServicesRepository
{
    public class PdfRepository : IPdfRepository  
    {
        private readonly ILogger<PdfRepository> _logger;
        private readonly string _pdfTemplatePath;

        public PdfRepository(ISubjectRepository subjectRepository, ILogger<PdfRepository> logger)
        {
            _logger = logger;
            _pdfTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Template.pdf");
        }

        public PdfDocument GeneratePdfDocument(ServisAdd servis, string subjectAddress, List<NarudzbinaItemPost> narudzbinaItems)
        {
            PdfDocument d = PdfReader.Open(_pdfTemplatePath, PdfDocumentOpenMode.Import);
            PdfDocument document = new PdfDocument();

            XFont regularFont = new XFont("Verdana", 10, XFontStyle.Bold);
            XFont boldFont = new XFont("Verdana", 8, XFontStyle.Bold);
            XFont regularFont10 = new XFont("Vedrana", 10, XFontStyle.Regular);
            XFont fontX = new XFont("Vedrana", 16, XFontStyle.Bold);

            for (int i = 0; i < d.Pages.Count; i++)
            {
                document.AddPage(d.Pages[i]);
            }

            PdfPage page = document.Pages[0];
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XTextFormatter tf = new XTextFormatter(gfx);

            // AdDate
            gfx.DrawString(
                servis.AdDate.ToString("dd/MM/yyyy"),
                regularFont,
                XBrushes.Black,
                new XRect(433, 89, page.Width, page.Height),
                XStringFormats.TopLeft
            );

            // AcDoc1 - Serijski broj
            gfx.DrawString(
                        servis.AcDoc1,
                        regularFont,
                        XBrushes.Black,
                        new XRect(480, 64, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AcSubject - Firma
            gfx.DrawString(
                        servis.AcReciever,
                        regularFont,
                        XBrushes.Black,
                        new XRect(100, 118, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AcAddress - Adresa
            gfx.DrawString(
                        subjectAddress,
                        regularFont,
                        XBrushes.Black,
                        new XRect(100, 134, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AcFieldSG & AcFieldSH - Ime i telefon potvrdioca.
            gfx.DrawString(
                        $"{servis.AcFieldSI},  {servis.AcFieldSJ}",
                        regularFont,
                        XBrushes.Black,
                        new XRect(90, 149, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AcFieldSA - Masina - Tip
            gfx.DrawString(
                        $"{servis.AcFieldSA},  Serijski broj: {servis.AcFieldSB}",
                        regularFont,
                        XBrushes.Black,
                        new XRect(120, 198, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AcNote - Opis kvara
            XRect rectNote = new XRect(53, 233, 490, 45);
            gfx.DrawRectangle(XBrushes.White, rectNote);
            tf.DrawString(servis.AcNote, boldFont, XBrushes.Black, rectNote, XStringFormats.TopLeft);

            // AcInternalNote - Primedbe
            XRect rectInternalNote = new XRect(53, 305, 490, 55);
            gfx.DrawRectangle(XBrushes.White, rectInternalNote);
            tf.DrawString(servis.AcInternalNote, boldFont, XBrushes.Black, rectInternalNote, XStringFormats.TopLeft);

            int paddingTop = 0;

            foreach (var item in narudzbinaItems)
            {
                gfx.DrawString(
                        $"{item.AcIdent}",
                        boldFont,
                        XBrushes.Black,
                        new XRect(85, 392 + paddingTop, page.Width, page.Height),
                        XStringFormats.TopLeft
                );

                gfx.DrawString(
                        $"{item.AcName}",
                        new XFont("Verdana", 7, XFontStyle.Bold),
                        XBrushes.Black,
                        new XRect(155, 391 + paddingTop, page.Width, page.Height),
                        XStringFormats.TopLeft
                );

                gfx.DrawString(
                        $"{item.AnQty}",
                        regularFont10,
                        XBrushes.Black,
                        new XRect(370, 390 + paddingTop, page.Width, page.Height),
                        XStringFormats.TopLeft
                );

                paddingTop += 18;
            }


            // AdFieldDA - Trajanje intervencije OD.
            gfx.DrawString(
                        $"{servis.AdFieldDA.ToString("HH:mm")}",
                        boldFont,
                        XBrushes.Black,
                        new XRect(138, 546, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AdFieldDB - Trajanje intervencije DO.
            gfx.DrawString(
                        $"{servis.AdFieldDB.ToString("HH:mm")}",
                        boldFont,
                        XBrushes.Black,
                        new XRect(204, 546, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // AnFieldNA - Broj radnih sati.
            gfx.DrawString(
                        $"{servis.AnFieldNA}",
                        boldFont,
                        XBrushes.Black,
                        new XRect(280, 546, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // GARANCIJA

            // Masina u garantnom roku
            bool podGarancijom = MasinaPodGarancijom(servis.AdFieldDD);
            int marginLeft = podGarancijom ? 70 : 117;

            //// Oznaka da li je masina u garanciji: 'X'.
            gfx.DrawString(
                        $"X",
                        fontX,
                        XBrushes.Black,
                        new XRect(marginLeft, 682, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // POTPIS

            // Cover up & Print signature person's name.
            XRect rectCoverUpSignatureNameKorisnik = new XRect(90, 790, 90, 8);
            gfx.DrawRectangle(XBrushes.White, rectCoverUpSignatureNameKorisnik);

            XRect rectCoverUpSignatureNameServiser = new XRect(415, 790, 120, 7);
            gfx.DrawRectangle(XBrushes.White, rectCoverUpSignatureNameServiser);

            // Ime korisnika ispod polja za potpis.
            gfx.DrawString(
                        $"{servis.AcFieldSG}",
                        regularFont10,
                        XBrushes.Black,
                        new XRect(415, 790, page.Width, page.Height),
                        XStringFormats.TopLeft
            );

            // Ime servisera ispod polja za potpis.
            // COMMENTED: Dok ne implementiramo cuvanje korisnicke sesije.
            // gfx.DrawString(
            //            $"{service.AcFieldSI}",
            //            regularFont10,
            //            XBrushes.Black,
            //            new XRect(90, 790, page.Width, page.Height),
            //            XStringFormats.TopLeft
            //);

            // Draw white rectangle to cover 'MP' texts from bottom of page.
            XRect rectCoverNote = new XRect(220, 758, 152, 30);
            gfx.DrawRectangle(XBrushes.White, rectCoverNote);


            // Signature - Potpis servisera.
            // If image is not able to be processed (not in base64 format), don't draw it at all.
            if (!string.IsNullOrEmpty(servis.Signature))
            {
                XImage image = SaveSignatureImage(servis.Signature);

                if (image is not null)
                {
                    gfx.DrawImage(image, 310, 630, 240, 350);
                }
            }


            // Draw text on bottom of the page.
            gfx.DrawString(
                        PdfConstants.BottomPageText,
                        boldFont,
                        XBrushes.Black,
                        new XRect(180, 820, page.Width, page.Height),
                        XStringFormats.TopLeft
            );


            // DISCLAIMER : For testing purposes only!
            //document.Save(_pdfNewFilePath);
            //Process.Start(_pdfNewFilePath);
            return document;
        }

        private XImage SaveSignatureImage(string signature)
        {
            try
            {
                var bytes = Convert.FromBase64String(signature);

                using (var ms = new MemoryStream(bytes))
                {
                    Func<Stream> streamFunc = () => ms;
                    var image = XImage.FromStream(streamFunc);
                    return image;
                }
            }
            catch (UnknownImageFormatException ex)
            {
                _logger.LogInformation($"Unable to save signature. \n {ex}");
                return null;
            }
        }

        private bool MasinaPodGarancijom(DateTime garancijaDo)
        {
            if (garancijaDo > DateTime.Now)
            {
                return true;
            }

            return false;
        }
    }
}