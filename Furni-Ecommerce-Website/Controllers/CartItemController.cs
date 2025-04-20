using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Service;

namespace Furni_Ecommerce_Website.Controllers
{
    public class CartItemController : Controller
    {
        private readonly CartItemService _cartItemService;
        private readonly ProductService _productService;

        public CartItemController(CartItemService cartItemService, ProductService productService)
        {
            _cartItemService = cartItemService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            var cartItems = _cartItemService.GetAllCartItems();
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
    }
}
