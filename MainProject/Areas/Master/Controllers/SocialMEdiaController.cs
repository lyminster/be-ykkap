using AutoMapper;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel.ViewModels;
using ViewModel.ViewModels.Master;

namespace TMS.Areas.Master.Controllers
{
    [Area("Master")]
    public class SocialMediaController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        SocialMediaDataAccessLayer DALSocialMedia;
        FileLogDataAccessLayer DALFileLog;


        public SocialMediaController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALSocialMedia = new SocialMediaDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            List<JsonSocialMediaVM> page1 = DALSocialMedia.FindAsync(new JsonSocialMediaVM { }, User);
            IndexSocialMediaVM data = new IndexSocialMediaVM();
            data.listIndex = page1;

            return View(data);
        }

        public ActionResult LoadData(FilterTgl filters)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                /// INI KALAU DI PAKE AJA BUAT CONTOH TRANSAKSI LAEN 
                //DateTime? from = !String.IsNullOrEmpty(filters.tglDari) ? Convert.ToDateTime(filters.tglDari) : null;
                //string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                //DateTime? to = !String.IsNullOrEmpty(filters.tglSampai) ? Convert.ToDateTime(filters.tglSampai) : null;
                //string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;


                var socialData = DALSocialMedia.FindAsync(new JsonSocialMediaVM { }, User);



                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    socialData = socialData.Where(m =>
                        m.CreatedBy.Contains(searchValue)
                        || m.urlFb.Contains(searchValue)
                        || m.urlIg.Contains(searchValue)
                        || m.urlWeb.Contains(searchValue)
                        || m.urlYt.Contains(searchValue)).ToList();
                }

                //total number of rows counts   
                recordsTotal = socialData.Count();
                //Paging   
                var data = socialData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult Create()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SocialMediaVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var saveData = _mapper.Map<SocialMediaVM, JsonSocialMediaVM>(data);
                var retrunSave = DALSocialMedia.SaveAsync(saveData, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create SocialMedia", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public IActionResult EditNew()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }

            var data = DALSocialMedia.GetSocialMediaFirstAsync();
            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNew(SocialMediaVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<SocialMediaVM, JsonSocialMediaVM>(data);
                var retrunSave = DALSocialMedia.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update SocialMedia", NotificationType.success);
                    return View(upddata);
                }
                else
                {
                    Alert(errMsg, NotificationType.error);
                    return View(upddata);
                }
                
            }
            var upddata2 = _mapper.Map<SocialMediaVM, JsonSocialMediaVM>(data);
            return View(upddata2);
        }


        public IActionResult Edit(String ID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }

            var data = DALSocialMedia.GetSocialMediaAsync(ID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SocialMediaVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<SocialMediaVM, JsonSocialMediaVM>(data);
                var retrunSave = DALSocialMedia.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update SocialMedia", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALSocialMedia.DeleteAsync(id, User);
                if (returns.result == true)
                {
                    return Json("Success");
                }
                else
                {
                    return Json(returns.message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
