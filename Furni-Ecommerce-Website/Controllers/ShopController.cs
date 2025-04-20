using Microsoft.AspNetCore.Mvc;

namespace Furni_Ecommerce_Website.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
