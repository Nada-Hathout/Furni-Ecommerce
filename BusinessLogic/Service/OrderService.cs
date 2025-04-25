using BusinessLogic.Repository;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class OrderService : IOrderService
    {
        public IOrderRepository OrderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            this.OrderRepository = orderRepository;


        }

        public async Task<Order> GetOrderByUserId(string userId)
        {
            return await OrderRepository.GetOrderByUserIdASC(userId);
        }

        public async Task<Order> SaveOrderASC(string userID, int paymentID, int AddressID, decimal totalAmount)
        {
            var Order = new Order
            {
                UserId =  userID  ,
                AddressId = AddressID,
                PaymentId = paymentID,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "success"
            };
          await  OrderRepository.SaveOrderAsc(Order);
            return Order;
    } }
}
