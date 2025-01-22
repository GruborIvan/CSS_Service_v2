namespace CssService.Domain.Models
{
    public class Masina
    {
        public int Id { get; set; }
        public string NazivMasine { get; set; }
        public string SerijskiBroj { get; set; }
        public string GarancijaOd {  get; set; }
        public string GarancijaDo {  get; set; }

        public Masina() { }
    }
}