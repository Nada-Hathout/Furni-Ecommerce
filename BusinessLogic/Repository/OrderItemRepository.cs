using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<OrderItem>> GetOrderItemsByOrderIdAsc(int OrderID)
        {
            return await context.OrderItems.Where(o=>o.OrderId == OrderID).ToListAsync();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

       

        public async Task<int> SaveOrderItemAsc(OrderItem OrderItem)
        {
            try
            {
                // Step 1: Add the order item
                context.OrderItems.Add(OrderItem);

                // Step 2: Update product stock (corrected lookup)
                var product = await context.Products
                    .Where(p => p.Id == OrderItem.ProductId) // Fix: Use ProductId, not OrderItem.Id
                    .FirstOrDefaultAsync();

                if (product == null)
                    throw new Exception($"Product {OrderItem.ProductId} not found.");

                if (product.Stock < OrderItem.Quantity)
                    throw new Exception($"Insufficient stock for {product.Name}.");

                product.Stock -= OrderItem.Quantity;
                context.Products.Update(product);

                // Step 3: Save all changes atomically
                await context.SaveChangesAsync();

                return 1; // Success
            }
            catch (Exception ex)
            {
                                                   // Log the error (ex.Message)
                return 0; // Failure
            }
        }

        public void Update(OrderItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
