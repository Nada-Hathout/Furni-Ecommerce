using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using DataAccess;

namespace BusinessLogic.Service
{

    public class ProductService : IProductService
    {
        private readonly FurniDbContext _context;
        private readonly IProductRepository productRepository;
        private readonly IReviewRepository reviewRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            FurniDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            this.productRepository = productRepository;
            this.reviewRepository = reviewRepository;
            this._context = context;
            this._httpContextAccessor = httpContextAccessor;
        }

        public Product? GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }


        public bool AddCart(int productId, HttpContext httpContext, string userID)
        {
            var product = productRepository.GetByID(productId);
            if (product == null) return false;

            // ========== SESSION CART (NORMAL SESSION) ========== //
            var session = httpContext.Session;
            string? cartString = session.GetString("Cart");
            List<CartItemViewModel>? sessionCart = string.IsNullOrEmpty(cartString)
                ? new List<CartItemViewModel>()
                : JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);

            // Add/update the item in the session cart (for non-logged-in users)
            var existingSessionItem = sessionCart.FirstOrDefault(i => i.ProductId == productId);
            if (existingSessionItem != null)
            {
                existingSessionItem.Quantity++;
            }
            else
            {
                sessionCart.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ImagePath = product.ImagePath,
                    Quantity = 1,
                    UnitPrice = product.Price,
                    AvailableQuantity = product.Stock
                });
            }

            // Save updated session cart as string
            session.SetString("Cart", JsonSerializer.Serialize(sessionCart));

            // ========== DATABASE CART (LOGGED-IN USER) ========== //
            int? cartId = session.GetInt32("CartId");
            Cart cart;

            // If user is logged in, handle the cart in the database
            if (!string.IsNullOrEmpty(userID))
            {
                // Try to fetch the user's cart from the database
                cart = _context.Carts.Include(c => c.CartItems)
                                     .FirstOrDefault(c => c.UserId == userID);

                if (cart == null)
                {
                    // Create a new cart for the user if not found
                    cart = new Cart
                    {
                        UserId = userID,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }
            }
            else
            {
                // If the user is not logged in, use session-based cart
                if (cartId == null)
                {
                    // No cart found in the session, so create a new one
                    cart = new Cart
                    {
                        UserId = userID,
                        CartItems = new List<CartItem>()
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                    session.SetInt32("CartId", cart.Id);
                }
                else
                {
                    // Use the session cart ID to find the cart from the database
                    cart = _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.Id == cartId.Value);
                    if (cart == null) return false;
                }
            }

            // Update the cart item in the database
            var existingDbItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (existingDbItem != null)
            {
                existingDbItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = 1
                });
            }

            // Save the changes to the database cart
            _context.SaveChanges();
            return true;
        }




        public ProductsAndCommentsViewModel? GetDetails(int id)
        {
            var product = productRepository.GetByID(id);
            if (product == null) return null;

            return new ProductsAndCommentsViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImagePath = product.ImagePath,
                Stock = product.Stock,
                Price = product.Price,
                CategoryName = product.Category?.Name
            };
        }

        public List<ProductsAndCommentsViewModel> GetProductsInfo()
        {
            var prds = productRepository.GetAll();
            var reviews = reviewRepository.GetAll();

            return prds.Select(p => new ProductsAndCommentsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath,
                Comments = reviews.Where(r => r.ProductId == p.Id).Select(r => new CommentViewModel
                {
                    Text = r.Comment,
                    UserName = $"{r.User.FirstName} {r.User.LastName}"
                }).ToList()
            }).ToList();
        }

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAll();
        }

        public IQueryable <ShopProductViewModel> SearchProduct(string keyword)
        {
            return (IQueryable<ShopProductViewModel>)productRepository.SearchProduct(keyword);
        }

        IQueryable<ShopProductViewModel> IProductService.GetProducts()
        {
            return (IQueryable<ShopProductViewModel>)productRepository.GetAllProducts();
        }

        public ProductsAndCommentsViewModel getDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}
