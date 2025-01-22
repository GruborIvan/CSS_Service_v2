namespace CssService.Infrastructure.Models
{
    public class MasinaDbo
    {
        public int Id { get; set; }
        public string Naziv_masine { get; set; }
        public string Serijski_broj { get; set; }
        public DateTime Garancija_od {  get; set; }
        public DateTime Garancija_do {  get; set; }

        public MasinaDbo(int id, string naziv_masine, string serijski_broj, DateTime garancija_od, DateTime garancija_do)
        {
            Id = id;
            Naziv_masine = naziv_masine;
            Serijski_broj = serijski_broj;
            Garancija_od = garancija_od;
            Garancija_do = garancija_do;
        }
    }
}