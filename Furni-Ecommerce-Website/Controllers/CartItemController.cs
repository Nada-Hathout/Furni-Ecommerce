using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Service;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Furni_Ecommerce_Website.Controllers
{

    public class CartItemController : Controller
    {
        private readonly ICartItemService _cartItemService;
        private readonly IProductService _productService;
        private readonly FurniDbContext _context;

        public CartItemController(ICartItemService cartItemService, IProductService productService, FurniDbContext context)
        {
            _cartItemService = cartItemService;
            _productService = productService;
            _context = context;
        }

        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = _cartItemService.GetAllCartItems(userId);
            var cartItemViewModels = cartItems.Select(item => new CartItemViewModel
            {
                CartItemId = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.Product.Price,
                Quantity = item.Quantity,
                ImagePath = item.Product.ImagePath,
                AvailableQuantity = item.Product.Stock 
            }).ToList();

            return View(cartItemViewModels);
        }

        //public IActionResult Add()
        //{
        //    ViewBag.Products = _productService.GetAllProducts();
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Add(CartItemViewModel vm)
        //{
        //    var product = _productService.GetProductById(vm.ProductId);

        //    if (product != null)
        //    {
        //        var newItem = new CartItem
        //        {
        //            ProductId = vm.ProductId,
        //            Quantity = vm.Quantity
        //        };

        //        _cartItemService.AddCartItem(newItem);
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.Products = _productService.GetAllProducts();
        //    return View(vm);
        //}

        public IActionResult Edit(int id)
        {
            var item = _cartItemService.GetCartItemById(id);
            if (item == null) return NotFound();

            var vm = new CartItemViewModel
            {
                CartItemId = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.Product.Price,
                Quantity = item.Quantity,
                ImagePath = item.Product.ImagePath,
                AvailableQuantity = item.Product.Stock
            };

            ViewBag.Products = _productService.GetAllProducts();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(CartItemViewModel vm)
        {
            var item = _cartItemService.GetCartItemById(vm.CartItemId);
            if (item == null) return NotFound();

            item.ProductId = vm.ProductId;
            item.Quantity = vm.Quantity;

            _cartItemService.UpdateCartItem(item);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _cartItemService.DeleteCartItem(id);
            return RedirectToAction("Index");
        }
        [HttpPost("CartItem/UpdateQuantity/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] UpdateQuantityDto dto)
        {
            try
            {
                var cartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

                if (cartItem == null)
                {
                    return NotFound(new { message = "Cart item not found" });
                }

                // Validate quantity
                if (dto.Quantity <= 0)
                {
                    return BadRequest(new { message = "Quantity must be greater than 0" });
                }

                // Check product stock
                if (dto.Quantity > cartItem.Product.Stock)
                {
                    return BadRequest(new
                    {
                        message = $"Only {cartItem.Product.Stock} items available in stock",
                        maxQuantity = cartItem.Product.Stock
                    });
                }

                // Update and save
                cartItem.Quantity = dto.Quantity;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    newQuantity = dto.Quantity,
                    itemTotal = (dto.Quantity * cartItem.Product.Price).ToString("0.00"),
                    productName = cartItem.Product.Name,
                    unitPrice = cartItem.Product.Price
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }


        public class UpdateQuantityDto
        {
            public int Quantity { get; set; }
        }
    }
}
