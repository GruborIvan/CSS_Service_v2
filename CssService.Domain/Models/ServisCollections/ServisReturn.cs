namespace CssService.Domain.Models.ServisCollections
{
    public class ServisReturn
    {
        public IEnumerable<Subject> Subjekti { get; set; }
        public IEnumerable<Ident> Identi { get; set; }
        public IEnumerable<City> Gradovi { get; set; }
        public IEnumerable<Servis> Servisi { get; set; }
        public IEnumerable<NarudzbinaItem> StavkeServisa { get; set; }
        public IEnumerable<Masina> Masine { get; set; }
        public IEnumerable<MasinaKorisnik> MasinaKorisnik { get; set; }
        public IEnumerable<ContactPerson> ContactPersons { get; set; }
    }
}