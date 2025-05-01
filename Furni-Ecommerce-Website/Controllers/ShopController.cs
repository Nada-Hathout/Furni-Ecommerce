using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Mvc;
using DataAccess;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace Furni_Ecommerce_Website.Controllers
{
    
    public class ShopController : Controller
    {
        private readonly IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }
        public IActionResult Index()
        {
            var products = _productService.GetProducts();
            return View("Index", products.ToList());

        }
        [HttpGet]
        public IActionResult SearchProduct(string keyword, int? minPrice, int? maxPrice, int page=1, int pageSize=8  )
        {
            //if(string.IsNullOrEmpty(keyword))
            // {
            //     var allProducts = _productService.GetProducts();
            //     return Json(allProducts);
            // }
            //var result =  _productService.SearchProduct(keyword);
            // return Json(result);
            var allProducts = string.IsNullOrEmpty(keyword)
             ? _productService.GetProducts()
             : _productService.SearchProduct(keyword);
            if (minPrice.HasValue)
            {
               allProducts = allProducts.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                 allProducts = allProducts.Where(p => p.Price <= maxPrice.Value);
            }

            var totalCount = allProducts.Count();

            var pagedProducts = allProducts.Skip((page -1) * pageSize)
                .Take(pageSize).ToList();

            return Json(new { data = pagedProducts, total = totalCount });
        }
      

    }
}
