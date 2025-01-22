namespace CssService.Infrastructure.Models
{
    public class UserDbo
    {
        public string AcUserId { get; set; }
        public int AnUserChg { get; set; }

        public UserDbo(string acUserId, int anUserChg)
        {
            AcUserId = acUserId;
            AnUserChg = anUserChg;
        }
    }
}