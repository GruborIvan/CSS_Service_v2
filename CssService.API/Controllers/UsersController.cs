using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<UsersController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetUsers.");

                var users = await _mediator.Send(new GetUsers());

                var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);

                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}