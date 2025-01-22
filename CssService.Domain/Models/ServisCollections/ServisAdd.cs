using CssService.Domain.Models.NarudzbinaCollections;

namespace CssService.Domain.Models.ServisCollections
{
    public class ServisAdd : NarudzbinaPost
    {
        public string AcDoc1 { get; set; } // Broj zapisnika
        public string AcFieldSI { get; set; } // Ime, Prezime porucioca.
        public string AcFieldSJ { get; set; } // Telefon porucioca.

        public string AcFieldSD { get; set; } // Email adresa porucioca.

        public string AcFieldSA { get; set; } // Tip masine

        public string AcFieldSB { get; set; } // Serijski broj masine

        public string AcNote { get; set; } // Opis kvara

        public string AcInternalNote { get; set; } // Napomena

        public DateTime AdFieldDA { get; set; }

        public DateTime AdFieldDB { get; set; }

        public int AnFieldNA { get; set; } // Utroseno vreme

        public int AnFieldNB { get; set; } // Kilometraza

        public string AcFieldSC { get; set; }  // U Garanciji - DA/NE

        public DateTime AdFieldDC { get; set; } // Garancija OD

        public DateTime AdFieldDD { get; set; } // Garancija DO

        public string AcFieldSG { get; set; } // Ime, Prezime potvrdioca

        public string AcFieldSH { get; set; } // Telefon potvrdioca

        public string AcFieldSE { get; set; } // Email potvrdioca

        public string SendEmail { get; set; }

        public string Signature { get; set; }
    }
}
