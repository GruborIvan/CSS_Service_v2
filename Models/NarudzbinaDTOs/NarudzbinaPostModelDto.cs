namespace CSS_Service.API.Models.NarudzbinaDTOs
{
    public class NarudzbinaPostModelDto
    {
        public IEnumerable<NarudzbinaPostDto> Narudzbine {  get; set; }
        public IEnumerable<NarudzbinaItemPostDto> StavkeNarudzbine { get; set; }

        public string DocType { get; set; }
        public string DocTypeService { get; set; }
    }
}