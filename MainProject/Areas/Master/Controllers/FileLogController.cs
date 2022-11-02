using AutoMapper;
using ClosedXML.Excel;
using DAL.DataAccessLayer.Master;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MainProject.Helpers;
using ViewModel.Constant;
using DAL.Helper;
using ViewModel.ViewModels;


namespace IISHOSTV1.Areas.Master.Controllers
{
    [Area("Master")]
    public class FileLogController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;

        FileLogDataAccessLayer DALFileLog;

        public FileLogController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }


        public IActionResult Index()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            //List<FileLogVM> page1 = DALFileLog.GetListFileLog(new FileLogVM { });
            IndexFileLogVM data = new IndexFileLogVM();

            data.FilterFromString = DateTime.Now.AddDays(-3).ToString("dd-MMM-yyyy");
            data.FilterToString = DateTime.Now.ToString("dd-MMM-yyyy");
            data.ListFilterJenis = DALFileLog.GetAllCreatedByFileLog().ToList();

            //data.listIndex = page1;
            return View(data);
        }


        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "swal('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "";
            TempData["notification"] = msg;
        }

        public IActionResult IndexTransporter()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("LoginForm", "Login");
            }
            //List<FileLogVM> page1 = DALFileLog.GetListFileLog(new FileLogVM { });
            IndexFileLogVM data = new IndexFileLogVM();

            data.FilterFromString = DateTime.Now.AddDays(-3).ToString("dd-MMM-yyyy");
            data.FilterToString = DateTime.Now.ToString("dd-MMM-yyyy");
            data.ListFilterJenis = DALFileLog.GetAllCreatedByFileLog().ToList();



            //data.listIndex = page1;
            return View(data);
        }

        public enum NotificationType
        {
            error,
            success,
            warning,
            info
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


                //DateTime? from = !String.IsNullOrEmpty(filters.tglDari) ? Convert.ToDateTime(filters.tglDari) : null;
                //string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                //DateTime? to = !String.IsNullOrEmpty(filters.tglSampai) ? Convert.ToDateTime(filters.tglSampai) : null;
                //string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;


                var customerData = DALFileLog.GetListFileLog(new FileLogVM { });



                //Sorting  
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.TableName.Contains(searchValue)
                        || m.CreatedBy.Contains(searchValue)
                        || m.FileName.Contains(searchValue)).ToList();
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




        public ActionResult LoadDataTransporterLog(FilterTgl filters)
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


                DateTime? from = !String.IsNullOrEmpty(filters.tglDari) ? Convert.ToDateTime(filters.tglDari) : null;
                string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                DateTime? to = !String.IsNullOrEmpty(filters.tglSampai) ? Convert.ToDateTime(filters.tglSampai) : null;
                string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;
                IndexFileLogVM filter = new IndexFileLogVM();

                filter.FilterDONumber = filters.filter;


                //gw comment dulu
                List<FileLogDetailVM> customerData = new List<FileLogDetailVM>();
                //if (String.IsNullOrEmpty(filters.filter) || String.IsNullOrEmpty(convertFrom) || String.IsNullOrEmpty(convertTo))
                //{
                //    Alert("please fill all filter parameter for searching..", NotificationType.warning);

                //}
                //else
                //{
                //}


                customerData = DALFileLog.FilterFileLogTransporter(new IndexFileLogVM { }, null, null, false).ToList();
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

        [HttpPost]
        public IActionResult DownloadExcelDocument(FilterTgl filters)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "ExportTransporterLog.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("DO Export");
                    worksheet.Cell(1, 1).Value = "No";
                    worksheet.Cell(1, 2).Value = "Remarks";
                    worksheet.Cell(1, 3).Value = "Created By";
                    worksheet.Cell(1, 4).Value = "Created Time";

                    IndexFileLogVM filter = new IndexFileLogVM();

                    filter.FilterDONumber = filters.filterJenis;



                    DateTime? from = !String.IsNullOrEmpty(filters.FilterFromString) ? Convert.ToDateTime(filters.FilterFromString) : null;
                    string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                    DateTime? to = !String.IsNullOrEmpty(filters.FilterToString) ? Convert.ToDateTime(filters.FilterToString) : null;
                    string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;




                    List<FileLogDetailVM> page1 = DALFileLog.FilterFileLogTransporter(filter,
                        convertFrom, convertTo, false);

                    for (int index = 1; index <= page1.Count; index++)
                    {
                        worksheet.Cell(index + 1, 2).Value = page1[index - 1].Remarks;
                        worksheet.Cell(index + 1, 3).Value = page1[index - 1].CreatedBy;
                        worksheet.Cell(index + 1, 1).Value = page1[index - 1].CodeData;
                        worksheet.Cell(index + 1, 4).Value = page1[index - 1].CreatedTime;


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

        public IActionResult ExportFileLogDetail(string formID)
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "FileLogDetail.xlsx";
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet =
                    workbook.Worksheets.Add("FileLogDetail");
                    worksheet.Cell(1, 1).Value = "No";
                    worksheet.Cell(1, 2).Value = "Remarks";
                    worksheet.Cell(1, 3).Value = "Created By";
                    worksheet.Cell(1, 4).Value = "Created Time";

                    List<FileLogDetailVM> page1 = DALFileLog.FilterFileLogDetailSingle(formID).OrderBy(x => x.OrderNo).ToList();

                    for (int index = 1; index <= page1.Count; index++)
                    {
                        worksheet.Cell(index + 1, 1).Value = page1[index - 1].OrderNo;
                        worksheet.Cell(index + 1, 2).Value = page1[index - 1].Remarks;
                        worksheet.Cell(index + 1, 3).Value = page1[index - 1].CreatedBy;
                        worksheet.Cell(index + 1, 4).Value = page1[index - 1].CreatedTime;

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

