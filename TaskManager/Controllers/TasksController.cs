using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
