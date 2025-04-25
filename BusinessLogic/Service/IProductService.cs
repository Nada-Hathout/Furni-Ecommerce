using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;

namespace BusinessLogic.Service
{
    public interface IProductService
    {
        public List<Product> GetAllProducts();
        public IQueryable<ShopProductViewModel> SearchProduct(string keyword);
        public IQueryable<ShopProductViewModel> GetProducts();

        List<ProductsAndCommentsViewModel> GetProductsInfo();
        ProductsAndCommentsViewModel getDetails(int id);
    }
}
