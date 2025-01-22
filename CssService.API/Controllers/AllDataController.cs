using AutoMapper;
using CssService.API.Models.NarudzbinaDTOs;
using CssService.Domain.Commands.Narudzbina;
using CssService.Domain.Queries.Narudzbine;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllDataController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public AllDataController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<AllDataController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NarudzbinaReturnDto>> PostNarudzbina([FromBody] NarudzbinaPostModelDto content)
        {
            try
            {
                _logger.LogInformation($"\n\n {JsonConvert.SerializeObject(content)}");
                _logger.LogInformation($"Received POST request for PostNarudzbina/AllData.");

                var narudzbinaPostCommand = _mapper.Map<InsertNarudzbineDataCommand>(content);

                await _mediator.Send(narudzbinaPostCommand);

                _logger.LogInformation($"Retrieving data for PostNarudzbina/AllData.");

                var allData = await _mediator.Send(new GetAllNarudzbinaData(content.DocType, content.DocTypeService));

                var allDataDto = _mapper.Map<NarudzbinaReturnDto>(allData);

                return Ok(allDataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                throw;
            }
        }
    }
}
