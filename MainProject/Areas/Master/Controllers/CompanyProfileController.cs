using AutoMapper;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using ViewModel.ViewModels;

namespace TMS.Areas.Master.Controllers
{
    [Area("Master")]
    public class CompanyProfileController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        CompanyProfileDataAccessLayer DALCompanyProfile;
        FileLogDataAccessLayer DALFileLog;


        public CompanyProfileController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALCompanyProfile = new CompanyProfileDataAccessLayer(_context, _mapper, _hostenv, configfile);
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
