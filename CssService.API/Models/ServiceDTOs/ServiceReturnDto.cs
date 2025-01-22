using System.Text.Json.Serialization;

namespace CssService.API.Models.ServiceDTOs
{
    public class ServiceReturnDto
    {
        [JsonPropertyName("subjekti")]
        public IEnumerable<SubjectDto> Subjekti { get; set; }

        [JsonPropertyName("identi")]
        public IEnumerable<IdentDto> Identi { get; set; }

        [JsonPropertyName("gradovi")]
        public IEnumerable<CityDto> Gradovi { get; set; }

        [JsonPropertyName("servisi")]
        public IEnumerable<ServiceDto> Servisi { get; set; }

        [JsonPropertyName("stavkeServisa")]
        public IEnumerable<NarudzbinaItemDto> StavkeServisa { get; set; }

        [JsonPropertyName("masine")]
        public IEnumerable<MasinaDto> Masine { get; set; }

        [JsonPropertyName("masinaKorisnik")]
        public IEnumerable<MasinaKorisnikDto> MasinaKorisnik { get; set; }

        [JsonPropertyName("contactPersons")]
        public IEnumerable<ContactPersonDto> ContactPersons { get; set; }
    }
}