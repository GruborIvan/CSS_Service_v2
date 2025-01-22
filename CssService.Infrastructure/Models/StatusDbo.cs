namespace CssService.Infrastructure.Models
{
    public class StatusDbo
    {
        public string AcStatus {  get; set; }
        public string AcName { get; set; }

        public StatusDbo(string acStatus, string acName)
        {
            AcStatus = acStatus;
            AcName = acName;
        }
    }
}