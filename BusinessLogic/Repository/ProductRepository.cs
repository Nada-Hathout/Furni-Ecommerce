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
        private readonly IFavoriteRepository _favoriteRepository;

        public ProductRepository(FurniDbContext furniDbContext, IFavoriteRepository favoriteRepository)
        {
            context = furniDbContext;
            _favoriteRepository = favoriteRepository;

        }
        
     
        public void Add(Product entity)
        {
            context.Products.Add(entity);
            context.SaveChanges();
        }
        public void Update(Product entity)
        {
            context.Products.Update(entity);
            context.SaveChanges();

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

        public IQueryable<ShopProductViewModel> SearchProduct(string keyword, string userId)
        {

            if(string.IsNullOrEmpty(keyword))
            {
                GetAllProducts(userId);
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
            var countOfFavItems = _favoriteRepository.FavCounter(userId);
            var favoriteProductIds = _favoriteRepository.GetAllUserFav(userId).Select(f => f.ProductId).ToList();
            return productQuery.Select(p => new ShopProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                imgUrl = p.ImagePath,
                qty = countOfFavItems,
                IsFavorite = favoriteProductIds.Contains(p.Id)
            });

        }

        public IQueryable<ShopProductViewModel> GetAllProducts(string userId)
        {
            
            var countOfFavItems = _favoriteRepository.FavCounter(userId);
            var favoriteProductIds = _favoriteRepository.GetAllUserFav(userId).Select(f => f.ProductId).ToList();
            return context.Products.Select(p => new ShopProductViewModel {  
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock,
            imgUrl = p.ImagePath ,
            qty=countOfFavItems, 
            IsFavorite = favoriteProductIds.Contains(p.Id)


            });

        }

        public void Delete(int id)
        {
           var prod = context.Products.FirstOrDefault(p=>p.Id == id);
            if(prod != null)
            {
                context.Products.Remove(prod);
            }
            context.SaveChanges();

        }
        public Product GetProdById(int id)
        {
            return context.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
