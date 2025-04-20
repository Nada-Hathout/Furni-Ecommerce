using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IProductRepository:IRepository<Product>
    {
<<<<<<< HEAD
        IEnumerable<ShopProductViewModel> SearchProduct(string keyword);
        IEnumerable<ShopProductViewModel> GetAllProducts();
=======
       
>>>>>>> 551d20608d3ffae05b98f25585a56c6d7ca9a376
    }
}
