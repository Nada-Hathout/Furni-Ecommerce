using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Furni_Ecommerce_Website.Controllers
{
    public class AuthController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        public IUserService userService;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = userService.Register(model);

            try
            {
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                   await  userManager.AddToRoleAsync(user, "User");
                    var claims = new List<Claim>
                    {
                       new Claim(ClaimTypes.Name, user.UserName),
                       new Claim ("UserId", user.Id)
                    };
                    await userManager.AddClaimsAsync(user, claims);

                    await signInManager.SignInAsync(user, isPersistent: false);
                    TempData["SuccessMessage"] = "Registration successful! Please log in.";

                    return RedirectToAction("Login", "Auth");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (DbUpdateException dbEx)
            {

                if (dbEx.InnerException is SqlException sqlEx
                    && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    var msg = sqlEx.Message;
                    if (msg.Contains("IX_AspNetUsers_PhoneNumber"))
                        ModelState.AddModelError(nameof(model.PhoneNumber),
                                                 "This phone number is already in use.");
                    else if (msg.Contains("IX_AspNetUsers_Email"))
                        ModelState.AddModelError(nameof(model.Email),
                                                 "This email is already in use.");
                    else
                        ModelState.AddModelError(string.Empty,
                            "A duplicate value error occurred. Please check your input.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "An error occurred while creating your account. Please try again.");
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByNameAsync(model.UserName);
                if (user != null) 
                {
                 bool found=await userManager.CheckPasswordAsync(user, model.Password);
                    if (found) 
                    {
                        //await signInManager.SignInAsync(user, isPersistent: model.RememberMe);

                        var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim("UserId",user.Id)
                        };
                        var identity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                         await signInManager.SignInAsync(user, isPersistent: model.RememberMe);
                        //await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

                        await userManager.AddClaimAsync(user, new Claim("UserId", user.Id));
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "UserName Or Password Invalid");
                
            }
            return View("Login", model);
        }
    }
}
