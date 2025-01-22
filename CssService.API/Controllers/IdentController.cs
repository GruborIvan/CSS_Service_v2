using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries.Idents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<IdentController> _logger;

        public IdentController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<IdentController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<IdentDto>>> GetAllIdents(string docTypeService)
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetAllIdents.");

                var idents = await _mediator.Send(new GetIdentsByDocType(docTypeService));

                var identsDtos = _mapper.Map<IEnumerable<IdentDto>>(idents);

                return Ok(identsDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}