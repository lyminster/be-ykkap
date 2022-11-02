using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IISHOSTV1.Controllers
{
    public class RegisterController : Controller
    {

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(
         CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("LoginForm", "Login");
        }

    }
}
