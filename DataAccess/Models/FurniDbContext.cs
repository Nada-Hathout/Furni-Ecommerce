using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public virtual DbSet<Contact> contact { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //base.OnModelCreating(builder);
            //builder.Entity<ApplicationUser>()
            //  .HasIndex(u => u.PhoneNumber)
            //  .IsUnique();
            base.OnModelCreating(builder);
            builder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)"); // 18 total digits, 2 decimal places

            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // You can also specify precision and scale directly
            builder.Entity<Order>()
                .Property(o => o.TotalAmount)
            .HasPrecision(18, 2);

            builder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2);
                
            builder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);
        }

    }
}
