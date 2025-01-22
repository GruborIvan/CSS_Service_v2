namespace CssService.Infrastructure.Models
{
    public class SubjectDbo
    {
        public string AcSubject { get; set; }
        public string AcName2 { get; set; }
        public string AcPost { get; set; }
        public string AcAddress { get; set; }

        public SubjectDbo(string acSubject, string acName2, string acPost, string acAddress)
        {
            AcSubject = acSubject;
            AcName2 = acName2;
            AcPost = acPost;
            AcAddress = acAddress;
        }
    }
}