using CssService.Domain.Interfaces;
using MediatR;

namespace CssService.Domain.Commands.Authentication
{
    public class AuthenticationCommandHandler : IRequestHandler<AuthenticationCommand, bool>
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationCommandHandler(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<bool> Handle(AuthenticationCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationRepository.AuthenticateToSqlServerAsync(request.Username, request.Password);
        }
    }
}