using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
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

                var users = await _mediator.Send(new GetCities());

                var citiesDto = _mapper.Map<IEnumerable<CityDto>>(users);

                return Ok(citiesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
