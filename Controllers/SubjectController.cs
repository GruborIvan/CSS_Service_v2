using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public SubjectController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<SubjectController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<SubjectDto>> GetAllSubjects()
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetAllSubjects.");

                var subjects = await _mediator.Send(new GetAllSubjects());

                return Ok(_mapper.Map<IEnumerable<SubjectDto>>(subjects));
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}