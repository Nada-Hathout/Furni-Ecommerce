using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IUserService
    {
        public ApplicationUser Register(RegisterViewModel model);
        public Task< List<AllAdminViewModel>> GetAllAdmin();

        public ApplicationUser CreateAdmin(AdminFormViewModel model);
        public Task<EditAdminViewModel> GetSpecificAdmin(string adminId);
        public Task<List<RoleSelectionViewModel>> GetAvailableRoles();
        public Task<IdentityResult> EditAdmin(EditAdminViewModel model);
    }
}
