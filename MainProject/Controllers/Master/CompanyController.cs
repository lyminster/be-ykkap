using AutoMapper;

using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.ConfigClass;
using Database.Models;
using Database.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Helper;
using System.Threading.Tasks;
using MainProject.Models;
using ViewModel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using TMS.Services;

namespace TMS.Controllers.Master
{

    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        CompanyDataAccessLayer _companyDataAccessLayer;
        SystemConfig _SystemConfig;
        BusinessModelContext _businessModelContext;
        public CompanyController(BusinessModelContext businessModelContext, IMapper mapper, IHostingEnvironment hostenv, IConfiguration configFile, SystemConfig systemConfig)
        {
            _SystemConfig = systemConfig;
            _businessModelContext = businessModelContext;
            _companyDataAccessLayer = new CompanyDataAccessLayer(businessModelContext, mapper, hostenv, configFile);
        }

        [HttpPost]
        [Route("post/FindCompany")]
        public async Task<IActionResult> FindCompanyAsync([FromBody] JsonCompanyVM filter)
        {
            List<JsonCompanyVM> ListCompany = new List<JsonCompanyVM>();

            try
            {
                JsonReturn ResultData = new JsonReturn(true);

                if (_SystemConfig.StaticKey == "true")
                {
                    var ValidStaticKey = await GlobalHelpers.GetAPIKeyValidationAndGenerateCookiesAsync(filter.ApiKey, _businessModelContext, this.HttpContext).ConfigureAwait(false);
                    if (!ValidStaticKey)
                    {
                        return BadRequest("invalid api key");
                    }
                }

                ListCompany = _companyDataAccessLayer.FindAsync(filter, User);
                ListCompany = ListCompany.OrderBy(x => x.Name).ThenBy(x => x.Name).ToList();
                ResultData.ObjectValue = ListCompany;
                return Ok(ResultData);
            }
            catch (Exception ex)
            {
                return BadRequest(GlobalHelpers.GetErrorMessage(ex));
            }
        }
    }
}
