using BusinessLogic.Repository;
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
    }
}
