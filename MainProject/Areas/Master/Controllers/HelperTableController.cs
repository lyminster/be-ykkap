using AutoMapper;
using ClosedXML.Excel;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.Models;
using DAL.Helper;
using Database.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Helpers;
using ViewModel.Constant;
using ViewModel.ViewModels;

namespace IISHOSTV1.Areas.Master.Controllers
{
    [Area("Master")]
    public class HelperTableController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        public HelperTableController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
        }
        public IActionResult Index()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            List<HelperTableVM> page1 = DALHelper.FilterHelper(null);
            IndexHelperTableVM data = new IndexHelperTableVM();
            data.listIndex = page1;
            return View(data);
        }

        public IActionResult Create()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HelperTableVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var retrunSave = DALHelper.Create(data);
                if (retrunSave.result == true)
                {
                    Alert("Success Create HelperTable", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public IActionResult Edit(string formID)
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            var data = DALHelper.GetHelperTable(formID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HelperTableVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var retrunSave = DALHelper.Edit(data);
                if (retrunSave.result == true)
                {
                    Alert("Success Update HelperTable", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }


        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }

        public enum NotificationType
        {
            error,
            success,
            warning,
            info
        }

    }
}
