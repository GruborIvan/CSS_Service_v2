using AutoMapper;
using CSS_Service.API.Models.NarudzbinaDTOs;
using CSS_Service.API.Models.ServiceDTOs;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public ServiceController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<ServiceController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceReturnDto>> PostService()
        {
            try
            {
                _logger.LogInformation($"Received POST request for PostService/AllDataService.");

                // INSERT LOGIC.

                _logger.LogInformation($"Retrieving data for PostService/AllDataService.");

                var allData = await _mediator.Send(new GetAllServiceData("0100"));

                var allDataDto = _mapper.Map<ServiceReturnDto>(allData);

                return Ok(allDataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}