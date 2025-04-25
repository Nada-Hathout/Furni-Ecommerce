using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        public FurniDbContext context;
        public PaymentRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public void Add(Payment entity)
        {
            context.Payments.Add(entity);
        }

        public async Task<Payment> AddPaymentAsc(Payment payment)
        {
            context.Payments.Add(payment);
            await context.SaveChangesAsync();
            return payment;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetAll()
        {
            throw new NotImplementedException();
        }

        public Payment GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> GetPaymentByIdAsc(int id)
        {
            return await context.Payments.Where(p=>p.Id==id).FirstOrDefaultAsync();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Payment entity)
        {
            throw new NotImplementedException();
        }
    }
}
