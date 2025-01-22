using AutoMapper;
using CssService.API.Models.ServisniNalog;
using CssService.Domain.Commands.UpdateNarudzbina;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NarudzbineController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<NarudzbineController> _logger;

        public NarudzbineController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<NarudzbineController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PostServisniNalog([FromBody] ServisniNalogDto servisniNalog)
        {
            try
            {
                var servisniNalogCommand = _mapper.Map<UpdateNarudzbinaCommand>(servisniNalog);
                await _mediator.Send(servisniNalogCommand);

                return Ok(servisniNalog.AcKey);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
            
        }
    }
}