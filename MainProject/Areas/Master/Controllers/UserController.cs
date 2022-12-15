using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAL.DataAccessLayer.Master;
using Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using MainProject.Models;
using Microsoft.Extensions.Configuration;
using Database.Repositories;
using Database.ViewModels;
using ViewModel.ViewModels;
using ViewModel.Constant;
using DAL.Helper;

namespace IISHOSTV1.Areas.Master.Controllers
{
    [Area("Master")]
    public class UserController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostenv;
        private IConfiguration _config;
        UserDataAccessLayer DALUser;

        public UserController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _hostenv = hostenv;
            _config = config;
            DALUser = new UserDataAccessLayer(_context, _mapper, _hostenv, _config);

        }

        public UserTypeRepository _userTypeRepo;
        public UserTypeRepository userTypeRepo
        {
            get
            {
                if (_userTypeRepo == null)
                {
                    _userTypeRepo = new UserTypeRepository(_context);
                }
                return _userTypeRepo;
            }
        }

        // GET: Login
        public async Task<IActionResult> Index()
        {
            UserViewModel filterData = new UserViewModel();
            List<UserViewModel> dbUser = DALUser.FindHome(new UserViewModel() { }).ToList();


            return View(dbUser);
        }
        public IActionResult AddOrEdit(string Id)
        {
            var hasil = DALUser.getUserType();




            if (string.IsNullOrEmpty(Id))
            {
                return View(new UserViewModel
                {

                    Idclient = null,
                    CreatedDate = DateTime.Today,
                    CreatedBy = null,
                    ModifiedBy = null,
                    ModifiedDate = DateTime.Today,
                    UserTypeDetails = hasil
                });
            }
            else
            {
                var dataUser = DALUser.GetUser(Id);


                UserViewModel userrr = new UserViewModel();
                userrr.isAD = Convert.ToBoolean(dataUser.IsAD);
                userrr.UserTypeDetails = hasil;
                userrr.Id = dataUser.ID;
                //userrr.Password = DALUser.DecryptString(_config.GetConnectionString("EncDecKey"), dataUser.Password);
                userrr.UserType = dataUser.UserType;


                userrr.IDVendor = dataUser.IDVendor;



                if (Convert.ToBoolean(dataUser.IsAD))
                {
                    userrr.EmailApproval = dataUser.Email;
                }
                else
                {
                    userrr.Email = dataUser.Email;
                }

                return View(userrr);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Email,EmailApproval,Password,UserType,isAD, companyList, Jabatan, storeUserList, IDVendor")] UserViewModel loginModel)
        {
            string errMsg = "";
            if (ModelState.IsValid)
            {
                string eMail = GlobalHelpers.GetEmailFromIdentity(User);
                String ClientID = ConstantHelper.IDCLIENT;

                var retrunSave = DALUser.Save(loginModel, eMail, ClientID, out errMsg);
                if (retrunSave)
                {
                    Alert("Success Add/Edit User", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
            }

            var hasil = DALUser.getUserType();

            return View(loginModel);
        }



        public async Task<JsonResult> GetApproval(string q)
        {
            var param = q;
            var hasil = await DALUser.getADData(q);
            List<returnAD> returnValue = hasil.Select(x => new returnAD
            {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name

            }).ToList();


            return Json(returnValue);
        }


        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(string id)
        {

            DALUser.Delete(id);
            Alert("Success Delete", NotificationType.success);
            return RedirectToAction(nameof(Index));
        }


        // GET: chagepass
        public async Task<IActionResult> ChangePass()
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
        public async Task<IActionResult> ChangePass(ChangePass change)
        {
            if (ModelState.IsValid)
            {
                if (DALUser.changePass(change, GlobalHelpers.GetEmailFromIdentity(User)))
                {
                    Alert("Success Change password!", NotificationType.success);
                    return View(change);
                }
                Alert("Failed change password!", NotificationType.error);
                return View(change);
            }
            return View(change);
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
