using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace BusinessLogic.Service
{
    public class ProductService : IProductService
    {
        private readonly FurniDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _productRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IFavoriteRepository _favoriteRepository;

        public ProductService(
            IProductRepository productRepository,
            IReviewRepository reviewRepository,
            IFavoriteRepository favoriteRepository,
            FurniDbContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _reviewRepository = reviewRepository;
            _favoriteRepository = favoriteRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public Product? GetProductById(int id) => _productRepository.GetByID(id);

        public bool AddCart(int productId, HttpContext httpContext, string userID)
        {
            var product = _productRepository.GetByID(productId);
            if (product == null) return false;

            // ========== SESSION CART ========== //
            var session = httpContext.Session;
            string? cartString = session.GetString("Cart");
            List<CartItemViewModel>? sessionCart = string.IsNullOrEmpty(cartString)
                ? new List<CartItemViewModel>()
                : JsonSerializer.Deserialize<List<CartItemViewModel>>(cartString);

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

            session.SetString("Cart", JsonSerializer.Serialize(sessionCart));

            // ========== DATABASE CART ========== //
            int? cartId = session.GetInt32("CartId");
            Cart cart;

            if (!string.IsNullOrEmpty(userID))
            {
                cart = _context.Carts.Include(c => c.CartItems)
                                     .FirstOrDefault(c => c.UserId == userID)
                        ?? CreateNewCart(userID);
            }
            else
            {
                cart = cartId == null
                    ? CreateNewCart(userID)
                    : _context.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.Id == cartId.Value);

                if (cart == null) return false;
                session.SetInt32("CartId", cart.Id);
            }

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

            _context.SaveChanges();
            return true;
        }

        private Cart CreateNewCart(string userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CartItems = new List<CartItem>()
            };
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart;
        }

        public ProductsAndCommentsViewModel GetDetails(int id)
        {
            var product = _productRepository.GetByID(id);
            return product == null ? null : MapToViewModel(product);
        }

        public List<ProductsAndCommentsViewModel> GetProductsInfo(string userId)
        {
            var products = _productRepository.GetAll();
            var reviews = _reviewRepository.GetAll();
            var favouritePrd = _favoriteRepository.GetAllUserFav(userId);
            var countOfFavItems = _favoriteRepository.FavCounter(userId);

            return products.Select(p => new ProductsAndCommentsViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                ImagePath = p.ImagePath,
                IsFavorite = favouritePrd.Any(f => f.ProductId == p.Id),
                qty = countOfFavItems,
                Comments = reviews.Where(r => r.ProductId == p.Id)
                                .Select(r => new CommentViewModel
                                {
                                    Text = r.Comment,
                                    UserName = $"{r.User.FirstName} {r.User.LastName}"
                                }).ToList()
            }).ToList();
        }

        public List<Product> GetAllProducts() => _productRepository.GetAll();

        public IQueryable<ShopProductViewModel> SearchProduct(string keyword) =>
            _productRepository.SearchProduct(keyword);

        public IQueryable<ShopProductViewModel> GetProducts() =>
            _productRepository.GetAllProducts();

        public void AddProduct(Product product) => _productRepository.Add(product);

        public void EditProduct(Product product) => _productRepository.Update(product);

        public void DeleteProduct(int id) => _productRepository.Delete(id);

        public Product GetProdById(int id) => _productRepository.GetProdById(id);

        private ProductsAndCommentsViewModel MapToViewModel(Product product) => new()
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
}