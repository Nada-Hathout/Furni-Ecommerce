using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
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
        public UserService(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            
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
    }
}
