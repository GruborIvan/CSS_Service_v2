namespace CssService.Domain.Interfaces
{
    public interface IAuthenticationRepository
    {
        Task<bool> AuthenticateToSqlServerAsync(string username, string password);
    }
}