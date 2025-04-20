using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    internal class UserRepository : IUsersRepository
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
            return context.Users.ToList();
        }

        public ApplicationUser GetByID(int id)
        {
            throw new NotImplementedException();

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
