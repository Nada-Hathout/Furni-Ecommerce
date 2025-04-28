using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IOrderItemRepository:IRepository<OrderItem>
    {
        public Task<int> SaveOrderItemAsc(OrderItem OrderItem);
        public Task<List<OrderItem>> GetOrderItemsByOrderIdAsc(int OrderID);
    }
}
