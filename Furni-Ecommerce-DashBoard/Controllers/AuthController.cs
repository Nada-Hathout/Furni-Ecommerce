using Microsoft.AspNetCore.Mvc;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
