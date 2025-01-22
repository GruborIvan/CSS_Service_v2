namespace CssService.Infrastructure.Models
{
    public class MasinaKorisnikDbo
    {
        public int Masina_id { get; set; }
        public string Subjekt_id { get; set; }

        public MasinaKorisnikDbo(int masina_id, string subjekt_id)
        {
            Masina_id = masina_id;
            Subjekt_id = subjekt_id;
        }
    }
}