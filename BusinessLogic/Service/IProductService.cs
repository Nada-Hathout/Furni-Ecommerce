using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Furni_Ecommerce_Shared.AdminViewModel;

namespace BusinessLogic.Service
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public IQueryable<ShopProductViewModel> SearchProduct(string keyword,string userId);
        public IQueryable<ShopProductViewModel> GetProducts(string userId);

        List<ProductsAndCommentsViewModel> GetProductsInfo(string userId);
        ProductsAndCommentsViewModel GetDetails(string userId,int id);
        public bool AddCart(int productId, HttpContext httpContext,string userID);
        public void AddProduct(Product product);
        public void EditProduct(Product product);

        public void DeleteProduct(int id);
        public Product GetProdById(int id);

        public IEnumerable<SelectListItem> GetAllCategories();
        public List<ProductViewModel> GetAllProductsAsViewModel();
        public ProductViewModel GetProductViewModelById(int id);

    }
}
