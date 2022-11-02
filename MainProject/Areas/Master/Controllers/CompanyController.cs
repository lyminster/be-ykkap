using AutoMapper;
using ClosedXML.Excel;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.Models;
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

namespace TMS.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        CompanyDataAccessLayer DALCompany;
        FileLogDataAccessLayer DALFileLog;

        public CompanyController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALCompany = new CompanyDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            List<JsonCompanyVM> page1 = DALCompany.FindAsync(new JsonCompanyVM { }, User);
            IndexCompanyVM data = new IndexCompanyVM();
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


                var customerData = DALCompany.FindAsync(new JsonCompanyVM { }, User);



                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Code.Contains(searchValue)
                        || m.CreatedBy.Contains(searchValue)
                        || m.Name.Contains(searchValue)).ToList();
                }

                //total number of rows counts   
                recordsTotal = customerData.Count();
                //Paging   
                var data = customerData.Skip(skip).Take(pageSize).ToList();
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
        public IActionResult Create(JsonCompanyVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var retrunSave = DALCompany.SaveAsync(data, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Company", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public IActionResult View(String formID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            var data = DALCompany.GetCompanyAsync(formID);
            return View(data);
        }
        public IActionResult Edit(String ID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            var data = DALCompany.GetCompanyAsync(ID);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CompanyVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<CompanyVM, JsonCompanyVM>(data);
                var retrunSave = DALCompany.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Company", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public string CopyFile(IFormFile fileInput)
        {

            var fileName2 = System.IO.Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 7) + "-" + fileInput.FileName);
            // Create new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(Path.Combine(_hostenv.WebRootPath, "Upload/") + fileName2))
            using (var uploadedFile = fileInput.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return fileName2;
        }

        public IActionResult Upload()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            IndexCompanyVM data = new IndexCompanyVM();
            var idLog = TempData["idLog"];
            if (idLog != null)
            {
                data.UrlFileLog = idLog.ToString();
            }

            return View(data);
        }
        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALCompany.DeleteAsync(id, User);
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

        [HttpPost]
        public IActionResult Upload(IndexCompanyVM file)
        {
            string _newFileName = "";
            string logpath = String.Empty;

            try
            {
                if (file.Upload == null)
                {
                    ModelState.AddModelError("FileURL", "Please upload file");
                    return View();
                }

                var idLog = Guid.NewGuid().ToString();

                if (file.Upload.FileName != null)
                {
                    //upload
                    var filename = GlobalHelpers.CopyFile(file.Upload, _hostenv);
                    var _path = Path.Combine(_hostenv.WebRootPath, "Upload/" + filename);


                    string errMessage = "Succes Upload";

                    //buat header log file dulu, dan kasi guid
                    FileLogVM dataLog = new FileLogVM();
                    dataLog.ID = idLog;
                    dataLog.FileName = filename;
                    dataLog.TableName = ConstantHelper.MSCompany;
                    dataLog.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                    dataLog.CreatedTime = DateTime.Now;
                    DALFileLog.Create(dataLog);

                    JsonReturn jsonresult = DALCompany.GenerateData(filename, _path, idLog, GlobalHelpers.GetEmailFromIdentity(User), out errMessage, out logpath);
                    if (jsonresult.result == false)
                    {
                        Alert(jsonresult.message, NotificationType.warning);
                    }
                    else
                    {
                        Alert("Finish process upload data! Please check Log", NotificationType.success);
                    }
                    try
                    {
                        if (System.IO.File.Exists(_path))
                        {
                            System.IO.File.Delete(_path);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }

                ViewBag.Message = "File Uploaded Successfully!!";

                file.UrlFileLog = idLog;
                TempData["idLog"] = idLog;
                return RedirectToAction("Upload", "Company", new { area = "Master" });

            }
            catch (Exception ex)
            {
                string err = "File upload failed!!" + ex.Message;
                ViewBag.Message = err;
                return RedirectToAction("Index", "Company", new { area = "Master", errorMessage = err });
            }
        }

        public IActionResult DownloadExcelDocument()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Company_Export.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Company Export");
                    worksheet.Cell(1, 1).Value = "CompanyCode";
                    worksheet.Cell(1, 2).Value = "CompanyName";

                    List<JsonCompanyVM> page1 = DALCompany.FindAsync(new JsonCompanyVM { }, User);

                    for (int index = 1; index <= page1.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = "'" + page1[index - 1].Code;
                        worksheet.Cell(index + 1, 2).Value = page1[index - 1].Name;

                    }
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult DownloadExcelTemplate()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Template_Company.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Template Company");
                    worksheet.Cell(1, 1).Value = "CompanyCode";
                    worksheet.Cell(1, 2).Value = "CompanyName";

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
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

