namespace CssService.Infrastructure.Models
{
    public class ReferentDbo
    {
        public string AcName { get; set; }
        public string AcMiddle {  get; set; }
        public string AcSurname { get; set; }
        public int AnUserId { get; set; }

        public ReferentDbo(string acName, string acMiddle, string acSurname, int anUserId)
        {
            AcName = acName;
            AcMiddle = acMiddle;
            AcSurname = acSurname;
            AnUserId = anUserId;
        }
    }
}