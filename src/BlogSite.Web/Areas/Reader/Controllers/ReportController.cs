using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Reports.Commands;
using BlogSite.Domain.Entities;
using BlogSite.Web.Areas.Reader.Models;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class ReportController(IMediator mediator, IMapper mapper,
        ILogger<ReportController> logger, IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ReportController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;


        [HttpPost]
        public async Task<IActionResult> Create(ReportModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = _mapper.Map<AddReportCommand>(model);
                command.CreatedAt = _serverTime.DateTime;

                await _mediator.SendCommandAsync<AddReportCommand, Report>(command);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error occourd while creating report");
                return BadRequest(ex);
            }
        }
    }
}
