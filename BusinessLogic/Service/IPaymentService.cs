using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IPaymentService
    {
        public Task<Payment> SavePaymentData(CheckoutViewModel model,string userID);
        public Task<Payment> GetPaymentById(int  id);
    }
}
