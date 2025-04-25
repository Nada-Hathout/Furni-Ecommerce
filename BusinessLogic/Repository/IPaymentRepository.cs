using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        public Task<Payment> AddPaymentAsc(Payment payment);
        public Task<Payment> GetPaymentByIdAsc(int id);
    }
}
