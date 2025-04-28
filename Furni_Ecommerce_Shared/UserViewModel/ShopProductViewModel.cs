using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.UserViewModel
{
   public class ShopProductViewModel
    {
        public int Id { get; set; }
        public string Name{ get; set; }
        public decimal Price { get; set; }
        public int Stock{ get; set; }
        public string imgUrl{ get; set; }
    }
}
