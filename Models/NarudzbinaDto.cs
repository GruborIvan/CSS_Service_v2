namespace CSS_Service.API.Models
{
    public record NarudzbinaDto
    {
        public string AcKey { get; set; }
        public string AcDocType { get; set; }
        public string AcReceiver { get; set; }
        public string AdDate { get; set; }
        public string AdDateValid { get; set; }
        public int AnClerk { get; set; }
        public double AnValue { get; set; }
        public double AnForPay { get; set; }
        public string AcWarehouse { get; set; }
        public string AcStatus { get; set; }
        public long AcKeyLocal { get; set; }
    }
}