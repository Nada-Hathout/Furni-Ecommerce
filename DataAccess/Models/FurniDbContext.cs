using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class FurniDbContext:IdentityDbContext<ApplicationUser>
    {
        public FurniDbContext():base()
        {
            
        }
        public FurniDbContext(DbContextOptions<FurniDbContext> options):base(options)
        {
            
        }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<CartItem> CartItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }

    }
}
