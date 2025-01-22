namespace CssService.API.Models
{
    public class ServiceDto
    {
        public string AcKey { get; set; }
        public string AdDate { get; set; }

        public string AcReceiver { get; set; }

        public string AcDoc1 { get; set; } // Broj zapisnika
        public string AcFieldSI { get; set; } // Kontakt osoba

        public string AcFieldSJ { get; set; } // Telefon subjekta

        public string AcFieldSD { get; set; } // Email subjekta

        public string AcFieldSA { get; set; } // Tip masine

        public string AcFieldSB { get; set; } // Serijski broj masine

        public string AcNote { get; set; } // Opis kvara

        public string AcInternalNote { get; set; } // Napomena

        public string AdFieldDA { get; set; }

        public string AdFieldDB { get; set; }

        public double AnFieldNA { get; set; } // Utroseno vreme

        public double AnFieldNB { get; set; } // Kilometraza

        public string AcDocType { get; set; }
        public string AcStatus { get; set; }

        public string AcFieldSC { get; set; }  // U Garanciji - DA/NE
        public string AdFieldDC { get; set; } // Garancija OD
        public string AdFieldDD { get; set; } // Garancija DO

        public string AcFieldSG { get; set; }
        public string AcFieldSH { get; set; }
        public string AcFieldSE { get; set; }

        public string Signature { get; set; }
    }
}
