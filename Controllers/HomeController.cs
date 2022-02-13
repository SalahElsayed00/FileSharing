using FileSharing.Data;
using FileSharing.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;

namespace FileSharing.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            var model = _context.Uploads.OrderByDescending(e => e.DownloadCount)
                 .Select(e => new UploadViewModel
                 {
                     Id = e.Id,
                     FileName = e.FileName,
                     OriginalName = e.OriginalFileName,
                     ContentType = e.ContentType,
                     Size = e.Size,
                     DownloadCount = e.DownloadCount,
                     UploadDate = e.UploadDate
                 }).Take(3);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult SetCulture(string Lang, string returnurl = null)
        {
            if (!string.IsNullOrEmpty(Lang))
            {
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(Lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(5) });
            }

            if (!string.IsNullOrEmpty(returnurl))
            {
                return LocalRedirect(returnurl);
            }
            return RedirectToAction("Index");
        }
    }
}
