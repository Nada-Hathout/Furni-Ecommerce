using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public FurniDbContext context;
        public OrderItemRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public void Add(OrderItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<OrderItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public OrderItem GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
