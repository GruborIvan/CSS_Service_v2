using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public StatusController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<StatusController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<StatusDto>>> GetStatuses()
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetStatuses.");

                var statuses = await _mediator.Send(new GetStatuses());

                return Ok(_mapper.Map<IEnumerable<StatusDto>>(statuses));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
