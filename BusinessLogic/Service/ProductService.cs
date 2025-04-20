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
       public IProductRepository productRepository;
        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            
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
