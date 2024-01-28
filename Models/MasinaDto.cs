namespace CSS_Service.API.Models
{
    public record MasinaDto(
        int Id,
        string NazivMasine,
        string SerijskiBroj,
        string GarancijaOd,
        string GarancijaDo
    );
}