using Microsoft.AspNetCore.Mvc;

namespace FileSharing.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
