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

namespace BlogSite.Web.Areas.Reader.Controllers
{
    [Area("Reader"), Authorize(Roles ="Reader")]
    public class SupportController(IMediator mediator, IMapper mapper, 
        ILogger<SupportController> logger, IServerTime serverTime, 
        UserManager<ApplicationUser> userManager) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<SupportController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        [HttpPost, ValidateAntiForgeryToken]
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

                var result = await _mediator.SendCommandAsync<MessageAddCommand, Message>(command);

                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return BadRequest(ModelState);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetUserMessages(Guid userId)
        {
            try
            {
                var query = new GetUserMessagesQuery { UserId = userId };
                var result = await _mediator.SendQueryAsync<GetUserMessagesQuery, 
                    IList<Message>>(query);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ModelState);
            }
        }
    }
}
