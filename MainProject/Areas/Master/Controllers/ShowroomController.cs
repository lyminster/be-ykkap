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
    public class ShowroomController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        HelperTableDataAccessLayer DALHelper;
        ShowroomDataAccessLayer DALShowroom;
        FileLogDataAccessLayer DALFileLog;


        public ShowroomController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            _config = configfile;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALShowroom = new ShowroomDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }

            return View();
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


                var showroomData = DALShowroom.FindAsync(new JsonShowroomVM { }, User);



                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn == "Name")
                        {
                            showroomData = showroomData.OrderBy(x => x.name).ToList();
                        }
                        else if (sortColumn == "Working Hour")
                        {
                            showroomData = showroomData.OrderBy(x => x.workingHour).ToList();
                        }
                        else if (sortColumn == "Address")
                        {
                            showroomData = showroomData.OrderBy(x => x.address).ToList();
                        }
                        else if (sortColumn == "Telepohone Number")
                        {
                            showroomData = showroomData.OrderBy(x => x.telephone).ToList();
                        }
                      
                    }
                    else
                    {
                        if (sortColumn == "Name")
                        {
                            showroomData = showroomData.OrderByDescending(x => x.name).ToList();
                        }
                        else if (sortColumn == "Working Hour")
                        {
                            showroomData = showroomData.OrderByDescending(x => x.workingHour).ToList();
                        }
                        else if (sortColumn == "Address")
                        {
                            showroomData = showroomData.OrderByDescending(x => x.address).ToList();
                        }
                        else if (sortColumn == "Telepohone Number")
                        {
                            showroomData = showroomData.OrderByDescending(x => x.telephone).ToList();
                        }
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    showroomData = showroomData.Where(m =>
                        m.name.Contains(searchValue)
                        || m.address.Contains(searchValue)
                        || m.telephone.Contains(searchValue)
                        || m.workingHour.Contains(searchValue) ).ToList();
                }

                //total number of rows counts   
                recordsTotal = showroomData.Count();
                //Paging   
                var data = showroomData.Skip(skip).Take(pageSize).ToList();
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
        public IActionResult Create(ShowroomVM data)
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
                    String folder = _config.GetConnectionString("UrlShowroomImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.urlImage = filename;
                }




                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);

                var SaveData = _mapper.Map<ShowroomVM, JsonShowroomVM>(data);
                var retrunSave = DALShowroom.SaveAsync(SaveData, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Showroom", NotificationType.success);
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
            var data = DALShowroom.GetShowroomAsync(ID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ShowroomVM data)
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
                    String folder = _config.GetConnectionString("UrlShowroomImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.urlImage = filename;
                }

                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);

                var upddata = _mapper.Map<ShowroomVM, JsonShowroomVM>(data);
                var retrunSave = DALShowroom.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Showroom", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(upddata);
            }
            var upddataErr = _mapper.Map<ShowroomVM, JsonShowroomVM>(data);
            return View(upddataErr);
        }

        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALShowroom.DeleteAsync(id, User);
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
