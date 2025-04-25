using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public FurniDbContext context;
        public OrderRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public void Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetLastOrderByUserId(string userId)
        {
            return await context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate) // or OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<Order> GetOrderByUserIdASC(string userId)
        {
            var query = context.Orders
    .Where(o => o.UserId == userId)
    .OrderByDescending(o => o.OrderDate);

            Console.WriteLine(query.ToQueryString());
            return await query.FirstOrDefaultAsync();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> SaveOrderAsc(Order order)
        {
            context.Orders.Add(order);
           await context.SaveChangesAsync();
            return order;
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
