using BlogSite.Application.Contracts;
using BlogSite.Infrastructure.Identity;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Web.Areas.Blogger.Controllers
{
    [Area("Blogger"), Authorize(Roles =" Reader")]
    public class BlogController(UserManager<ApplicationUser> userManager,
        IServerTime serverTime,
        ILogger<BlogController> logger,
        IMapper mapper,
        IMediator mediator,
       SignInManager<ApplicationUser> signInManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IServerTime _serverTime = serverTime;
        private readonly ILogger<BlogController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;


        [Authorize(Roles = "Reader")]
        [HttpGet("blog/start-own-blog")]
        public async Task<IActionResult> Index()
        {
            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user.HasBlog)
            {
                ApplicationUser? userWithDetails = await _userManager.Users.Include(u => u.BlogSite)
                    .FirstOrDefaultAsync(u => u.Id == user.Id);

                if(userWithDetails.BlogSite.Status == Domain.Enums.Status.Active)
                {
                    return RedirectToAction("Index", "BlogManagement");
                }
                else
                {
                    return RedirectToAction("Restricted");
                }
            }
            else
                return View();
        }
    }
}
