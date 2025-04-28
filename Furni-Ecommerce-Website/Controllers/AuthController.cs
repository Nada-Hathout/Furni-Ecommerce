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
        public AuthController(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IUserService userService)
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
            {
                return View(model);
            }

            try
            {
                // Create user through the service
                var user = userService.Register(model);

                // Attempt to create the user
                var creationResult = await userManager.CreateAsync(user, model.Password);

                if (!creationResult.Succeeded)
                {
                    AddErrorsToModelState(creationResult.Errors);
                    return View(model);
                }

                // Assign default "User" role
                var roleResult = await userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    await HandleFailedRoleAssignment(user, roleResult);
                    return View(model);
                }

                // Add standard claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim("UserId", user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        };

                var claimsResult = await userManager.AddClaimsAsync(user, claims);
                if (!claimsResult.Succeeded)
                {
                    await HandleFailedClaimsAssignment(user, claimsResult);
                    return View(model);
                }

                // Sign in and set session
                await signInManager.SignInAsync(user, isPersistent: false);
                HttpContext.Session.SetString("UserId", user.Id);

                TempData["SuccessMessage"] = "Registration successful! Welcome!";
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx)
            {
                HandleDuplicateEntryErrors(sqlEx, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "An unexpected error occurred during registration. Please try again.");
                // Log the exception here
            }

            return View(model);
        }

        // Helper methods
        private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private async Task HandleFailedRoleAssignment(ApplicationUser user, IdentityResult roleResult)
        {
            AddErrorsToModelState(roleResult.Errors);
            // Clean up by deleting the user if role assignment fails
            await userManager.DeleteAsync(user);
        }

        private async Task HandleFailedClaimsAssignment(ApplicationUser user, IdentityResult claimsResult)
        {
            AddErrorsToModelState(claimsResult.Errors);
            // Clean up by deleting the user if claims assignment fails
            await userManager.DeleteAsync(user);
        }

        private void HandleDuplicateEntryErrors(SqlException sqlEx, RegisterViewModel model)
        {
            if (sqlEx.Number == 2627 || sqlEx.Number == 2601)
            {
                if (sqlEx.Message.Contains("IX_AspNetUsers_PhoneNumber"))
                {
                    ModelState.AddModelError(nameof(model.PhoneNumber),
                        "This phone number is already in use.");
                }
                else if (sqlEx.Message.Contains("IX_AspNetUsers_Email"))
                {
                    ModelState.AddModelError(nameof(model.Email),
                        "This email is already in use.");
                }
                else if (sqlEx.Message.Contains("IX_AspNetUsers_UserName"))
                {
                    ModelState.AddModelError(nameof(model.UserName),
                        "This username is already taken.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "The information you provided conflicts with an existing account.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty,
                    "A database error occurred during registration.");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Add custom claims if needed
            var claims = new List<Claim>
    {
        new Claim("UserId", user.Id),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName)
    };

            // Add any existing user claims
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            await signInManager.SignInWithClaimsAsync(
                user,
                model.RememberMe,
                claims);

            HttpContext.Session.SetString("UserId", user.Id);

            // Handle return URL for redirect after login
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}