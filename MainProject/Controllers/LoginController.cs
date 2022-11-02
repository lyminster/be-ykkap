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
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using static DAL.Helper.GlobalHelpers;
using DAL.Helper;

namespace MainProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostenv;
        private IConfiguration _config;
        UserDataAccessLayer DALUser;
        //Page1DataAccessLayer Page1DAL;
        public LoginController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration config)
        {
            _context = context;
            _mapper = mapper;
            _hostenv = hostenv;
            _config = config;
            DALUser = new UserDataAccessLayer(_context, _mapper, _hostenv, _config);
            //Page1DAL = new Page1DataAccessLayer(_context, _mapper, _hostenv, _config);
        }



        // GET: Login
        public async Task<IActionResult> Index()
        {
            UserViewModel filterData = new UserViewModel();
            List<UserViewModel> dbUser = DALUser.FindHome(filterData).ToList();
            //List<UserViewModel> userrr = dbUser.Select(x => new UserViewModel
            //{
            //    Id = x.Id,
            //    Email = x.Email,
            //    Password = x.Password,
            //    isAD = Convert.ToBoolean(x.isAD),
            //    UserType = userTypeRepo.GetByCode(x.UserType).UserTypeName,

            //}).ToList();



            return View(dbUser);
        }


        // GET: LoginPage
        public async Task<IActionResult> LoginForm()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail != null)
            {
                return RedirectToAction("Home", "Login");
            }
            return View();
        }

        // GET: chagepass
        public async Task<IActionResult> ChangePass()
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
        public async Task<IActionResult> ChangePass(ChangePass change)
        {
            if (ModelState.IsValid)
            {
                if (DALUser.changePass(change, GlobalHelpers.GetEmailFromIdentity(User)))
                {
                    Alert("Success Change password!", NotificationType.success);
                    return RedirectToAction("Home", "Login");
                }
                Alert("Failed change password!", NotificationType.error);
                return View(change);
            }
            return View(change);
        }

        [HttpPost]
        public JsonResult ResetPass(string email)
        {
            JsonResponseAPI res = new JsonResponseAPI();
            res.errorMsg = "Error Koneksi";
            res.errorCode = "01";

            //cek email exist gak
            var user = DALUser.findByEmail(email);
            if (user == null)
            {
                res.errorMsg = "Error No User Found";
                res.errorCode = "01";
                return Json(res);
            }
            else if (Convert.ToBoolean(user.IsAD))
            {
                res.errorMsg = "User AD cannot Reset Password";
                res.errorCode = "01";
                return Json(res);
            }
            else
            {
                var respo = DALUser.resetPass(email, user.Email);
                if (respo)
                {
                    res.errorMsg = "Success Reset Password";
                    res.errorCode = "00";
                    return Json(res);
                }
                res.errorMsg = "Error Reset Password";
                res.errorCode = "01";
                return Json(res);
            }

            return Json(res);
        }

        public async Task<IActionResult> Home()
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
        public async Task<IActionResult> LoginForm(UserViewModel loginmodel)
        {

            User result = DALUser.Find(loginmodel, null, null);

            if (result != null)
            {


                String IDVendor = "";

                if (!String.IsNullOrEmpty(result.IDVendor))
                {
                    IDVendor = result.IDVendor;
                }

                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, result.Email),
            new Claim(EnumClaims.Username.ToString(), result.Email),
            new Claim(EnumClaims.IsAD.ToString(), result.IsAD),
            new Claim(EnumClaims.UserType.ToString(), result.UserType),
            new Claim(EnumClaims.Email.ToString(), result.Email),
            new Claim(EnumClaims.IDClient.ToString(), result.Idclient),
            new Claim(EnumClaims.IDVendor.ToString(), IDVendor),
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    IssuedUtc = DateTime.Now,
                    // The time at which the authentication ticket was issued.

                    RedirectUri = "/Home/"
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };


                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);


                //Response.Cookies.Append("Email", result.Email.ToString());
                //Response.Cookies.Append("UserType", result.UserType.ToString());
                //Response.Cookies.Append("", result.IsAD.ToString());
                //if (result.IDVendor != null)
                //{
                //    Response.Cookies.Append("IDVendor", result.IDVendor.ToString());
                //}

                return RedirectToAction("Home", "Login");
            }
            ViewBag.ErrorResult = "No User Found !";
            return View();
        }

        // GET: Login/AddOrEdit
        public IActionResult AddOrEdit(int Id = 0)
        {
            return View();
        }
        // GET: Login/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loginModel = DALUser.GetUser(id); ;

            if (loginModel == null)
            {
                return NotFound();
            }

            var userrr = _mapper.Map<UserViewModel>(loginModel);
            return View(userrr);
        }

        // GET: Login/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("Id,Email,EmailApproval,Password,UserType,isAD, companyList")] UserViewModel loginModel)
        {

            if (ModelState.IsValid)
            {
                string eMail = GlobalHelpers.GetEmailFromIdentity(User);
                String ClientID = "VEN";
                string errMsg = "";
                var retrunSave = DALUser.Save(loginModel, eMail, ClientID, out errMsg);
                if (retrunSave)
                {
                    Alert("Success Add/Edit User", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                Alert(errMsg, NotificationType.error);
                return View(loginModel);
            }
            return View(loginModel);
        }



        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(String id)
        {

            DALUser.Delete(id);
            Alert("Success Delete", NotificationType.success);
            return RedirectToAction(nameof(Index));
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
