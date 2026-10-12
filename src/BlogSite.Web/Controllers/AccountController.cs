using Microsoft.AspNetCore.Mvc;

namespace BlogSite.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
