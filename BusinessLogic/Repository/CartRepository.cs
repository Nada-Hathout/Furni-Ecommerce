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

        public void AddItemToCart(string userId, int productId)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            var exisitingItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.Id && ci.ProductId == productId);
            if (exisitingItem != null)
            {
                exisitingItem.Quantity += 1;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.CartItems.Add(newItem);
            }
            _context.SaveChanges();
        }

        public int GetCartItemsCountByUserId(string userId)
        {
            var cart = _context.Carts.FirstOrDefault(c=>c.UserId == userId);
            if (cart == null) return 0;
            return _context.CartItems.Where(ci=>ci.CartId == cart.Id).Sum(ci=>ci.Quantity);
        }
    }
}
