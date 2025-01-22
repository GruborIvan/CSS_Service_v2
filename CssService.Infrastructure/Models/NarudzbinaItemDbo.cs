namespace CssService.Infrastructure.Models
{
    public class NarudzbinaItemDbo
    {
        public string AcKey { get; set; }
        public int AnNo { get; set; }
        public string AcIdent { get; set; }
        public string AcName { get; set; }
        public decimal AnRTPrice { get; set; }
        public decimal AnSalePrice { get; set; }
        public string AcUM {  get; set; }
        public string AcVATCode { get; set; }
        public double AnVAT {  get; set; }
        public decimal AnQty { get; set; }
        public string AcUM2 { get; set; }

        public NarudzbinaItemDbo(string acKey, int anNo, string acIdent, string acName, decimal anRTPrice, decimal anSalePrice, string acUM, string acVATCode, double anVAT, decimal anQty, string acUM2)
        {
            AcKey = acKey;
            AnNo = anNo;
            AcIdent = acIdent;
            AcName = acName;
            AnRTPrice = anRTPrice;
            AnSalePrice = anSalePrice;
            AcUM = acUM;
            AcVATCode = acVATCode;
            AnVAT = anVAT;
            AnQty = anQty;
            AcUM2 = acUM2;
        }
    }
}