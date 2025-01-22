namespace CssService.Domain.Interfaces
{
    public interface ISignatureRepository
    {
        Task SaveSignatureAsync(string acKey, string signature);
    }
}