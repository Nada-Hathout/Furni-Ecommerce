using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.UserViewModel
{
    public class ProductsAndCommentsViewModel
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [MaxLength(300)]
        public string? ImagePath { get; set; }

      
        public List<CommentViewModel> Comments { get; set; }
       
        public String CategoryName { get; set; }

    }
    public class CommentViewModel
    {
        public string Text { get; set; }
        public string UserName { get; set; }
    }

  
}
