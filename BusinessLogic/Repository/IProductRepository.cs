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
        IEnumerable<ShopProductViewModel> SearchProduct(string keyword);
        IEnumerable<ShopProductViewModel> GetAllProducts();

       
    }
}
