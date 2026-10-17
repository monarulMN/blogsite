using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles ="Admin, Support")]
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
