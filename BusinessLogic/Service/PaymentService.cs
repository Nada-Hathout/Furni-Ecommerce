using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class PaymentService:IPaymentService
    {
        public IPaymentRepository paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
            
        }

        public async Task<Payment> GetPaymentById(int id)
        {
           return await paymentRepository.GetPaymentByIdAsc(id);
        }

        public async Task<Payment> SavePaymentData(CheckoutViewModel model, string userID)
        {
            Payment payment = new Payment()
            {
                UserId = userID,
                TransactionId = model.PaymentDetails.TransactionId,
                PaymentMethod = model.PaymentDetails.PaymentMethod,
                PaymentStatus = model.PaymentDetails.PaymentStatus
            };
          await  paymentRepository.AddPaymentAsc(payment);
            return payment;
        }
    }
}
