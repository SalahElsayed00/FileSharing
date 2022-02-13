using FileSharing.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Controllers
{
    public class ContactUSController : AdminBaseController
    {
        private readonly IContactUsService _contactUsService;

        public ContactUSController(IContactUsService contactUsService)
        {
            this._contactUsService = contactUsService;
        }
        //[HttpGet]
        public async Task<IActionResult> Index()
        {
            var result =await _contactUsService.GetAll().ToListAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id ,bool status)
        {
             await _contactUsService.ChangeStatusAsync(id,status);
            return RedirectToAction("Index");
        }
    }
}
