using AutoMapper;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.ConfigClass;
using Database.InfrastructurClass;
using Database.Models;
using Database.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using MainProject.Models;
using static DAL.Helper.GlobalHelpers;

namespace TMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultAPIController : ControllerBase
    {

        private readonly IJwtAuthManager _jwtAuthManager;
        UserDataAccessLayer DALUser;
        IMapper _mapper;
        SystemConfig _SystemConfig;
        public DefaultAPIController(IJwtAuthManager jwtAuthManager, BusinessModelContext businessModelContext, IMapper mapper, IHostingEnvironment hostenv, IConfiguration configFile, SystemConfig systemConfig)
        {
            DALUser = new UserDataAccessLayer(businessModelContext, mapper, hostenv, configFile);
            _SystemConfig = systemConfig;
            _jwtAuthManager = jwtAuthManager;
            _mapper = mapper;

        }



        [HttpPost]
        [Route("GenerateAPIKey")]
        public IActionResult GenerateAPIKey(string apiKey)
        {
            try
            {
                string encryptionKey = "EDS";
                string dStr = Encryption.passwordEncrypt(apiKey, encryptionKey);

                return Ok(dStr);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] JsonAD request)
        {
            try
            {
                JsonReturn ResultData = new JsonReturn(true);

                if (!ModelState.IsValid)
                {
                    return BadRequest("please fill username and password");

                }

                if (_SystemConfig.IsTesting == "true")
                {
                    request.Email = "map2@map.com";
                    request.Password = "123";
                }


                //hit AD
                JsonAD jsonAD = new JsonAD();
                jsonAD.Email = request.Email;
                jsonAD.Password = request.Password;
                String flagAD = "";
                HttpStatusCode ADresult = HttpStatusCode.BadRequest;
                if (_SystemConfig.IsUsingAD == "true")
                {
                    try
                    {
                        ADresult = GlobalHelpers.SentRequest(JsonConvert.SerializeObject(jsonAD), _SystemConfig.URLAD, GlobalHelpers.MethodEnum.POST.ToString(), false, null, out flagAD);
                        if (ADresult != System.Net.HttpStatusCode.OK && ADresult != System.Net.HttpStatusCode.Accepted)
                        {
                            ResultData = new JsonReturn(false);
                            ResultData.message = "username password wrong";
                            return Ok(ResultData);
                        }

                    }
                    catch (Exception)
                    {
                        ADresult = HttpStatusCode.BadRequest;
                    }

                }
                User user = new User();

                if (ADresult == System.Net.HttpStatusCode.OK)
                {
                    // ini user biasa /regular

                    user.UserType = DAL.Helper.ConstantVariable.EnumUserType.USER.ToString();
                    user.ID = request.Email;
                    user.Email = request.Email;
                    user.Email = HttpUtility.HtmlDecode(flagAD);
                    user.Idclient = _SystemConfig.IDClient;
                    var cekuser = DALUser.GetUser(user.Email);
                    if (cekuser != null)
                    {

                        user = cekuser;
                    }

                }
                else
                {
                    var cekuser = DALUser.Find(new UserViewModel { Password = request.Password, Email = request.Email }, null, null);
                    if (cekuser == null)
                    {
                        ResultData = new JsonReturn(false);
                        ResultData.message = "username password wrong";
                        return Ok(ResultData);

                    }
                    user = cekuser;
                }









                String IDVendor = "";
                if (!String.IsNullOrEmpty(user.IDVendor))
                {
                    IDVendor = user.IDVendor;
                }



                //hardcode semua jadi email
                user.Email = HttpUtility.HtmlDecode(user.Email);
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(EnumClaims.Username.ToString(), user.Email),
            new Claim(EnumClaims.IsAD.ToString(), user.IsAD),
            new Claim(EnumClaims.UserType.ToString(), user.UserType),
            new Claim(EnumClaims.Email.ToString(), user.Email),
            new Claim(EnumClaims.IDClient.ToString(), user.Idclient),
            new Claim(EnumClaims.IDVendor.ToString(), IDVendor),
        };

                var jwtResult = _jwtAuthManager.GenerateTokens(request.Email, claims.ToArray(), DateTime.Now);

                ResultData = new JsonReturn(true);
                ResultData.message = "Succes Login";
                ResultData.ObjectValue = new MainProject.Models.JsonToken
                {
                    Token = jwtResult.AccessToken,
                    User = user,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                };
                return Ok(ResultData);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
