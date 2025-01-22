using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries.Cities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public CitiesController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<CitiesController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetCities.");

                var users = await _mediator.Send(new GetAllCities());

                var citiesDto = _mapper.Map<IEnumerable<CityDto>>(users);

                return Ok(citiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}
