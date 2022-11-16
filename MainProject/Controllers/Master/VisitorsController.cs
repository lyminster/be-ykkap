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
using ViewModel.ViewModels.Master;

namespace TMS.Controllers.Master
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorsController : ControllerBase
    {
        SystemConfig _SystemConfig;
        BusinessModelContext _businessModelContext;
        VisitorDataAccessLayer _VisitorDataAccessLayer;
        public VisitorsController(BusinessModelContext businessModelContext, IMapper mapper, IHostingEnvironment hostenv, IConfiguration configFile, SystemConfig systemConfig)
        {
            _SystemConfig = systemConfig;
            _businessModelContext = businessModelContext;
            _VisitorDataAccessLayer = new VisitorDataAccessLayer(businessModelContext, mapper, hostenv, configFile);
        }
        [HttpPost]
        [Route("post/AddVisitor")]
        public async Task<IActionResult> AddVisitorAsync([FromBody] JsonVisitorVM filter)
        {
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

                _VisitorDataAccessLayer.SaveAsync(filter, User);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(GlobalHelpers.GetErrorMessage(ex));
            }
        }
    }
}
