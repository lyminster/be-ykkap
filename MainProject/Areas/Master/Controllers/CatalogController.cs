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
    public class CatalogController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        CatalogDetailDataAccessLayer DALCatalogDetail;
        FileLogDataAccessLayer DALFileLog;


        public CatalogController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALCatalogDetail = new CatalogDetailDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            List<JsonCatalogDetailVM> page1 = DALCatalogDetail.FindAsync(new JsonCatalogDetailVM { }, User);
            IndexCatalogDetalVM data = new IndexCatalogDetalVM();
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


                var catalogDetailData = DALCatalogDetail.FindAsync(new JsonCatalogDetailVM { }, User);



                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    catalogDetailData = catalogDetailData.Where(m =>
                        m.CreatedBy.Contains(searchValue)
                        || m.name.Contains(searchValue)
                        || m.description.Contains(searchValue)
                        || m.CatalogType.Contains(searchValue)
                        || m.enPdfUrl.Contains(searchValue)
                        || m.imgUrl.Contains(searchValue)
                        || m.idPdfUrl.Contains(searchValue)).ToList();
                }

                //total number of rows counts   
                recordsTotal = catalogDetailData.Count();
                //Paging   
                var data = catalogDetailData.Skip(skip).Take(pageSize).ToList();
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
                return RedirectToAction("LoginForm", "Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(JsonCatalogDetailVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var retrunSave = DALCatalogDetail.SaveAsync(data, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Catalog Detail", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public IActionResult Edit(String ID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            var data = DALCatalogDetail.GetCatalogDetailbyId(ID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CatalogDetailVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<CatalogDetailVM, JsonCatalogDetailVM>(data);
                var retrunSave = DALCatalogDetail.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Catalog Detail", NotificationType.success);
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
                var returns = DALCatalogDetail.DeleteAsync(id, User);
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
