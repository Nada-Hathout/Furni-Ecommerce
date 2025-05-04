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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Data;

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
                var user = userService.Register(model);
                var creationResult = await userManager.CreateAsync(user, model.Password);

                if (!creationResult.Succeeded)
                {
                    AddErrorsToModelState(creationResult.Errors);
                    return View(model);
                }

                var roleResult = await userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    await HandleFailedRoleAssignment(user, roleResult);
                    return View(model);
                }

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

                // Sign in and set session and make session permanent
                await signInManager.SignInAsync(user, isPersistent: true);

                // Storing User data in session ------
                HttpContext.Session.SetString("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.UserName);
                HttpContext.Session.SetString("UserEmail", user.Email);

                TempData["SuccessMessage"] = "Registration successful! Welcome!";
                return RedirectToAction("Login", "Auth");
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException is SqlException sqlEx)
            {
                HandleDuplicateEntryErrors(sqlEx, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,
                    "An unexpected error occurred during registration. Please try again.");
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
            await userManager.DeleteAsync(user);
        }

        private async Task HandleFailedClaimsAssignment(ApplicationUser user, IdentityResult claimsResult)
        {
            AddErrorsToModelState(claimsResult.Errors);
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
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public async Task< IActionResult> Login()
        {
            var loginVM = new LoginViewModel()
            {
                schemes = await signInManager.GetExternalAuthenticationSchemesAsync()
            };
            return View(loginVM);
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

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // Add any existing user claims
            await signInManager.SignInWithClaimsAsync(
                user,
                model.RememberMe,
                claims);

            // Storing User data in session ------
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Handle return URL for redirect after login
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult KeepSessionAlive()
        {
            // Renew session lifetime ------
            HttpContext.Session.SetString("LastActivity", DateTime.UtcNow.ToString());
            return Ok();
        }
        public IActionResult ExternalLogin(string provider, string returnUrl = "")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Auth", new { ReturnUrl = returnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "", string remoteError = "")
        {

            var loginVM = new LoginViewModel()
            {
                schemes = await signInManager.GetExternalAuthenticationSchemesAsync()
            };

            if (!string.IsNullOrEmpty(remoteError))
            {
                ModelState.AddModelError("", $"Error from extranal login provide: {remoteError}");
                return View("Login", loginVM);
            }

            //Get login info
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError("", $"Error from extranal login provide: {remoteError}");
                return View("Login", loginVM);
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
                return RedirectToAction("Index", "Home");
            else
            {
                var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (!string.IsNullOrEmpty(userEmail))
                {
                    var user = await userManager.FindByEmailAsync(userEmail);

                    if (user == null)
                    {
                        user = new ApplicationUser()
                        {
                            FirstName=userEmail,
                            UserName = userEmail,
                            Email = userEmail,
                            LastName= userEmail,
                            EmailConfirmed = true
                        };

                        await userManager.CreateAsync(user);
                        await userManager.AddToRoleAsync(user, "user");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("EditProfile", "Profile");
                }

            }

            ModelState.AddModelError("", $"Something went wrong");
            return View("Login", loginVM);
        }
    }
}