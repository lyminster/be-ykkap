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
        private IConfiguration _config;
        HelperTableDataAccessLayer DALHelper;
        CatalogDetailDataAccessLayer DALCatalogDetail;
        CatalogTypeDataAccessLayer DALCatalogType;
        FileLogDataAccessLayer DALFileLog;


        public CatalogController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            _config = configfile;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALCatalogDetail = new CatalogDetailDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALCatalogType = new CatalogTypeDataAccessLayer(_context, _mapper, _hostenv, configfile);
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
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn == "Name")
                        {
                            catalogDetailData = catalogDetailData.OrderBy(x => x.name).ToList();
                        }
                        else if (sortColumn == "Catalog Type")
                        {
                            catalogDetailData = catalogDetailData.OrderBy(x => x.CatalogTypeName).ToList();
                        }
                        else if (sortColumn == "Description")
                        {
                            catalogDetailData = catalogDetailData.OrderBy(x => x.description).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn == "Name")
                        {
                            catalogDetailData = catalogDetailData.OrderByDescending(x => x.name).ToList();
                        }
                        else if (sortColumn == "Catalog Type")
                        {
                            catalogDetailData = catalogDetailData.OrderByDescending(x => x.CatalogTypeName).ToList();
                        }
                        else if (sortColumn == "Description")
                        {
                            catalogDetailData = catalogDetailData.OrderByDescending(x => x.description).ToList();
                        }
                    }
                }
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
            CatalogDetailVM data = new CatalogDetailVM();
            
            data.ListCatalogType = _mapper.Map<List<CatalogType>, List<JsonCatalogTypeVM>>(DALCatalogType.GetListCatalogTypeAsync());
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CatalogDetailVM data)
        {
            if (ModelState.IsValid)
            {
                var filename = "";


                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlCatalogImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.imgUrl = filename;
                }

                data.imgUrl = filename;
                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var SaveData = _mapper.Map<CatalogDetailVM, JsonCatalogDetailVM>(data);
                var retrunSave = DALCatalogDetail.SaveAsync(SaveData, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Catalog Detail", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                data.ListCatalogType = _mapper.Map<List<CatalogType>, List<JsonCatalogTypeVM>>(DALCatalogType.GetListCatalogTypeAsync());
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
            data.ListCatalogType = _mapper.Map<List<CatalogType>, List<JsonCatalogTypeVM>>(DALCatalogType.GetListCatalogTypeAsync());
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CatalogDetailVM data)
        {
            if (ModelState.IsValid)
            {
                if (data.Upload == null)
                {
                    ModelState.AddModelError("FileURL", "Please upload file");
                    return View();
                }
                var filename = "";

                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlCatalogImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.imgUrl = filename;
                }

                
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<CatalogDetailVM, JsonCatalogDetailVM>(data);
                var retrunSave = DALCatalogDetail.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Catalog Detail", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                upddata.ListCatalogType = _mapper.Map<List<CatalogType>, List<JsonCatalogTypeVM>>(DALCatalogType.GetListCatalogTypeAsync());
                Alert(errMsg, NotificationType.error);
                return View(upddata);
            }
            
            var upddata2 = _mapper.Map<CatalogDetailVM, JsonCatalogDetailVM>(data);
            upddata2.ListCatalogType = _mapper.Map<List<CatalogType>, List<JsonCatalogTypeVM>>(DALCatalogType.GetListCatalogTypeAsync());
            return View(upddata2);
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
