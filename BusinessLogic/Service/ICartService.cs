using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ICartService
    {
        void AddItemToCart(string userId, int productId);
        int GetCartItemsCountByUserId(string userId);
       
    }
}
