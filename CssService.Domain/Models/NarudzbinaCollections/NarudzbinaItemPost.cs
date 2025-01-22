namespace CssService.Domain.Models.NarudzbinaCollections
{
    public class NarudzbinaItemPost
    {
        public int AnNo { get; set; }
        public string AcKey { get; set; } = null;
        public string AcIdent { get; set; }
        public string AcName { get; set; }
        public double AnRTPrice { get; set; }
        public double AnSalePrice { get; set; }
        public string AcUm { get; set; }
        public string AcVatCode { get; set; }
        public double AnVat { get; set; }
        public string AnUMToUM2 { get; set; }
        public double AnQty { get; set; }
        public long AcKeyLocal { get; set; }

        public NarudzbinaItemPost()
        {

        }
    }
}