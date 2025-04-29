using System.Diagnostics;
using BusinessLogic.Service;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Furni_Ecommerce_Website.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using BusinessLogic.Repository;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;

namespace Furni_Ecommerce_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService productService;
        private readonly IReviewService reviewService;
        private readonly IFavoriteService favoriteService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IReviewService reviewService, IFavoriteService favoriteService)
        {
            _logger = logger;
            this.productService = productService;
            this.reviewService = reviewService;
            this.favoriteService = favoriteService;

        }

        public IActionResult Index()
        {
            var userId= User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo(userId);
            var favCount = favoriteService.GetFavoriteCount(userId);

            ViewBag.FavoriteCount = favCount;
            return View(products);
        }

        public IActionResult PrdDetails(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var prd = productService.GetDetails(id);
            var favCount = favoriteService.GetFavoriteCount(userId);

            ViewBag.FavoriteCount = favCount;
            if (prd == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.prd_Id = prd.Id;
                return View("PrdDetails", prd);

            }


        }
        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult Favorite()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List< FavouriteViewModel> FavouritePrd = favoriteService.GetFavProducts(userId);
            var favCount = favoriteService.GetFavoriteCount(userId);

            ViewBag.FavoriteCount = favCount;
            return View("Favorite",FavouritePrd);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [Authorize]
        public JsonResult Toggle(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Json(new { isFavourite = false, error = "User not authenticated" });
            }

            var isFavourite = favoriteService.ToggleFavourite(userId, productId);
            var updatedCount = favoriteService.GetFavoriteCount(userId); // ???? ??? ????? ??????

            return Json(new
            {
                isFavourite,
                updatedFavCount = updatedCount
            });
        }



        public IActionResult About()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo(userId);
            return View(products);
        }

        public IActionResult Blog()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo(userId);
            return View(products);
        }
        public IActionResult Services()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo(userId);
            return View(products);
        }
        public IActionResult Contact()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo(userId);
            return View(products);
        }


    }
}
