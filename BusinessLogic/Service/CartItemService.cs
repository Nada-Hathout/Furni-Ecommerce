using BusinessLogic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class CartItemService:ICartItemService
    {
        public ICartItemRepository cartItemRepository;
        public CartItemService(ICartItemRepository cartItemRepository)
        {
            this.cartItemRepository = cartItemRepository;
            
        }
    }
}
