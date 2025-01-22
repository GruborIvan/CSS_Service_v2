using AutoMapper;
using CssService.API.Models.ServiceDTOs;
using CssService.Domain.Commands.Servis;
using CssService.Domain.Queries.Servisi;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CssService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<ServiceController> _logger;

        public ServiceController(IMapper mapper,
                                 IMediator mediator,
                                 ILogger<ServiceController> logger)
        {
            _mapper = mapper;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ServiceReturnDto>> PostService([FromBody] ServicePostDto servicePostDto)
        {
            try
            {
                _logger.LogInformation($"\n\n {JsonConvert.SerializeObject(servicePostDto)}");
                _logger.LogInformation($"Received POST request for PostService/AllDataService.");

                var insertServisiCommand = _mapper.Map<InsertServisCommand>(servicePostDto);

                await _mediator.Send(insertServisiCommand);

                _logger.LogInformation($"Retrieving data for PostService/AllDataService.");

                var allData = await _mediator.Send(new GetAllServiceData(acDocTypeService: servicePostDto.DocType));

                var allDataDto = _mapper.Map<ServiceReturnDto>(allData);

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
