using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furni_Ecommerce_Shared.UserViewModel;
using DataAccess.Models;
namespace BusinessLogic.Service
{
    public interface IFavoriteService
    {
        bool ToggleFavourite(string userId, int productId);
        public List<FavouriteViewModel> GetFavProducts(string id);
        int GetFavItemsCountByUserId(string userId);
        public int GetFavoriteCount(string id);
    }
}
