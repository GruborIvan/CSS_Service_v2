using AutoMapper;
using CssService.API.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CssService.Domain.Commands.Authentication;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AuthenticationController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<IdentController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> PostAuthentication([FromBody] AuthenticationCredentials credentials)
        {
            try
            {
                _logger.LogInformation($"The user with username {credentials.Username} is trying to log in.");

                var credentialsCommand = _mapper.Map<AuthenticationCommand>(credentials);
                var result = await _mediator.Send(credentialsCommand);

                if (result == false)
                    return BadRequest();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }

    }
}
