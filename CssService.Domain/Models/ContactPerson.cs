namespace CssService.Domain.Models
{
    public class ContactPerson
    {
        public string AcSubject { get; set; }
        public int AnNo { get; set; }
        public string AcName { get; set; }
        public string AcSurname { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }

        public ContactPerson() { }
    }
}