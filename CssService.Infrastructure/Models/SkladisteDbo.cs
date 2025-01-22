namespace CssService.Infrastructure.Models
{
    public class SkladisteDbo
    {
        public string AcIssuer { get; set; }
        public string AcReceiver { get; set; }

        public SkladisteDbo(string acIssuer, string acReceiver)
        {
            AcIssuer = acIssuer;
            AcReceiver = acReceiver;
        }
    }
}