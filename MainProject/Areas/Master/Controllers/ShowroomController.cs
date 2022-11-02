using Microsoft.AspNetCore.Mvc;

namespace TMS.Areas.Master.Controllers
{
    public class ShowroomController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
