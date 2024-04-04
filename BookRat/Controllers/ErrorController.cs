using Microsoft.AspNetCore.Mvc;

namespace BookRat.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
