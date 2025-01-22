using AutoMapper;
using CssService.API.Models;
using CssService.Domain.Queries.Skladiste;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkladisteController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<SkladisteController> _logger;

        public SkladisteController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<SkladisteController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SkladisteDto>> GetSkladisteByDocType(string docType)
        {
            try
            {
                _logger.LogInformation($"Received GET request for GetSkladisteByDocType.");

                var skladiste = await _mediator.Send(new GetSkladisteByDocType(docType));

                var skladisteDto = _mapper.Map<SkladisteDto>(skladiste);

                return Ok(skladisteDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}