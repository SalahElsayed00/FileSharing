using FileSharing.Data;
using FileSharing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileSharing.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {

        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public UploadController(IWebHostEnvironment env, ApplicationDbContext context)
        {

            this._env = env;
            this._context = context;
        }


        // get user id (الي عامل login now) /////  بنجيب اليوزر ايدي الي عامل نسجيل دخول
        private string UserId
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }

        // view all my uploads
        [HttpGet]
        public IActionResult Index()
        {
            var result = _context.Uploads.Where(e => e.UserId == UserId)
                .OrderByDescending(e => e.UploadDate)
                .Select(e => new UploadViewModel
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    OriginalName = e.OriginalFileName,
                    ContentType = e.ContentType,
                    Size = e.Size,
                    UploadDate = e.UploadDate,
                    DownloadCount = e.DownloadCount
                });
            return View(result);
        }

        // view create form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // create method to upload file and save this file in iis server and database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputUpload model)
        {
            if (ModelState.IsValid)
            {
                // save file in upload folder in wwwroot
                var newName = Guid.NewGuid().ToString();
                var extention = Path.GetExtension(model.File.FileName);
                var fileName = string.Concat(newName, extention);
                var root = _env.WebRootPath;
                var path = Path.Combine(root, "Uploads", fileName);

                using (var fs = System.IO.File.Create(path))
                {
                    await model.File.CopyToAsync(fs);
                }
                // save file in database
                await _context.Uploads.AddAsync(new Upload
                {
                    UserId = UserId,
                    FileName = fileName,
                    OriginalFileName = model.File.FileName,
                    ContentType = model.File.ContentType,
                    Size = model.File.Length

                });
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Upload");
            }

            return View(model);
        }

        // view page delete
        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var selectUpload = await _context.Uploads.FindAsync(Id);
            if (selectUpload == null)
            {
                return NotFound();
            }
            if (selectUpload.UserId != UserId)
            {
                return NotFound();
            }
            return View(selectUpload);
        }

        //delete file in my uploads
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string Id)
        {
            var selectUpload = await _context.Uploads.FindAsync(Id);
            if (selectUpload == null)
            {
                return NotFound();
            }
            if (selectUpload.UserId != UserId)
            {
                return NotFound();
            }
            _context.Uploads.Remove(selectUpload);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }

        //implement search function
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Results(string term)
        {

            var model = _context.Uploads
                .Where(e => e.OriginalFileName.Contains(term))
                .OrderByDescending(e => e.DownloadCount)
                .Select(e => new UploadViewModel
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    OriginalName = e.OriginalFileName,
                    ContentType = e.ContentType,
                    Size = e.Size,
                    UploadDate = e.UploadDate,
                    DownloadCount = e.DownloadCount
                });

            return View(model);
        }


        // show all uploads
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Browse(int requiredpage = 1)
        {

            const int PageSize = 3;
            decimal RowsCount = await _context.Uploads.CountAsync();

            var PageCount = Math.Ceiling(RowsCount / PageSize);

            if (requiredpage > PageCount)
            {
                requiredpage = 1;
            }
            requiredpage = requiredpage <= 0 ? 1 : requiredpage;
            int skip_count = (requiredpage - 1) * PageSize;
            var model = await _context.Uploads.OrderByDescending(e => e.DownloadCount)
                .Select(e => new UploadViewModel
                {
                    FileName = e.FileName,
                    OriginalName = e.OriginalFileName,
                    ContentType = e.ContentType,
                    Size = e.Size,
                    DownloadCount = e.DownloadCount,
                    UploadDate = e.UploadDate
                })
                .Skip(skip_count)
                .Take(PageSize)
                .ToListAsync();
            ViewBag.CurrentPage = requiredpage;
            ViewBag.PageCount = PageCount;
            return View(model);
        }

        // Download method 
        [HttpGet]
        public async Task<IActionResult> Download(string Id)
        {
            var selectfile = await _context.Uploads.FirstOrDefaultAsync(e => e.FileName == Id);
            if (selectfile == null)
            {
                return NotFound();
            }
            selectfile.DownloadCount++;
            _context.Update(selectfile);
            await _context.SaveChangesAsync();
            Response.Headers.Add("Expires", DateTime.Now.AddDays(-3).ToLongDateString());
            Response.Headers.Add("Cache-Control", "no-cache");
            var path = "~/Uploads/" + selectfile.FileName;
            return File(path, selectfile.ContentType, selectfile.OriginalFileName);
        }
    }
}
