using Microsoft.AspNetCore.Mvc;

namespace FileSharing.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
