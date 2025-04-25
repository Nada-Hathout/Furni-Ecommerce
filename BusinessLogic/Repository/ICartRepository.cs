using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface ICartRepository:IRepository<Cart>
    {
        public void AddItemToCart(string userId, int productId);
        public int GetCartItemsCountByUserId(string userId);
    }
}
