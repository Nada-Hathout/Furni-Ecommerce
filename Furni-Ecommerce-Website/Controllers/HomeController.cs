using System.Diagnostics;
using BusinessLogic.Service;
using DataAccess.Models;
using Furni_Ecommerce_Website.Models;
using Microsoft.AspNetCore.Mvc;

using Furni_Ecommerce_Shared.UserViewModel;
using BusinessLogic.Repository;

namespace Furni_Ecommerce_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ProductService productService;
        ProductsAndCommentsViewModel product_comments ;
        ReviewService reviewService;

        public HomeController(ILogger<HomeController> logger, ProductService productService , ProductsAndCommentsViewModel prd_comments, ReviewService reviewService)
        {
            _logger = logger;
            this.productService = productService;
            this.product_comments = prd_comments;
            this.reviewService = reviewService;

        }

        public IActionResult Index()
        {
            List<Product> products = productService.productRepository.GetAll();
            List<Review> reviews=reviewService.reviewRepository.GetAll();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
