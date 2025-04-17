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
    }
}
