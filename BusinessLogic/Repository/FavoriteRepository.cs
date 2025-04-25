using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        public FurniDbContext context;
        public FavoriteRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public bool Exists(string userId, int productId)
        {
            return context.Favorites.Any(f => f.UserId == userId && f.ProductId == productId);
        }
        public void Add(string userId, int productId)
        {
            var fav = new Favorite { UserId = userId, ProductId = productId };
            context.Favorites.Add(fav);
            context.SaveChanges();
        }
        public void Remove(string userId, int productId)
        {
            var fav = context.Favorites.FirstOrDefault(f => f.UserId == userId && f.ProductId == productId);
            if (fav != null)
            {
                context.Favorites.Remove(fav);
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Favorite> GetAll()
        {
            throw new NotImplementedException();
        }

        public Favorite GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Favorite entity)
        {
            throw new NotImplementedException();
        }

        public void Add(Favorite entity)
        {
            throw new NotImplementedException();
        }
    }
}
