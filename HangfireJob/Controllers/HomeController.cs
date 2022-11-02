using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

using Database.Models;

using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Database.ConfigClass;
using Hangfire;
using System;
using DAL.DataAccessLayer.JOB;
using HangfireJob.Models;
using HangfireJob.Service;
using Hangfire.Storage;
using System.IO;
using System.Linq;
using Database.ViewModels;
using System.Collections.Generic;
using DAL.DataAccessLayer;
using HangfireJob.Services;
using EmailService = HangfireJob.Services.EmailService;

namespace hangfire.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _config;
        private readonly SystemConfig _systemConfig;
        private readonly EmailConfig _emailConfig;
        private readonly BusinessModelContext _context;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostenv;
        public HomeController(ILogger<HomeController> logger,
                                UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager
            , BusinessModelContext dBContext
            , SystemConfig systemConfig, EmailConfig emailConfig, IMapper _mapper, IHostingEnvironment hostenv, IConfiguration config
            )
        {
            _logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            _config = config;
            _context = dBContext;
            _emailConfig = emailConfig;
            _systemConfig = systemConfig;
            _hostenv = hostenv;
        }

        public IActionResult Index()
        {


            return View();
        }

        public IActionResult GenerateTestingData()
        {


            //String DepanDO = "DO2022TESTING";
            //// File DO Created
            //String PathFileExample = Path.Combine(_systemConfig.PathFileDO, "DOCREATEDSAP" + DateTime.Now.ToString("dd_MMM_yyyy_HH_mm") + ".txt");
            //using StreamWriter file = new(PathFileExample, append: true);

            //for (int i = 0; i < 5; i++)
            //{
            //    String DONumber = DepanDO + i.ToString().PadLeft(5, '0');
            //    String DODate = DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd HH:mm");
            //    String StoreCode = "KS01";
            //    String DCCode = "DCGI";
            //    String SBU = "9E";


            //    String RowData = DONumber.ToUpper() + ";" + DODate + ";" + StoreCode + ";" + DCCode + ";" + SBU;

            //    file.WriteLine(RowData);
            //}








            //  File DO Shipment
            //String PathFileDOShipmentExample = Path.Combine(_systemConfig.PathFileDOShipment, "DOSHIPMENT" + DateTime.Now.ToString("dd_MMM_yyyy_HH_mm") + ".txt");
            //using StreamWriter fileDOShipment = new(PathFileDOShipmentExample, append: true);

            //for (int i = 0; i < 5; i++)
            //{
            //    String DONumber = DepanDO + i.ToString().PadLeft(5, '0');
            //    String DOPackDate = DateTime.Now.AddDays(i + 3).ToString("yyyyMMdd");
            //    String DOLoadingDate = DateTime.Now.AddDays(i + 3).ToString("yyyyMMdd");
            //    String DOLoadingTime = DateTime.Now.AddDays(i + 6).ToString("yyyyMMdd");
            //    String TransporterName = "SONJAYA";
            //    String QtyLoadedPCS = (i + 30).ToString();
            //    String QtyLoadedCTN = (i + 15).ToString();

            //    String RowData = DONumber.ToUpper() + ";" + DOPackDate + ";" + DOLoadingDate + ";" + DOLoadingTime + ";" + TransporterName + ";" + QtyLoadedPCS + ";" + QtyLoadedCTN;

            //    fileDOShipment.WriteLine(RowData);
            //}





            ////File GR
            //String PathFileGRExample = Path.Combine(_systemConfig.PathFileGR, "GR" + DateTime.Now.ToString("dd_MMM_yyyy_HH_mm") + ".txt");
            //using StreamWriter fileGR = new(PathFileGRExample, append: true);

            //for (int i = 0; i < 5; i++)
            //{




            //    String DONumber = DepanDO + i.ToString().PadLeft(5, '0');
            //    String CompanyCode = "0101";
            //    String GRDate = DateTime.Now.AddDays(i + 6).ToString("yyyyMMdd");
            //    String StoreCode = "KS01";

            //    String RowData = CompanyCode.ToUpper() + ";" + StoreCode + ";" + DONumber + ";" + GRDate + ";" + StoreCode;

            //    fileGR.WriteLine(RowData);
            //}



            //for (int i = 0; i < 5; i++)
            //{
            //    String DONumber = DepanDO + i.ToString().PadLeft(5, '0');
            //    if (_context.Pod_Tbs.FirstOrDefault(x => x.Do_Vc == DONumber) == null)
            //    {
            //        _context.Pod_Tbs.Add(new Database.Models.Pod_Tb
            //        {

            //            Do_Vc = DONumber,
            //            DateTime_Dt = DateTime.Now.AddDays(30 + i),
            //            Store_Ch = "KS76",
            //            UserId_Ch = "System",
            //            Qty_NM = 3,
            //        });
            //    }

            //    _context.SaveChanges();

            //}




            //// File TRANSPORTER
            //String PathFileTransporter = Path.Combine(_systemConfig.PathFileTransporter, "DOTRANSPORTER" + DateTime.Now.ToString("dd_MMM_yyyy_HH_mm") + ".txt");
            //using StreamWriter fileVendor = new(PathFileTransporter, append: true);


            //for (int i = 0; i < 5; i++)
            //{


            //    String DONumber = DepanDO + i.ToString().PadLeft(5, '0');
            //    String ReceiverName = "ReceiverName";
            //    String DropDate = DateTime.Now.AddDays(i + 5).ToString("dd-MMM-yyyy HH:mm");
            //    string Remark = "xxxxxxxxxxxx";
            //    string ScanOfPOD = DepanDO + i.ToString().PadLeft(5, '0') + ".jpg";
            //    string ScanPODDate = DateTime.Now.AddDays(i + 6).ToString("dd-MMM-yyyy HH:mm");

            //    String RowData = DONumber.ToUpper() + ";" + DropDate + ";" + ReceiverName + ";" + Remark + ";" + ScanOfPOD + ";" + ScanPODDate;

            //    fileVendor.WriteLine(RowData);
            //    String PathOri = Path.Combine(_systemConfig.PathFileTransporterImage, "ORI.jpg");
            //    String Destination = Path.Combine(_systemConfig.PathFileTransporterImage, ScanOfPOD);
            //    if (!System.IO.File.Exists(Destination))
            //    {
            //        System.IO.File.Copy(PathOri, Destination);
            //    }


            //}




            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";

            return Redirect(baseUrl + "/hangfire");
        }
        public IActionResult ReloadJob()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in StorageConnectionExtensions.GetRecurringJobs(connection))
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }


            var rootFolder = Directory.GetCurrentDirectory();

            String Queue = "";



            Queue = "defaultqueue";

       
            return Redirect("/hangfire");
        }

        public async Task<IActionResult> AddUserToAdminRole()
        {
            await roleManager.CreateAsync(new IdentityRole("HangfireAdmin"));
            var user = await userManager.FindByNameAsync("new@gmail.com");

            await userManager.AddToRoleAsync(user, "HangfireAdmin");

            return Ok();
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
