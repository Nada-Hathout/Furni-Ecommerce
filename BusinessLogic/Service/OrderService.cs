using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class OrderService:IOrderService
    {
        public IOrderRepository OrderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
          this.  OrderRepository = orderRepository;

            
        }
    }
}
