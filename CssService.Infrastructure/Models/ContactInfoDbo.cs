namespace CssService.Infrastructure.Models
{
    public class ContactInfoDbo
    {
        public string AcSubject { get; set; }
        public int AnNo { get; set; }
        public string AcPhone { get; set; }
        public string AcType { get; set; }

        public ContactInfoDbo(string acSubject, int anNo, string acPhone, string acType)
        {
            AcSubject = acSubject;
            AnNo = anNo;
            AcPhone = acPhone;
            AcType = acType;
        }
    }
}