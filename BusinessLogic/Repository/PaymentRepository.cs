using DataAccess.Models;
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
            throw new NotImplementedException();
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
