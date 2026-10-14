using BlogSite.Infrastructure.Identity;
using BlogSite.Web.Areas.Admin.Models;
using Cortex.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin, Support")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public RoleController(RoleManager<ApplicationRole> roleManager, 
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new List<RoleWithUserCountViewModel>();

            foreach(var role in roles)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                model.Add(new RoleWithUserCountViewModel
                {
                    RoleId = role.Id.ToString(),
                    RoleName = role.Name,
                    UserCount = usersInRole.Count
                });
            }
            return View(model);
        }
    }
}
