using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
