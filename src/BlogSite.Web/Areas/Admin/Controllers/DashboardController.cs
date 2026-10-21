using BlogSite.Application.Features.BlogPosts.Queries;
using BlogSite.Application.Features.BlogSites.Queries;
using BlogSite.Application.Features.Categories.Queries;
using BlogSite.Application.Features.Tags.Queries;
using BlogSite.Application.Features.Users.Queries;
using BlogSite.Domain.Enums;
using BlogSite.Web.Areas.Admin.Models;
using Cortex.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class DashboardController(ILogger<DashboardController> logger, 
        IMediator mediator) : Controller
    {
        private readonly ILogger<DashboardController> _logger = logger;
        private readonly IMediator _mediator = mediator;
        
        public async Task<IActionResult> Index()
        {
            var blogPostQuery = new GetTotalBlogPostAdminQuery { Status = Status.Active };
            var categoryQuery = new GetTotalCategoriesAdminQuery { Status = Status.Active };
            var tagQuery = new GetTotalTagsAdminQuery { Status = Status.Active };
            var blogSiteQuery = new GetTotalBlogSiteAdminQuery { Status = Status.Active };
            var userQuery = new GetTotalUserAdminQuery { IsBlocked = false };
            var model = new DashboardModel();

            try
            {
                model.TotalPosts = await _mediator.SendQueryAsync<GetTotalBlogPostAdminQuery, int>(blogPostQuery);
                model.TotalCategories = await _mediator.SendQueryAsync<GetTotalCategoriesAdminQuery, int>(categoryQuery);
                model.TotalTags = await _mediator.SendQueryAsync<GetTotalTagsAdminQuery, int>(tagQuery);
                model.TotalBlogSites = await _mediator.SendQueryAsync<GetTotalBlogSiteAdminQuery, int>(blogSiteQuery);
                model.TotalUsers = await _mediator.SendQueryAsync<GetTotalUserAdminQuery, int>(userQuery);
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching dashboard data");

                return View(model);
            }
        }
    }
}
