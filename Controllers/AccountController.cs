using FileSharing.Data;
using FileSharing.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileSharing.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        //inject SignInManager And UserManager
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
        }

        // Get UserId
        private string UserId
        {
            get { return User.FindFirstValue(ClaimTypes.NameIdentifier); }
        }

        public IActionResult Index()
        {
            return View();
        }
        // view login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // login action mehtod
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl)
        {

            if (ModelState.IsValid)
            {
                var existinguser = await _userManager.FindByEmailAsync(model.Email);
                if (existinguser == null)
                {
                    TempData["Error"] = "Invalid Email Or Password";
                    return View(model);
                }
                if (!existinguser.IsBlocked)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
                    if (result.Succeeded)
                    {

                        // return request Download
                        if (!string.IsNullOrEmpty(returnurl))
                        {
                            return LocalRedirect(returnurl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
                }
                else
                {
                    TempData["Error"] = "This Account Has Been Blocked";
                }
            }

            return View(model);
        }

        // view signup
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //sinup action method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // logout action method
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //Login with Facebook and Google
        public IActionResult ExternalLogin(string provider)
        {

            var Properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, "/Account/ExternalResponse");
            return Challenge(Properties, provider);
        }
        public async Task<IActionResult> ExternalResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["Message"] = "";
                return RedirectToAction("Login");
            };
            var loginresult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (!loginresult.Succeeded)
            {
                // Create new local account
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                var lastname = info.Principal.FindFirstValue(ClaimTypes.Surname);

                var UserToCreate = new ApplicationUser
                {
                    FirstName = firstname,
                    LastName = lastname,
                    UserName = email,
                    Email = email
                };
                var CreateResult = await _userManager.CreateAsync(UserToCreate);
                if (CreateResult.Succeeded)
                {
                    var exloginResult = await _userManager.AddLoginAsync(UserToCreate, info);
                    if (exloginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(UserToCreate, false, info.LoginProvider);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        await _userManager.DeleteAsync(UserToCreate);
                    }
                }
                return RedirectToAction("Login");
            }

            // blocked user

            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var existinguser = await _userManager.FindByEmailAsync(email);
                if (existinguser == null)
                {
                    TempData["Error"] = "Invalid Email Or Password";
                    return RedirectToAction("Login");
                }
                if (existinguser.IsBlocked)
                {
                    await _signInManager.SignOutAsync();
                    TempData["Error"] = "This Account Has Been Blocked";
                    return RedirectToAction("Login");
                }

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> info()
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }
            var userdata = new InfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return View(userdata);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> info(InfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(UserId);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["Message"] = "operation accomplished successfully";
                        return RedirectToAction("info");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Changepassword()
        {
            return View();
        }
        //change password
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Changepassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentuser = await _userManager.GetUserAsync(User);
                if (currentuser != null)
                {
                    var changepassword = await _userManager.ChangePasswordAsync(currentuser, model.CurrentPassword, model.NewPassword);
                    if (changepassword.Succeeded)
                    {
                        TempData["messagepassword"] = "Password changed successfully Please login again";
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }
                    foreach (var error in changepassword.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }
    }
}
