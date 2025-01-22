using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries.Statuses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<StatusController> _logger;

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

                var statusesDbo = await _mediator.Send(new GetStatuses());

                var statuses = _mapper.Map<IEnumerable<StatusDto>>(statusesDbo);

                return Ok(statuses);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}