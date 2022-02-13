using ClosedXML.Excel;
using FileSharing.Areas.Admin.Models;
using FileSharing.Areas.Admin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly IXLWorkbook _xLWorkbook;

        public UserController(IUserService userService, IXLWorkbook xLWorkbook)
        {
            this._userService = userService;
            this._xLWorkbook = xLWorkbook;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAll().ToListAsync();
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Blocked()
        {
            var result = await _userService.GetBlokedUsers().ToListAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Search(TermViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Search(model.Term).ToListAsync();
                return View("Index", result);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Block(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var result = await _userService.ToggleBlockUserAsync(userId);
                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
                return RedirectToAction("Index");
            }
            TempData["Error"] = "User Id Is Not Found";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UserCount()
        {
            var totaluserscount = await _userService.UserRegistrationCountAsync();
            var month = DateTime.Today.Month;
            var monthuserscount = await _userService.UserRegistrationCountAsync(month);
            return Json(new { total = totaluserscount, month = monthuserscount });
        }

        public async Task<IActionResult> ExportToExel()
        {
            string contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string filename = "users.xlsx";

            var result = await _userService.GetAll().ToListAsync();

            var worksheet = _xLWorkbook.Worksheets.Add("All Users");

            worksheet.Cell(1, 1).Value = "First Name";
            worksheet.Cell(1, 2).Value = "Last Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Created Date";

            for (int i = 1; i < result.Count; i++)
            {
                var row = i + 1;
                worksheet.Cell(row, 1).Value = result[i - 1].FirstName;
                worksheet.Cell(row, 2).Value = result[i - 1].LastName;
                worksheet.Cell(row, 3).Value = result[i - 1].Email;
                worksheet.Cell(row, 4).Value = result[i - 1].CreateDate;
            }

            using (var ms = new MemoryStream())
            {
                _xLWorkbook.SaveAs(ms);
                return File(ms.ToArray(), contenttype, filename);
            }
        }
    }
}
