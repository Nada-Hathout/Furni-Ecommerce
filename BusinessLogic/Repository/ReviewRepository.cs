using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        public FurniDbContext context;
        public ReviewRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
            
        }
        public void Add(Review entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetAll()
        {
            throw new NotImplementedException();
        }

        public Review GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Review entity)
        {
            throw new NotImplementedException();
        }
    }
}
