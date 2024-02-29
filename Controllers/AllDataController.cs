﻿using AutoMapper;
using CSS_Service.API.Models.NarudzbinaDTOs;
using CSS_Service.Domain.Commands;
using CSS_Service.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CSS_Service.API.Controllers
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
                // CREATE NARUDZBINA.
                _logger.LogInformation($"Received POST request for PostNarudzbina/AllData.");

                var startCreateNarudzbina = _mapper.Map<StartCreateNarudzbina>(content);

                await _mediator.Send(startCreateNarudzbina);

                // GET & RETURN ALL DATA.
                _logger.LogInformation($"Retrieving data for PostNarudzbina/AllData.");

                var allData = await _mediator.Send(new GetAllNarudzbinaData(content.DocType, content.DocTypeService));

                var allDataDto = _mapper.Map<NarudzbinaReturnDto>(allData);

                return Ok(allDataDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {message}", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}