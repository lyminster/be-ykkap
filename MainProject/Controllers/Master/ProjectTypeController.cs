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
    public class ProjectTypeController : Controller
    {
        ProjectTypeDataAccessLayer _projectTypeDataAccessLayer;
        SystemConfig _SystemConfig;
        BusinessModelContext _businessModelContext;
        public ProjectTypeController(BusinessModelContext businessModelContext, IMapper mapper, IHostingEnvironment hostenv, IConfiguration configFile, SystemConfig systemConfig)
        {
            _SystemConfig = systemConfig;
            _businessModelContext = businessModelContext;
            _projectTypeDataAccessLayer = new ProjectTypeDataAccessLayer(businessModelContext, mapper, hostenv, configFile);
        }

        [HttpPost]
        [Route("post/GetListProjectType")]
        public async Task<IActionResult> GetListProjectTypeAsync([FromBody] JsonProjectTypeVM filter)
        {
            List<JsonProjectTypeVM> projectType = new List<JsonProjectTypeVM>();

            try
            {
                JsonReturn ResultData = new JsonReturn(true);

                if (_SystemConfig.StaticKey == "true")
                {
                    System.Security.Claims.ClaimsPrincipal users;
                    var ValidStaticKey = GlobalHelpers.GetApiKeyValidation(filter.ApiKey, _businessModelContext, this.HttpContext, out users);
                    System.Threading.Thread.CurrentPrincipal = new System.Security.Claims.ClaimsPrincipal(users);
                    if (!ValidStaticKey)
                    {
                        return BadRequest("invalid api key");
                    }
                }

                projectType = _projectTypeDataAccessLayer.GetListProjectTypeAsync();
                ResultData.ObjectValue = projectType;
                return Ok(ResultData);
            }
            catch (Exception ex)
            {
                return BadRequest(GlobalHelpers.GetErrorMessage(ex));
            }
        }
    }
}
