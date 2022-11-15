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
    public class VisitorsController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        CompanyDataAccessLayer DALCompany;
        FileLogDataAccessLayer DALFileLog;

        public VisitorsController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
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
    }
}
