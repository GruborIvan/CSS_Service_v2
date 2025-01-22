namespace CssService.Infrastructure.Models
{
    public class CityDbo
    {
        public string AcPost {  get; set; }
        public string AcName { get; set; }
        public string AcRegion { get; set; }

        public CityDbo(string acPost, string acName, string acRegion)
        {
            AcPost = acPost;
            AcName = acName;
            AcRegion = acRegion;
        }
    }
}