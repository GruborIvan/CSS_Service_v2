using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries.Subjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<SubjectController> _logger;

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
                throw;
            }
        }
    }
}
