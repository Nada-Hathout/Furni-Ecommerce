using DataAccess.Models;
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
        public void Add(Favorite entity)
        {
            throw new NotImplementedException();
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
    }
}
