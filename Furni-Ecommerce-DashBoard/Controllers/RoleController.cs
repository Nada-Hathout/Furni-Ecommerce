using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    public class RoleController : Controller
    {
       public RoleManager<IdentityRole> roleManager ;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid) 
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = roleViewModel.RoleName
                };
                    IdentityResult result=await roleManager.CreateAsync(role);
                if (result.Succeeded) 
                {
                    return RedirectToAction("Index","Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,item.Description);
                    
                }

            }
            return View(roleViewModel);
        }
        public async Task<IActionResult> Roles()
        {
            // Get all roles in the system
            var roles = await roleManager.Roles.ToListAsync(); // Get all roles from the RoleManager
            return View(roles);
        }

    }
}
