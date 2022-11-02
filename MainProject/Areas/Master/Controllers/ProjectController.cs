using Microsoft.AspNetCore.Mvc;

namespace TMS.Areas.Master.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
