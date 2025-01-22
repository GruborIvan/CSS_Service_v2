using System.Text.Json.Serialization;

namespace CssService.API.Models.NarudzbinaDTOs
{
    public class NarudzbinaReturnDto
    {
        [JsonPropertyName("subjekti")]
        public IEnumerable<SubjectDto> Subjekti { get; set; }

        [JsonPropertyName("identi")]
        public IEnumerable<IdentDto> Identi { get; set; }

        [JsonPropertyName("gradovi")]
        public IEnumerable<CityDto> Gradovi { get; set; }

        [JsonPropertyName("statusi")]
        public IEnumerable<StatusDto> Statusi { get; set; }

        [JsonPropertyName("referenti")]
        public IEnumerable<ReferentDto> Referenti { get; set; }

        [JsonPropertyName("narudzbine")]
        public IEnumerable<NarudzbinaDto> Narudzbine { get; set; }

        [JsonPropertyName("narudzbinaItemi")]
        public IEnumerable<NarudzbinaItemDto> NarudzbinaItemi { get; set; }

        [JsonPropertyName("masine")]
        public IEnumerable<MasinaDto> Masine { get; set; }

        [JsonPropertyName("masinaKorisnik")]
        public IEnumerable<MasinaKorisnikDto> MasinaKorisnik { get; set; }
    }
}