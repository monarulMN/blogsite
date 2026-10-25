using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Comments.Commands;
using BlogSite.Domain.Entities;
using BlogSite.Web.Areas.Reader.Models.Comment;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Reader.Controllers
{
    [Area("Reader")]
    public class CommentController(IMediator mediator, IMapper mapper,
        ILogger<CommentController> logger, IServerTime serverTime) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CommentController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCommentModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var command = _mapper.Map<AddCommentCommand>(model);
                command.CreateAt = _serverTime.DateTime;

                var result = await _mediator.SendCommandAsync<AddCommentCommand, Comment>(command);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a comment.");

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
