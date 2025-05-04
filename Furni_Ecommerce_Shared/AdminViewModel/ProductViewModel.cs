using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CatId { get; set; }
        public string? CategoryName { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public string? ImgPath { get; set; }
        public IFormFile? Img { get; set; }


    }
}
