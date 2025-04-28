using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IFavoriteRepository:IRepository<Favorite> 
    {
        public bool Exists(string userId, int productId);
        public void Add(string userId, int productId);
        public void Remove(string userId, int productId);
        public List<Favorite> GetAllUserFav(string userId);
        public int FavCounter(string userId);

    }

 }
