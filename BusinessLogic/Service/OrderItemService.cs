using BusinessLogic.Repository;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class OrderItemService:IOrderItemService
    {
        public IOrderItemRepository orderItemRepository;
        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
            
        }

        public async Task<List<OrderItem>> GetOrderItemsByOrderId(int orderID)
        {
            return await orderItemRepository.GetOrderItemsByOrderIdAsc(orderID);
        }

        public async Task<int> SaveOrderASC(List<CartItem> cartItems, int orderID)
        {
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderID,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };
                await orderItemRepository.SaveOrderItemAsc(orderItem);
            }
            return 1; // Success (only after all items are processed)
        }
    }
}
