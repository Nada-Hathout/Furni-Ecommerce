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
        private readonly FurniDbContext _context;

        public CartRepository(FurniDbContext context)
        {
            _context = context;
        }

        public void Add(Cart entity)
        {
            _context.Carts.Add(entity);
        }

        public void Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }
        }

        public List<Cart> GetAll()
        {
            return _context.Carts.ToList();
        }

        public Cart GetByID(int id)
        {
            return _context.Carts.Find(id);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Update(Cart entity)
        {
            _context.Carts.Update(entity);
        }
    }
}
