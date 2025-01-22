namespace CssService.Infrastructure.Models
{
    public class IdentDbo
    {
        public string AcIdent { get; set; }
        public string AcName { get; set; }
        public double AnRTPrice { get; set; }
        public decimal AnSalePrice { get; set;}
        public string AcUM {  get; set; }
        public string AcVATCode { get; set; }
        public decimal AnVAT {  get; set; }
        public decimal AnUMToUM2 { get; set; }
        public string AcWarehouse { get; set; }
        public decimal AnStock { get; set; }

        public IdentDbo(string acIdent, string acName, double anRTPrice, decimal anSalePrice, string acUM, string acVATCode, decimal anVAT, decimal anUMToUM2, string acWarehouse, decimal anStock)
        {
            AcIdent = acIdent;
            AcName = acName;
            AnRTPrice = anRTPrice;
            AnSalePrice = anSalePrice;
            AcUM = acUM;
            AcVATCode = acVATCode;
            AnVAT = anVAT;
            AnUMToUM2 = anUMToUM2;
            AcWarehouse = acWarehouse;
            AnStock = anStock;
        }
    }
}