using BlogSite.Application.Contracts;
using BlogSite.Application.Features.Librarys.Commands;
using BlogSite.Application.Features.Librarys.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Reader.Models;
using BlogSite.Web.Areas.Reader.Models.Library;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Reader.Controllers
{
    [Area("Reader"), Authorize(Roles ="Reader")]
    public class LibraryController(IMediator mediator, IMapper mapper, 
        ILogger<LibraryController> logger, IServerTime serverTime, 
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<LibraryController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        [Route("user/library")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var query = new GetLibraryQuery { ApplicationUserId = user.Id };
                var result = await _mediator.SendQueryAsync<GetLibraryQuery, IList<Library>>(query);

                var model = new ListPostLibraryModel();

                model.Posts = result.Select(x => new LibraryPostViewModel
                    {
                        Id = x.BlogPost.Id,
                        Title = x.BlogPost.Title
                    }).ToList();

                return View(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user libraries.");
                return RedirectToAction("Index", "Home", new { area = "Reader" });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddLibraryModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var checkCommand = _mapper.Map<CheckLibraryQuery>(model);
                var exists = await _mediator.SendQueryAsync<CheckLibraryQuery, bool>(checkCommand);

                if (exists)
                    return Ok();

                var command = _mapper.Map<AddLibraryCommand>(model);
                command.CreatedAt = _serverTime.DateTime;

                var result = await _mediator.SendCommandAsync<AddLibraryCommand, Library>(command);

                return Ok(result);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a library.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid blogpostId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var command = _mapper.Map<DeleteLibraryCommand>(user);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "An error occurred while retrieving user libraries.");
                return BadRequest();
            }
        }
    }
}
