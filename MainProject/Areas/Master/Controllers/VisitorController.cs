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
    public class VisitorController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        VisitorDataAccessLayer DALVisitor;
        FileLogDataAccessLayer DALFileLog;

        public VisitorController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALVisitor = new VisitorDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }
        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            //List<JsonVisitorVM> data = DALVisitor.FindAsync(new JsonVisitorVM { }, User);
            VisitorVM data = new VisitorVM();
            data.FilterFromString = DateTime.Now.ToString("01-MMM-yyyy");
            data.FilterToString = DateTime.Now.ToString("dd-MMM-yyyy");
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
                DateTime? from = !String.IsNullOrEmpty(filters.tglDari) ? Convert.ToDateTime(filters.tglDari) : null;
                string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                DateTime? to = !String.IsNullOrEmpty(filters.tglSampai) ? Convert.ToDateTime(filters.tglSampai) : null;
                string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;
                
                var visitorData = DALVisitor.FindAsync(new JsonVisitorVM { }, User, from, to);

                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn == "Name")
                        {
                            visitorData = visitorData.OrderBy(x => x.Name).ToList();
                        }
                        else if (sortColumn == "Email")
                        {
                            visitorData = visitorData.OrderBy(x => x.Email).ToList();
                        }
                        else if (sortColumn == "Phone Number")
                        {
                            visitorData = visitorData.OrderBy(x => x.PhoneNumber).ToList();
                        }
                        else if (sortColumn == "Access From")
                        {
                            visitorData = visitorData.OrderBy(x => x.AccessFrom).ToList();
                        }
                        else if (sortColumn == "Created Time")
                        {
                            visitorData = visitorData.OrderBy(x => x.CreatedTime).ToList();
                        }
                        
                    }
                    else
                    {
                        if (sortColumn == "Name")
                        {
                            visitorData = visitorData.OrderByDescending(x => x.Name).ToList();
                        }
                        else if (sortColumn == "Email")
                        {
                            visitorData = visitorData.OrderByDescending(x => x.Email).ToList();
                        }
                        else if (sortColumn == "Phone Number")
                        {
                            visitorData = visitorData.OrderByDescending(x => x.PhoneNumber).ToList();
                        }
                        else if (sortColumn == "Access From")
                        {
                            visitorData = visitorData.OrderByDescending(x => x.AccessFrom).ToList();
                        }
                        else if (sortColumn == "Created Time")
                        {
                            visitorData = visitorData.OrderByDescending(x => x.CreatedTime).ToList();
                        }
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    visitorData = visitorData.Where(m =>
                        m.CreatedBy.Contains(searchValue)
                        || m.Name.Contains(searchValue)
                        || m.PhoneNumber.Contains(searchValue)
                        || m.Email.Contains(searchValue)
                        || m.AccessFrom.Contains(searchValue)).ToList();
                }

                //total number of rows counts   
                recordsTotal = visitorData.Count();
                //Paging   
                var data = visitorData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALVisitor.DeleteAsync(id, User);
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

        public IActionResult DownloadExcelDocument()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Visitor_Export.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("Visitor Export");
                    worksheet.Cell(1, 1).Value = "Name";
                    worksheet.Cell(1, 2).Value = "Email";
                    worksheet.Cell(1, 3).Value = "Phone Number";
                    worksheet.Cell(1, 4).Value = "Access From";
                    worksheet.Cell(1, 5).Value = "Access Date";

                    var data = DALVisitor.FindAsync(new JsonVisitorVM { }, User, null, null);

                    for (int index = 1; index <= data.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = data[index - 1].Name;
                        worksheet.Cell(index + 1, 2).Value = data[index - 1].Email;

                        worksheet.Cell(index + 1, 3).Value = "'" + data[index - 1].PhoneNumber;
                        worksheet.Cell(index + 1, 4).Value = data[index - 1].AccessFrom;

                        var claimdate = "'" + data[index - 1].CreatedTime.ToString(ConstantHelper.Date_SAP);
                        worksheet.Cell(index + 1, 5).Value = claimdate;

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

    }
}
