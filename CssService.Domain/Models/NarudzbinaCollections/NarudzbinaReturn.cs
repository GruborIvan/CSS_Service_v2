namespace CssService.Domain.Models.NarudzbinaReturn
{
    public class NarudzbinaReturn
    {
        public IEnumerable<Subject> Subjekti { get; set; }
        public IEnumerable<Ident> Identi { get; set; }
        public IEnumerable<City> Gradovi { get; set; }
        public IEnumerable<Status> Statusi { get; set; }
        public IEnumerable<Referent> Referenti { get; set; }
        public IEnumerable<Narudzbina> Narudzbine { get; set; }
        public IEnumerable<NarudzbinaItem> NarudzbinaItemi { get; set; }
        public IEnumerable<Masina> Masine { get; set; }
        public IEnumerable<MasinaKorisnik> MasinaKorisnik { get; set; }
    }
}