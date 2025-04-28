using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IOrderService
    {
        public Task<Order>SaveOrderASC(string userID,int paymentID,int AddressID,decimal totalAmount);
        public Task<Order> GetOrderByUserId(string userId);
    }
}
