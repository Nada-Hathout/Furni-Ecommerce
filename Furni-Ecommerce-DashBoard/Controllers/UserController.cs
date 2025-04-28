using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Furni_Ecommerce_DashBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<ApplicationUser> userManager ,RoleManager<IdentityRole> roleManager,IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.userService = userService;
            
        }
        public async Task<IActionResult> Index()
        {
            var usersInRole = await userService.GetAllUsers();
            
            return View(usersInRole);
        }
        public async Task<IActionResult> Details(string id)
        {
           
            var model =await userService.GetSpecificUser(id);

            return View(model);
        }
        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
          
            var result =await userService.DeleteUser(id);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "User deleted successfully.";

                return RedirectToAction(nameof(Index));
            }

            // handle errors if any
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
