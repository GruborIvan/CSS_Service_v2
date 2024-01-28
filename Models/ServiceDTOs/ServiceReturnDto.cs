namespace CSS_Service.API.Models.ServiceDTOs
{
    public class ServiceReturnDto
    {
        public IEnumerable<SubjectDto> Subjekti { get; set; }
        public IEnumerable<IdentDto> Identi { get; set; }
        public IEnumerable<CityDto> Gradovi { get; set; }
        public IEnumerable<ServiceDto> Servisi { get; set; }
        public IEnumerable<NarudzbinaItemDto> StavkeServisa { get; set; }
        public IEnumerable<MasinaDto> Masine { get; set; }
        public IEnumerable<MasinaKorisnikDto> MasinaKorisnik { get; set; }
        public IEnumerable<ContactPersonDto> ContactPersons { get; set; }
    }
}