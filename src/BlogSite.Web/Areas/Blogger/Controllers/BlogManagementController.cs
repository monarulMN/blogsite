using BlogSite.Application.Contracts;
using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Application.Features.Comments.Queries;
using BlogSite.Domain.Entities;
using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Blogger.Models;
using Cortex.Mediator;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Blogger.Controllers
{
    [Area("Blogger"), Authorize(Roles = "Blogger")]
    public class BlogManagementController(UserManager<ApplicationUser> userManager,
        IServerTime serverTime,
        ILogger<BlogManagementController> logger,
        IMapper mapper,
        IMediator mediator) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IServerTime _serverTime = serverTime;
        private readonly ILogger<BlogManagementController> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;


        [HttpGet("blog/blog-management")]
        public async Task<IActionResult> Index()
        {
            var author = await _userManager.GetUserAsync(User);

            var blogSiteCommand = new GetBlogSiteByUserQuery { AuthorId = author.Id };
            var blogSite = await _mediator.SendQueryAsync<GetBlogSiteByUserQuery, 
                Blogsite>(blogSiteCommand);

            var categoryCountCommand = new GetCategoryCountByBlogSiteQuery { BlogSiteId = blogSite.Id };
            var categoryCount = await _mediator.SendQueryAsync<GetCategoryCountByBlogSiteQuery, 
                int>(categoryCountCommand);

            var commentCommand = new GetBlogSitesTotalComentsQuery { BlogSiteId = blogSite.Id };
            var (approved, pending) = await _mediator.SendQueryAsync<GetBlogSitesTotalComentsQuery, 
                (int, int)>(commentCommand);

            var postCommand = new GetBlogPostTotalByBlogSiteQuery { BlogSiteId = blogSite.Id };
            var (published, draft) = await _mediator.SendQueryAsync<GetBlogPostTotalByBlogSiteQuery, 
                (int, int)>(postCommand);

            var model = new BlogManagementModel
            {
                BlogSiteSlug = blogSite.Slug,
                TotalCategories = categoryCount,
                ApprovedComments = approved,
                PendingComments = pending,
                PublishedPosts = published,
                DraftPosts = draft
            };


            return View(model);
        }
    }
}
