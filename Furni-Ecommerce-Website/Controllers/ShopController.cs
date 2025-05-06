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
        private readonly IFavoriteService _favoriteService;

        public ShopController(IProductService productService, IFavoriteService favoriteService)
        {
            this._productService = productService;
            this._favoriteService = favoriteService;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var products = _productService.GetProducts(userId);
            var favCount = _favoriteService.GetFavoriteCount(userId);

            ViewBag.FavoriteCount = favCount;
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allProducts = string.IsNullOrEmpty(keyword)
             ? _productService.GetProducts(userId).Where(p=>p.Stock>0)
             : _productService.SearchProduct(keyword,userId).Where(p=>p.Stock>0);
            if (minPrice.HasValue)
            {
               allProducts = allProducts.Where(p => p.Price >= minPrice.Value &&p.Stock>0 );
            }

            if (maxPrice.HasValue)
            {
                 allProducts = allProducts.Where(p => p.Price <= maxPrice.Value &&p.Stock>0);
            }

            var totalCount = allProducts.Count();

            var pagedProducts = allProducts.Skip((page -1) * pageSize)
                .Take(pageSize).ToList();

            return Json(new { data = pagedProducts, total = totalCount });
        }
      

    }
}
