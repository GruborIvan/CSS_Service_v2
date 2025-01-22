namespace CssService.Infrastructure.Models
{
    public class ServiceDbo
    {
        public string AcKey { get; set; }
        public string AcStatus { get; set; }
        public DateTime AdDate { get; set; }
        public string AcDocType { get; set; } // Tip dokumenta.
        public string AcDoc1 { get; set; } // Broj zapisnika.
        public string AcReceiver { get; set; }
        public string AcFieldSI { get; set; } // Kontakt osoba
        public string AcFieldSJ { get; set; } // Telefon subjekta
        public string AcFieldSA { get; set; } // Tip masine
        public string AcFieldSB { get; set; } // Serijski broj masine
        public string AcNote { get; set; } // Opis kvara
        public string AcInternalNote { get; set; } // Napomena
        public DateTime AdFieldDA { get; set; }

        public DateTime AdFieldDB { get; set; }

        public decimal AnFieldNA { get; set; } // Utroseno vreme

        public decimal AnFieldNB { get; set; } // Kilometraza
        public string AcSignature { get; set; }
        public string AcFieldSD { get; set; } // Email subjekta
        public string AcFieldSC { get; set; }  // U Garanciji - DA/NE
        public DateTime AdFieldDC { get; set; } // Garancija OD
        public DateTime AdFieldDD { get; set; } // Garancija DO

        public string AcFieldSG { get; set; }
        public string AcFieldSH { get; set; }
        public string AcFieldSE { get; set; }

        public ServiceDbo(string acKey, string acStatus, DateTime adDate, string acDocType, string acDoc1, string acReceiver, string acFieldSI, string acFieldSJ, string acFieldSA, string acFieldSB, string acNote, string acInternalNote, DateTime adFieldDA, DateTime adFieldDB, decimal anFieldNA, decimal anFieldNB, string acSignature, string acFieldSD, string acFieldSC, DateTime adFieldDC, DateTime adFieldDD, string acFieldSG, string acFieldSH, string acFieldSE)
        {
            AcKey = acKey;
            AcStatus = acStatus;
            AdDate = adDate;
            AcDocType = acDocType;
            AcDoc1 = acDoc1;
            AcReceiver = acReceiver;
            AcFieldSI = acFieldSI;
            AcFieldSJ = acFieldSJ;
            AcFieldSA = acFieldSA;
            AcFieldSB = acFieldSB;
            AcNote = acNote;
            AcInternalNote = acInternalNote;
            AdFieldDA = adFieldDA;
            AdFieldDB = adFieldDB;
            AnFieldNA = anFieldNA;
            AnFieldNB = anFieldNB;
            AcSignature = acSignature;
            AcFieldSD = acFieldSD;
            AcFieldSC = acFieldSC;
            AdFieldDC = adFieldDC;
            AdFieldDD = adFieldDD;
            AcFieldSG = acFieldSG;
            AcFieldSH = acFieldSH;
            AcFieldSE = acFieldSE;
        }
    }
}