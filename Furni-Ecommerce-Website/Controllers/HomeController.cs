using System.Diagnostics;
using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Website.Models;
using Microsoft.AspNetCore.Mvc;

using Furni_Ecommerce_Shared.UserViewModel;
using BusinessLogic.Repository;
using System.Collections.Generic;
using System.Security.Claims;

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
            List<ProductsAndCommentsViewModel> products = productService.GetProductsInfo();
            return View(products);
        }

        public IActionResult PrdDetails(int id)
        {
            var prd = productService.getDetails(id);
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

            return View("Favorite");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Toggle(int productId)
        {
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isFavourite = favoriteService.ToggleFavourite(userId, productId);
            return Json(new { isFavourite });
        }

    }
}
