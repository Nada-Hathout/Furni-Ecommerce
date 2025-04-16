using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class ReviewService
    {
        public IReviewRepository reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
            
        }
    }
}
