using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.UserViewModel
{
    public class FavouriteViewModel
    {
        public int UserId { get; set; }
        public int PrdId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImgUrl { get; set; }
        public bool IsFavorite { get; set; }
        public int qty { get; set; }
    }
}
