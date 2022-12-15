using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TMS.Areas.Master.Controllers
{
    [Area("Master")]
    public class ShowroomsController : Controller
    {
        // GET: ShowroomsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ShowroomsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShowroomsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShowroomsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShowroomsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShowroomsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShowroomsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShowroomsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
