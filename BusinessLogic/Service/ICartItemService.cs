using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ICartItemService
    {
        /// <summary>
        /// Adds a new cart item.
        /// </summary>
        /// <param name="item">The cart item to add.</param>
        void AddCartItem(CartItem item);

        /// <summary>
        /// Updates an existing cart item.
        /// </summary>
        /// <param name="item">The cart item with updated values.</param>
        void UpdateCartItem(CartItem item);

        /// <summary>
        /// Deletes a cart item by its identifier.</param>
        /// <param name="id">The identifier of the cart item to delete.</param>
        void DeleteCartItem(int id);

        /// <summary>
        /// Gets a cart item by its identifier.</summary>
        /// <param name="id">The identifier of the cart item.</param>
        /// <returns>The matching cart item.</returns>
        CartItem GetCartItemById(int id);

        /// <summary>
        /// Gets all cart items.</summary>
        /// <returns>List of all cart items.</returns>
        List<CartItem> GetAllCartItems(string userID);
      public Task< List<CartItem>> GetAllCartItemsAsc(string userID);
        public Task<int> RemoveRangeCartItemAsc(List<CartItem> cartItems);
        

    }
}
