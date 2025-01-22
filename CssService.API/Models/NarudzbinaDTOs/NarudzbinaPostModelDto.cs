using System.Text.Json.Serialization;

namespace CssService.API.Models.NarudzbinaDTOs
{
    public class NarudzbinaPostModelDto
    {
        [JsonPropertyName("narudzbine")]
        public List<NarudzbinaPostDto> Narudzbine {  get; set; }

        [JsonPropertyName("stavkeNarudzbine")]
        public List<NarudzbinaItemPostDto> StavkeNarudzbine { get; set; }

        [JsonPropertyName("docType")]
        public string DocType { get; set; }

        [JsonPropertyName("docTypeService")]
        public string DocTypeService { get; set; }

        public NarudzbinaPostModelDto()
        {
            Narudzbine = new List<NarudzbinaPostDto>();
            StavkeNarudzbine = new List<NarudzbinaItemPostDto>();
        }
    }
}