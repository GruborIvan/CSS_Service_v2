namespace CssService.Domain.Models
{
    public class Ident
    {
        public string AcIdent { get; set; }
        public string AcName { get; set; }
        public double AnRTPrice { get; set; }
        public double AnSalePrice { get; set; }
        public string AcUm {  get; set; }
        public string AcVatCode { get; set; }
        public double AnVat {  get; set; }
        public double AnUMToUM2 { get; set; }
        public string AcWarehouse { get; set; }
        public double AnStock {  get; set; }

        public Ident() { }
    }
}