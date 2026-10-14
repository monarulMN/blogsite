using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Commands;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Codes;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class BlogSiteController(IMediator mediator, 
        IMapper mapper, 
        ILogger<BlogSiteController> logger, 
        IServerTime serverTime ) : Controller
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BlogSiteController> _logger = logger;
        private readonly IServerTime _serverTime = serverTime;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Active(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var activeCommand = new BlogSiteActiveCommand { Id = id };
                await _mediator.SendCommandAsync<BlogSiteActiveCommand, Guid>(activeCommand);

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "BlogSite Active Successfully",
                        Type = ResponseType.Danger
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Failed to Active blog site with Id: {id}");
                TempData.Put(Constants.ResponseTemkey, 
                    new ResponseModel
                    {
                        Message = "Failed To Active Blogsite",
                        Type = ResponseType.Danger
                    });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
