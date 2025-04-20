using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly FurniDbContext _context;

        public CartItemRepository(FurniDbContext context)
        {
            _context = context;
        }

        public void Add(CartItem entity)
        {
            _context.CartItems.Add(entity);
        }

        public void Delete(int id)
        {
            var item = _context.CartItems.Find(id);
            if (item != null)
            {
                _context.CartItems.Remove(item);
            }
        }

        public List<CartItem> GetAll()
        {
            return _context.CartItems.ToList();
        }

        public CartItem GetByID(int id)
        {
            return _context.CartItems.Find(id);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Update(CartItem entity)
        {
            _context.CartItems.Update(entity);
        }
    }
}
