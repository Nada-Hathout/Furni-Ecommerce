using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class CartRepository : ICartRepository
    {
        public FurniDbContext context;

        public CartRepository(FurniDbContext context)
        {
            
            this.context = context;
        }
        public void Add(Cart entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Cart> GetAll()
        {
            throw new NotImplementedException();
        }

        public Cart GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Cart entity)
        {
            throw new NotImplementedException();
        }
    }
}
