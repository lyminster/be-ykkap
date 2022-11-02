using Microsoft.AspNetCore.Mvc;

namespace TMS.Areas.Master.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
