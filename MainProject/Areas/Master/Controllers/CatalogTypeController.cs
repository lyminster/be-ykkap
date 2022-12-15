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
    public class CatalogTypeController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        private IConfiguration _config;
        CatalogTypeDataAccessLayer DALCatalogType;
        FileLogDataAccessLayer DALFileLog;


        public CatalogTypeController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            _config = configfile;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALCatalogType = new CatalogTypeDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            List<JsonCatalogTypeVM> page1 = DALCatalogType.FindAsync(new JsonCatalogTypeVM { }, User);
            IndexCatalogTypeVM data = new IndexCatalogTypeVM();
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


                var catalogTypeData = DALCatalogType.FindAsync(new JsonCatalogTypeVM { }, User);



                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn == "Name")
                        {
                            catalogTypeData = catalogTypeData.OrderBy(x => x.name).ToList();
                        }
                        else if (sortColumn == "Description")
                        {
                            catalogTypeData = catalogTypeData.OrderBy(x => x.description).ToList();
                        } 
                    }
                    else
                    {
                        if (sortColumn == "Name")
                        {
                            catalogTypeData = catalogTypeData.OrderByDescending(x => x.name).ToList();
                        }
                        else if (sortColumn == "Description")
                        {
                            catalogTypeData = catalogTypeData.OrderByDescending(x => x.description).ToList();
                        }
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    catalogTypeData = catalogTypeData.Where(m =>
                        m.name.ToLower().Contains(searchValue.ToLower())
                        || m.description.ToLower().Contains(searchValue.ToLower())
                        || Convert.ToString(m.OrderNo).Contains(searchValue)).ToList();
                }

                //total number of rows counts   
                recordsTotal = catalogTypeData.Count();
                //Paging   
                var data = catalogTypeData.Skip(skip).Take(pageSize).ToList();
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
        public IActionResult Create(CatalogTypeVM data)
        {
          
            if (ModelState.IsValid)
            {
                var filename = "";
                string errMsg = "";



                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlCatalogImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.imgUrl = filename;
                }


                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var SaveData = _mapper.Map<CatalogTypeVM, JsonCatalogTypeVM>(data);
                var retrunSave = DALCatalogType.SaveAsync(SaveData, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Catalog Type", NotificationType.success);
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
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            var data = DALCatalogType.GetCatalogTypeBYIdAsync(ID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CatalogTypeVM data)
        {
            if (ModelState.IsValid)
            {
                var filename = "";
                string errMsg = "";
                 
                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlCatalogImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.imgUrl = filename;
                }


                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<CatalogTypeVM, JsonCatalogTypeVM>(data);
                var retrunSave = DALCatalogType.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Catalog Type", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(upddata);
            }
            var upddata2 = _mapper.Map<CatalogTypeVM, JsonCatalogTypeVM>(data);
            return View(upddata2);
        }

        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALCatalogType.DeleteAsync(id, User);
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
