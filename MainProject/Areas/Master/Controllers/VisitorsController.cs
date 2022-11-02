using Microsoft.AspNetCore.Mvc;

namespace TMS.Areas.Master.Controllers
{
    public class VisitorsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
