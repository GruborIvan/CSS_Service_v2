using System.Text.Json.Serialization;

namespace CssService.API.Models.ServiceDTOs
{
    public class ServicePostDto
    {
        [JsonPropertyName("servisi")]
        public List<ServisAddDto> Servisi { get; set; }

        [JsonPropertyName("stavkeServisa")]
        public List<ServisItemAddDto> StavkeServisa { get; set; }

        [JsonPropertyName("docType")]
        public string DocType { get; set; }
        public string DocTypeStockTranfer { get; set; }
        public string DocTypeWarehouseIssuer { get; set; }
        public string DocTypeWarehouseReceiver { get; set; }

        [JsonPropertyName("mail")]
        public string Mail { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}