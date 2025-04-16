using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class CartService
    {
        public ICartRepository CartRepository;
        public CartService(ICartRepository cartRepository)
        {
            CartRepository = cartRepository;
            
        }
    }
}
