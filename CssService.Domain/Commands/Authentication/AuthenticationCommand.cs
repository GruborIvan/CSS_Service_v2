using MediatR;

namespace CssService.Domain.Commands.Authentication
{
    public class AuthenticationCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public AuthenticationCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}