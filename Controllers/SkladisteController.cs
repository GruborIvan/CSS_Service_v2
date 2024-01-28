using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkladisteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public SkladisteController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<SkladisteController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{docType}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SkladisteDto>> GetSkladisteByDocType(string docType)
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetSkladisteByDocType.");

                var skladiste = await _mediator.Send(new GetSkladisteByDocType(docType));

                var skladisteDto = _mapper.Map<IEnumerable<SkladisteDto>>(skladiste);

                return Ok(skladisteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
