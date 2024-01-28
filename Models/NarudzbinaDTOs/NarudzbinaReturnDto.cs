namespace CSS_Service.API.Models.NarudzbinaDTOs
{
    public class NarudzbinaReturnDto
    {
        public IEnumerable<SubjectDto> Subjekti { get; set; }
        public IEnumerable<IdentDto> Identi { get; set; }
        public IEnumerable<CityDto> Gradovi { get; set; }
        public IEnumerable<StatusDto> Statusi { get; set; }

        public IEnumerable<ReferentDto> Referenti { get; set; }
        public IEnumerable<NarudzbinaDto> Narudzbine { get; set; }
        public IEnumerable<NarudzbinaItemDto> NarudzbinaItemi { get; set; }
        public IEnumerable<MasinaDto> Masine { get; set; }
        public IEnumerable<MasinaKorisnikDto> MasinaKorisnik { get; set; }
    }
}
