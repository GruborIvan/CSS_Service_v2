namespace CssService.Domain.Models.NarudzbinaCollections
{
    public class NarudzbinaPost
    {
        public string AcReciever { get; set; }
        public DateTime AdDate { get; set; }
        public int UserID { get; set; }
        public string DocType { get; set; }
        public long AcKeyLocal { get; set; }

        public NarudzbinaPost() { }
    }
}