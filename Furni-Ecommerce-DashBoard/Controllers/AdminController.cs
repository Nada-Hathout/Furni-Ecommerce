using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    [Authorize(Roles = "Owner")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService userService;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.userService = userService;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var model =await userService.GetAllAdmin();

            return View(model);
        }

        // GET: Admin/Create
        public async Task<IActionResult> Create()
        {
            var model = new AdminFormViewModel
            {
                Id = string.Empty,

                AvailableRoles = await GetAvailableRoles()
            };
            return View(model);
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userService.CreateAdmin(model);
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    foreach (var role in model.AvailableRoles.Where(r => r.IsSelected))
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }

                    TempData["SuccessMessage"] = "Admin created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            model.AvailableRoles = await GetAvailableRoles();
            return View(model);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
           var model=await userService.GetSpecificAdmin(id);

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(EditAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.AvailableRoles = await GetAvailableRoles();
                return View(model);
            }

            var result = await userService.EditAdmin(model);
            if (result == null) return NotFound();

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.AvailableRoles = await GetAvailableRoles();
                return View(model);
            }

            TempData["SuccessMessage"] = "Admin updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["ErrorMessage"] = "Failed to delete admin: " +
                    string.Join(", ", result.Errors.Select(e => e.Description));
            }
            else
            {
                TempData["SuccessMessage"] = "Admin deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
        private async Task<List<RoleSelectionViewModel>> GetAvailableRoles()
        {
            return await userService.GetAvailableRoles();
        }
    }
}
