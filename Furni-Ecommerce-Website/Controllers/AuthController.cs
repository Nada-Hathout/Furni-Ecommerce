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
using BusinessLogic.External_Service;
using Microsoft.AspNetCore.Authorization;
using BusinessLogic.Settings;

namespace Furni_Ecommerce_Website.Controllers
{
    public class AuthController : Controller
    {
        public UserManager<ApplicationUser> userManager;
        public SignInManager<ApplicationUser> signInManager;
        public IUserService userService;
        private readonly IEmailService emailService;

        public AuthController(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IUserService userService,IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
            this.emailService = emailService;
        }

        public IActionResult Register()
        {
           
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Register");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User not found.");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful! and Email Confirmed Welcome!";
                return RedirectToAction(nameof(Login));
            }

            return RedirectToAction(nameof(Login));
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

                // ✅ Assign default "user" role
                var roleResult = await userManager.AddToRoleAsync(user, "user");
                if (!roleResult.Succeeded)
                {
                    await HandleFailedRoleAssignment(user, roleResult);
                    return View(model);
                }

                // Generate confirmation token and link
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(
                    "ConfirmEmail",
                    "Auth",
                    new { userId = user.Id, token },
                    protocol: HttpContext.Request.Scheme
                );

                if (string.IsNullOrEmpty(confirmationLink))
                {
                    throw new InvalidOperationException("Failed to generate confirmation link");
                }

                try
                {
                    await emailService.SendEmailAsync(
                        user.Email,
                        "Confirm Your Email Address",
                        confirmationLink
                    );
                }
                catch (Exception emailEx)
                {
                    await userManager.DeleteAsync(user);
                    ModelState.AddModelError(string.Empty, "Failed to send confirmation email.");
                    ModelState.AddModelError(string.Empty, emailEx.Message);
                    return View(model);
                }

                TempData["SuccessMessage"] = "Registration successful! Please check your email to confirm your account.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An unexpected error occurred during registration.");
                return View(model);
            }
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
            if (user != null && !await userManager.IsEmailConfirmedAsync(user))
            {
                var loginVM = new LoginViewModel()
                {
                    schemes = await signInManager.GetExternalAuthenticationSchemesAsync()
                };
                ModelState.AddModelError(string.Empty, "Please confirm your email first.");
                return View(loginVM);
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
     public IActionResult ForgetPassword()
        {
            return View();

        }
        //[HttpPost]
        //public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        //{
        //    var token = await userManager.GeneratePasswordResetTokenAsync(userManager.FindByEmailAsync(model.Email).Result);
        //    var ResetPasswordLink = Url.Action("ResetPassword", "Auth", new { email = model.Email, Token=token },Request.Scheme);
        //    if (ModelState.IsValid)
        //    {
        //        var user = await userManager.FindByEmailAsync(model.Email);
        //        if (user is not null)
        //        {
        //            var email = new Email()
        //            {
        //                To = model.Email,
        //                Subject = "Reset Password",
        //                Body = "Please click the link below to reset your password."
        //            };
        //            emailSettingg.SendEmail(email);
        //            return RedirectToAction(nameof(CheckYourInbox));
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Email not found.");
        //        }
        //    }

        //        return View("ForgetPassword",model);


        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View("ForgetPassword", model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email not found.");
                return View("ForgetPassword", model);
            }

            // Generate token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            // Generate reset password link
            var resetPasswordLink = Url.Action(
                "ResetPassword",
                "Auth",
                new { email = model.Email, token = token },
                protocol: HttpContext.Request.Scheme
            );

            // Create email body with link
            var emailBody = $@"
    <div style='font-family:Arial, sans-serif; padding:20px; background-color:#f4f4f4;'>
        <div style='max-width:600px; margin:auto; background:white; padding:20px; border-radius:10px;'>
            <h2 style='color:#1c5531;'>Hello {user.UserName},</h2>
            <p style='font-size:16px; color:#333;'>We received a request to reset your password.</p>
            <p style='margin:20px 0;'>
                <a href='{resetPasswordLink}' style='background-color:#1c5531; color:white; padding:12px 20px; text-decoration:none; border-radius:5px;'>Reset Your Password</a>
            </p>
            <p style='font-size:14px; color:#888;'>If you did not request this, you can safely ignore this email.</p>
        </div>
    </div>";

            // Prepare email
            var email = new Email()
            {
                To = model.Email,
                Subject = "Reset Your Password",
                Body = emailBody
            };

            // Send email
            emailSettingg.SendEmail(email);

            // Redirect to check inbox view
            TempData["SuccessMessage"] = "Please check your inbox for the password reset link.";
            return RedirectToAction(nameof(CheckYourInbox));
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid password reset token.");
            }

            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid request.");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Your password has been reset successfully.";
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }



    }
}