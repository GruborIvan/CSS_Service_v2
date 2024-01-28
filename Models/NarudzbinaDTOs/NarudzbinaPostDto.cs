namespace CSS_Service.API.Models.NarudzbinaDTOs
{
    public class NarudzbinaPostDto
    {
        public string AcReciever { get; set; }
        public DateTime AdDate { get; set; }
        public int UserID { get; set; }
        public string DocType { get; set; }
        public long AcKeyLocal { get; set; }
    }
}