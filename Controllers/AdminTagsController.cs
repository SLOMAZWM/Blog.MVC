using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    public class AdminTagsController : Controller
    {
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
