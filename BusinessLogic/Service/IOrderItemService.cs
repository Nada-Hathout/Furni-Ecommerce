using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IOrderItemService
    {
        public Task<int> SaveOrderASC(List<CartItem> cartItems,int orderID);
        public Task<List<OrderItem>> GetOrderItemsByOrderId(int orderID);
    }
}
