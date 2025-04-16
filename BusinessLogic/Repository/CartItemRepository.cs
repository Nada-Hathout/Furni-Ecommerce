using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    internal class CartItemRepository : ICartItemRepository
    {
        public FurniDbContext context;
        public CartItemRepository(FurniDbContext furniDbContext)
        {
            
            this.context = furniDbContext;
        }
        public void Add(CartItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CartItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public CartItem GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(CartItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
