using BusinessLogic.Repository;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class FavoriteService:IFavoriteService
    {
        public IFavoriteRepository FavoriteRepository;
        public FavoriteService(IFavoriteRepository favoriteRepository)
        {
            FavoriteRepository = favoriteRepository;

            
        }

        public int GetFavoriteCount(string UserId)
        {
            return FavoriteRepository.FavCounter(UserId);
        }

        //public int GetCountOfFavPrd(string UserId)
        //{

        //}



        public bool ToggleFavourite(string userId, int productId)
        {
            var exists=FavoriteRepository.Exists(userId, productId);
            if (exists) { 
            FavoriteRepository.Remove(userId, productId);
               
                return false;

            }
            else
            {
                FavoriteRepository.Add(userId, productId);
                return true;
            }
        }

        List<FavouriteViewModel> IFavoriteService.GetFavProducts(string userId)
        {
            var favouritePrd = FavoriteRepository.GetAllUserFav(userId);
            var countOfFavItems = FavoriteRepository.FavCounter(userId);
            return FavoriteRepository.GetAllUserFav(userId)
            .Select(f => new FavouriteViewModel
            {
                PrdId = f.Product.Id,
                Name = f.Product.Name,
                Price = f.Product.Price,
                ImgUrl = f.Product.ImagePath,
                IsFavorite = favouritePrd.Any(p => p.ProductId == f.ProductId),
                qty=countOfFavItems
            })
            .ToList();
        }
    }
}
