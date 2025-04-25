using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;

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
        public void Update(Product entity)
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

        public IQueryable<ShopProductViewModel> SearchProduct(string keyword)
        {

            if(string.IsNullOrEmpty(keyword))
            {
                GetAllProducts();
            }

            keyword = keyword.ToLower();
            var category = context.Categories.FirstOrDefault(c=>c.Name == keyword);
            IQueryable<Product> productQuery;
            if(category != null)
            {
                productQuery=context.Products.Where(p=>p.CategoryId == category.Id);
            }
            else
            {
                productQuery = context.Products.Where(p=>p.Name.ToLower().Contains(keyword));
            }
            return productQuery.Select(p => new ShopProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                imgUrl = p.ImagePath
            });

        }

        public IQueryable<ShopProductViewModel> GetAllProducts()
        {
            return context.Products.Select(p => new ShopProductViewModel {Id = p.Id, Name = p.Name, Price = p.Price, Stock = p.Stock ,imgUrl = p.ImagePath});
        }
    }
}
