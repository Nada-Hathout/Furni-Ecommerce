using BusinessLogic.Repository;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class CartService:ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public void AddCart(Cart cart)
        {
            _cartRepository.Add(cart);
            _cartRepository.Save();
        }

        public void UpdateCart(Cart cart)
        {
            _cartRepository.Update(cart);
            _cartRepository.Save();
        }

        public void DeleteCart(int id)
        {
            _cartRepository.Delete(id);
            _cartRepository.Save();
        }

        public Cart GetCartById(int id)
        {
            return _cartRepository.GetByID(id);
        }

        public List<Cart> GetAllCarts()
        {
            return _cartRepository.GetAll();
        }

        public void AddItemToCart(string userId, int productId)
        {
            _cartRepository.AddItemToCart(userId, productId);
        }

        public int GetCartItemsCountByUserId(string userId)
        {
            return _cartRepository.GetCartItemsCountByUserId(userId);
        }
    }
}
