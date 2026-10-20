using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Messages.Commands;
using BlogSite.Application.Features.Messages.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Admin.Models;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class MessageController(IMediator mediator,
        IMapper mapper,
        ILogger<MessageController> logger,
        IServerTime serverTime,
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<MessageController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        [HttpPost]
        public async Task<IActionResult> Create(AddMessageModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                var command = _mapper.Map<MessageAddCommand>(model);
                command.SenderId = user.Id;
                command.CreatedAt = _serverTime.DateTime;

                var reasult = await _mediator.SendCommandAsync<MessageAddCommand, Message>(command);

                return Ok(reasult);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetUserMessagesAsync(Guid userId)
        {
            try
            {
                var query = new GetUserMessagesQuery { UserId = userId };
                var result = await _mediator.SendQueryAsync<GetUserMessagesQuery, IList<Message>>(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to occurred while to data id:{userId}");
                return BadRequest(ModelState);
            }
        }
    }
}
