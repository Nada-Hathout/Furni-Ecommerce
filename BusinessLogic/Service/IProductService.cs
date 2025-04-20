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
<<<<<<< HEAD
        public List<Product> GetAllProducts();
        public IEnumerable<ShopProductViewModel> SearchProduct(string keyword);
        public IEnumerable<ShopProductViewModel> GetProducts();
=======
        List<ProductsAndCommentsViewModel> GetProductsInfo();
        ProductsAndCommentsViewModel getDetails(int id);
>>>>>>> 551d20608d3ffae05b98f25585a56c6d7ca9a376
    }
}
