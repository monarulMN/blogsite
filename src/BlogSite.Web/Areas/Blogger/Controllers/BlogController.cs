using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogSites.Commands;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Extensions;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Admin.Models;
using BlogSite.Web.Areas.Blogger.Models.Blog;
using BlogSite.Web.Codes;
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

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Reader")]
        public async Task<IActionResult> Create(StartOwnBlogModel model)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                model.AuthorId = user.Id;

                if(ModelState.IsValid && !user.HasBlog)
                {
                    var command = _mapper.Map<BlogSiteAddCommand>(model);
                    command.CreatedAt = _serverTime.DateTime;
                    command.UpdatedAt = _serverTime.DateTime;

                    await _mediator.SendCommandAsync<BlogSiteAddCommand, Blogsite>(command);

                    user.HasBlog = true;
                    await _userManager.AddToRoleAsync(user, "Blogger");
                    await _userManager.UpdateAsync(user);
                    await _signInManager.RefreshSignInAsync(user);

                    TempData.Put(Constants.ResponseTemkey,
                        new ResponseModel
                        {
                            Message = " Blog site created successfully",
                            Type = ResponseType.Success
                        });

                    return RedirectToAction(nameof(Index));
                }

                return View("Index", model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating blog site for user {UserId}", User.Identity.Name);

                ModelState.AddModelError(string.Empty, "An error occurred while creating your blog. Please try again later.");

                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Blog site don't create",
                        Type = ResponseType.Danger
                    });

                return View("Index", model);
            }
        }

        [Authorize(Roles = "Blogger")]
        public async Task<IActionResult> Update()
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                var query = new GetBlogSiteByUserQuery { AuthorId = user.Id };
                var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, Blogsite>(query);

                var model = _mapper.Map<UpdateBlogSiteModel>(blogSite);

                return View(model);

            }
            catch(Exception ex)
            {
                TempData.Put(Constants.ResponseTemkey,
                    new ResponseModel
                    {
                        Message = "Blog site data don't found",
                        Type = ResponseType.Danger
                    });

                _logger.LogError(ex, "Error occurred while getting blog site");

                ModelState.AddModelError(string.Empty, 
                    "An error occurred while getting your blog site. Please try again later.");

                return RedirectToAction("Index", "BlogManagement");
            }
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Blogger")]
        public async Task<IActionResult> Update(UpdateBlogSiteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var command = _mapper.Map<BlogSiteUpdateCommand>(model);
                    command.UpdatedAt = _serverTime.DateTime;
                    await _mediator.SendCommandAsync<BlogSiteUpdateCommand, Blogsite>(command);

                    TempData.Put(Constants.ResponseTemkey, 
                        new Models.ResponseModel
                        {
                            Message = "Blog site updated successfully.",
                            Type = Models.ResponseType.Success
                        });

                    return RedirectToAction("Index", "BlogManagement");
                }

                return RedirectToAction("Index", "BlogManagement", model);
            }
            catch (Exception ex)
            {
                TempData.Put(Constants.ResponseTemkey, 
                    new Models.ResponseModel
                    {
                        Message = "Blog site not updated.",
                        Type = Models.ResponseType.Danger
                    });

                _logger.LogError(ex, "Error occurred while updateing blog site");

                ModelState.AddModelError(string.Empty, 
                    "An error occurred while updateing your blog. Please try again later.");

                return RedirectToAction("Index", "BlogManagement", model);
            }
        }

        public IActionResult Restricted()
        {
            return View();
        }
    }
}
