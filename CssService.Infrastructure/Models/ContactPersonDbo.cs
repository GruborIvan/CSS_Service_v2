namespace CssService.Infrastructure.Models
{
    public class ContactPersonDbo
    {
        public string AcSubject { get; set; }
        public int AnNo { get; set; }
        public string AcName { get; set; }
        public string AcSurname { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }

        public ContactPersonDbo(string acSubject, int anNo, string acName, string acSurname)
        {
            AcSubject = acSubject;
            AnNo = anNo;
            AcName = acName;
            AcSurname = acSurname;
        }
    }
}