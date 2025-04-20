using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class ProductRepository : IProductRepository
    {
        public FurniDbContext context;
        public ProductRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public void Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return context.Products.ToList();
        }

        public Product GetByID(int id)
        {
            return context.Products.Include(p => p.Category)
                                  .Include(p => p.Reviews)
                                 .ThenInclude(r => r.User).FirstOrDefault(p => p.Id == id);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
