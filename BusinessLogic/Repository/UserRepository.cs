using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class UserRepository : IUsersRepository
    {
      public  FurniDbContext context;
        public UserRepository(FurniDbContext dbContext)
        {
            this.context = dbContext;
            
        }
        public void Add(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetAll()
        {
            throw new NotImplementedException();
        }

        public ApplicationUser GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public string? GetUniquPhone(string phone)
        {
           return context.Users.FirstOrDefault(u=>u.PhoneNumber == phone)?.PhoneNumber;
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
