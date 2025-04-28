using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class UserService:IUserService
    {
      public  IUsersRepository usersRepository;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;
        public UserService(IUsersRepository usersRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.usersRepository = usersRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task< List<AllAdminViewModel>> GetAllAdmin()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var model = admins.Select(u => new AllAdminViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                Roles = _userManager.GetRolesAsync(u).Result.ToList()
            }).ToList();
            return model;   
        }

        public ApplicationUser Register(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };
            return user;
        }

        public ApplicationUser CreateAdmin(AdminFormViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };
            return user;
        }

        public async  Task<EditAdminViewModel> GetSpecificAdmin(string adminId)
        {
            var user = await _userManager.FindByIdAsync(adminId);
            if (user == null) return null;

            var userRoles = await _userManager.GetRolesAsync(user);
            var availableRoles = await GetAvailableRoles();

            var model = new EditAdminViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                AvailableRoles = availableRoles.Select(r => new RoleSelectionViewModel
                {
                    RoleName = r.RoleName,
                    IsSelected = userRoles.Contains(r.RoleName)
                }).ToList()
            };
            return model;   
        }

        public async Task<List<RoleSelectionViewModel>> GetAvailableRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleSelectionViewModel { RoleName = r.Name })
                .ToListAsync();

            return roles;
        }

        public async Task<IdentityResult> EditAdmin(EditAdminViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return null;

            // Update basic properties
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            // Update password only if provided
           

            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove all current roles
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
                return removeRolesResult;

            // Add selected roles
            var rolesToAdd = model.AvailableRoles
                .Where(r => r.IsSelected)
                .Select(r => r.RoleName)
                .ToList();

            if (rolesToAdd.Any())
            {
                var addRolesResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addRolesResult.Succeeded)
                    return addRolesResult;
            }

            // Finally, update the user
            return await _userManager.UpdateAsync(user);
        }


    }
}
