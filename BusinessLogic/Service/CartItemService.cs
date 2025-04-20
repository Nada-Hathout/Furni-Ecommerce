using BusinessLogic.Repository;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class CartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;

        public CartItemService(ICartItemRepository cartItemRepository)
        {
            _cartItemRepository = cartItemRepository;
        }

        public void AddCartItem(CartItem item)
        {
            _cartItemRepository.Add(item);
            _cartItemRepository.Save();
        }

        public void UpdateCartItem(CartItem item)
        {
            _cartItemRepository.Update(item);
            _cartItemRepository.Save();
        }

        public void DeleteCartItem(int id)
        {
            _cartItemRepository.Delete(id);
            _cartItemRepository.Save();
        }

        public CartItem GetCartItemById(int id)
        {
            return _cartItemRepository.GetByID(id);
        }

        public List<CartItem> GetAllCartItems()
        {
            return _cartItemRepository.GetAll();
        }
    }

}
