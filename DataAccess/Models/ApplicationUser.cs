using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual ICollection<Address>? Addresses { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }

        public virtual ICollection<Favorite>? Favorites { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
