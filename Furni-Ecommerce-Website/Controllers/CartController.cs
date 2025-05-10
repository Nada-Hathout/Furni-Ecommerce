using System.Security.Claims;
using BusinessLogic.Service;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Furni_Ecommerce_Website.Controllers
{
    [Authorize(Roles ="User")]
    public class CartController : Controller
    {
      private readonly  ICartService cartService;
        private readonly IFavoriteService favoriteService;

        public CartController(ICartService cartService, IFavoriteService favoriteService)
        { 
            this.cartService = cartService;
            this.favoriteService = favoriteService;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult AddItemToCart(int productId)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Auth") });
            }
            var UserIdClaim = User.FindFirst("UserId");
            if(UserIdClaim == null)
            {
                return Json(new { success = false, message = "UnAuthorize Access" });   
            }
            string UserId = UserIdClaim.Value;
            cartService.AddItemToCart(UserId, productId);
        
            return Json(new { success = true });
           
        }


        [HttpGet]
        public IActionResult GetCartItemsCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = cartService.GetCartItemsCountByUserId(userId);
            return Json(new {count = count});
        }

        [HttpGet]
        public IActionResult GetFavCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = favoriteService.GetFavItemsCountByUserId(userId);
            return Json(new { count = count });
        }


    }
}
