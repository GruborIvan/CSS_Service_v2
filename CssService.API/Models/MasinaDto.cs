namespace CssService.API.Models
{
    public record MasinaDto(
        int Id,
        string NazivMasine,
        string SerijskiBroj,
        string GarancijaOd,
        string GarancijaDo
    );
}