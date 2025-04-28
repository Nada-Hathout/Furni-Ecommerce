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
    public class ProductService : IProductService
    {

        private readonly FurniDbContext _context;

       

      

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

       public IProductRepository productRepository;
        public IReviewRepository reviewRepository;
        public IFavoriteRepository favoriteRepository;
      
        public ProductService(IProductRepository productRepository, IReviewRepository reviewRepository,FurniDbContext context, IFavoriteRepository favoriteRepository)
        {
            this.productRepository = productRepository;

            this.reviewRepository = reviewRepository;
       
            this.favoriteRepository = favoriteRepository;
           

        }

        public ProductsAndCommentsViewModel getDetails(int id)
        {
            Product product = productRepository.GetByID(id);
            
            if (product == null) {
                return null;
            }
            else
            {
                ProductsAndCommentsViewModel productVM = new ProductsAndCommentsViewModel
                {
                    Id=product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    ImagePath = product.ImagePath,
                    Stock = product.Stock,
                    Price = product.Price,
                    CategoryName=product.Category?.Name,
                };
                return productVM;
            }
        }

        public List<ProductsAndCommentsViewModel> GetProductsInfo(string userId)
        {
            List<Product> prds = productRepository.GetAll();
            List<Review> reviews = reviewRepository.GetAll();
            var favouritePrd = favoriteRepository.GetAllUserFav(userId);
            var countOfFavItems = favoriteRepository.FavCounter(userId);
            List<ProductsAndCommentsViewModel> prdViewModel = prds.Select(p => new ProductsAndCommentsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath,
                IsFavorite=favouritePrd.Any(f=>f.ProductId==p.Id),
                qty= countOfFavItems,
                Comments = reviews.Where(r => r.ProductId == p.Id).Select(r => new CommentViewModel
                {
                    Text = r.Comment,
                    UserName=r.User.FirstName+" "+r.User.LastName,
                }).ToList(),

            }).ToList();
           
            return prdViewModel;
           
        }

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAll();
        }

        public IEnumerable<ShopProductViewModel> SearchProduct(string keyword)
        {
            return productRepository.SearchProduct(keyword);
        }

        IEnumerable<ShopProductViewModel> IProductService.GetProducts()
        {
            return productRepository.GetAllProducts();
        }
    }
}
