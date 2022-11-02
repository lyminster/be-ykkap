using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.DataAccessLayer;
using DAL.DataAccessLayer.Master;
using Database.Models;
using Database.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MainProject.Models;

namespace MainProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        private readonly BusinessModelContext _context;

        public HomeController(ILogger<HomeController> logger, BusinessModelContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _logger = logger;
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            
        }
         
        public IActionResult UrlActionDocument(string idwf, string username, string status) //page1
        {


            UrlViewModel page1 = new UrlViewModel();
            if (status == "1")
            {
                page1.statuscode = "Approve";
            }
            if (status == "2")
            {
                page1.statuscode = "Reject";
            }
            if (status == "3")
            {
                page1.statuscode = "Revise";
            }

            UpdateWorkflow reqData = new UpdateWorkflow();
            reqData.ID = idwf;
            reqData.IDClient = "VEN";
            reqData.Username = username;
            reqData.Action = Convert.ToInt64(status);
            reqData.IDDocumentWorkflow = idwf;
            reqData.Notes = page1.statuscode;
   
            return View(page1);
        }
        public IActionResult Index()
        {
            return View();
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
