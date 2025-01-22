using System.Text.Json.Serialization;

namespace CssService.API.Models
{
    public class ContactPersonDto
    {
        [JsonPropertyName("acSubject")]
        public string AcSubject { get; set; }
        public int AnNo { get; set; }
        public string AcName { get; set; }
        public string AcSurname { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }

        public ContactPersonDto(string acSubject, int anNo, string acName, string acSurname, string emailAddress, string phoneNo)
        {
            AcSubject = acSubject;
            AnNo = anNo;
            AcName = acName;
            AcSurname = acSurname;
            EmailAddress = emailAddress;
            PhoneNo = phoneNo;
        }
    }
}