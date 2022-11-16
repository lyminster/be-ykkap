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
    public class ProjectReferencesController : ControllerBase
    {
        ProjectReferencesDataAccessLayer _projectReferencesDataAccessLayer;
        SystemConfig _SystemConfig;
        BusinessModelContext _businessModelContext;
        public ProjectReferencesController(BusinessModelContext businessModelContext, IMapper mapper, IHostingEnvironment hostenv, IConfiguration configFile, SystemConfig systemConfig)
        {
            _SystemConfig = systemConfig;
            _businessModelContext = businessModelContext;
            _projectReferencesDataAccessLayer = new ProjectReferencesDataAccessLayer(businessModelContext, mapper, hostenv, configFile);
        }

        [HttpPost]
        [Route("post/GetListProject")]
        public async Task<IActionResult> GetListProjectAsync([FromBody] JsonProjectReferencesVM filter)
        {
            List<JsonProjectReferencesVM> listProject = new List<JsonProjectReferencesVM>();

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

                listProject = _projectReferencesDataAccessLayer.GetProjectReferencesAsync();
                ResultData.ObjectValue = listProject;
                return Ok(ResultData);
            }
            catch (Exception ex)
            {
                return BadRequest(GlobalHelpers.GetErrorMessage(ex));
            }
        }
    }
}
