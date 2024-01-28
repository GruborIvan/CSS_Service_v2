using AutoMapper;
using CSS_Service.API.Models;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public IdentController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<IdentController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{docTypeService}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<IdentDto>>> GetAllIdents(string docTypeService)
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetAllIdents.");

                var idents = await _mediator.Send(new GetAllIdents(docTypeService));

                var identsDtos = _mapper.Map<IEnumerable<IdentDto>>(idents);

                return Ok(identsDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
