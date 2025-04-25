using BusinessLogic.Repository;
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
    }
}
