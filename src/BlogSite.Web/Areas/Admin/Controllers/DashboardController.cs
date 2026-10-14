using BlogSite.Application.Features.Users.Queries;
using BlogSite.Web.Areas.Admin.Models;
using Cortex.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IMediator _mediator;

        public DashboardController(ILogger<DashboardController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var userQuery = new GetTotalUserAdminQuery { IsBlocked = false };
            var model = new DashboardModel();

            try
            {
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
