namespace CssService.API.Models
{
    public record IdentDto(
        string AcIdent,
        string AcName,
        double AnRTPrice,
        double AnSalePrice,
        string AcUm,
        string AcVatCode,
        double AnVat,
        double AnUMToUM2,
        string AcWarehouse,
        double AnStock
    );
}