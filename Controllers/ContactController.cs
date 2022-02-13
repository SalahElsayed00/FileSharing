using FileSharing.Data;
using FileSharing.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileSharing.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ContactController(ApplicationDbContext context)
        {
            this._context = context;
        }


        private string UserId
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Contacts.Add(new Data.Contact
                {
                    Name = model.Name,
                    Email = model.Email,
                    Subject = model.Subject,
                    Phone = model.Phone,
                    Messege = model.Messege,
                    UserId = UserId
                });
                await _context.SaveChangesAsync();
                TempData["Messege"] = "messege hase been sent sucsesfully";
                return RedirectToAction("Index");
            }
            return View(model);
        }

    }
}
