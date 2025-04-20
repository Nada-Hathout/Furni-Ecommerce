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
    public class ProductService:IProductService
    {

        private readonly FurniDbContext _context;

       

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

       public IProductRepository productRepository;
        public IReviewRepository reviewRepository;
      
        public ProductService(IProductRepository productRepository, IReviewRepository reviewRepository,FurniDbContext context)
        {
            this.productRepository = productRepository;

            this.reviewRepository = reviewRepository;
           this. _context = context;
           

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

        public List<ProductsAndCommentsViewModel> GetProductsInfo()
        {
            List<Product> prds = productRepository.GetAll();
            List<Review> reviews = reviewRepository.GetAll();
            List<ProductsAndCommentsViewModel> prdViewModel = prds.Select(p => new ProductsAndCommentsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath,
                Comments = reviews.Where(r => r.ProductId == p.Id).Select(r => new CommentViewModel
                {
                    Text = r.Comment,
                    UserName=r.User.FirstName+" "+r.User.LastName,
                }).ToList(),

            }).ToList();
           
            return prdViewModel;
           
        }
    }
}
