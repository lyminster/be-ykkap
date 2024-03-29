﻿using AutoMapper;
using DAL.DataAccessLayer.Master;
using DAL.Helper;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using ViewModel.ViewModels;
using ViewModel.ViewModels.Master;

namespace TMS.Areas.Master.Controllers
{
    [Area("Master")]
    public class ProjectController : Controller
    {
        private readonly BusinessModelContext _context;
        private readonly IHostingEnvironment _hostenv;
        private IConfiguration _config;
        private readonly IMapper _mapper;
        HelperTableDataAccessLayer DALHelper;
        ProjectReferencesDataAccessLayer DALProject;
        FileLogDataAccessLayer DALFileLog;
        ProjectTypeDataAccessLayer DALProjectType;


        public ProjectController(BusinessModelContext context, IHostingEnvironment hostenv, IMapper mapper, IConfiguration configfile)
        {
            _context = context;
            _hostenv = hostenv;
            _mapper = mapper;
            _config = configfile;
            DALHelper = new HelperTableDataAccessLayer(_context, _mapper, _hostenv);
            DALProject = new ProjectReferencesDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALProjectType = new ProjectTypeDataAccessLayer(_context, _mapper, _hostenv, configfile);
            DALFileLog = new FileLogDataAccessLayer(_context, _mapper, _hostenv, configfile);
        }

        public IActionResult Index()
        {
            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            List<JsonProjectReferencesVM> page1 = DALProject.FindAsync(new JsonProjectReferencesVM { }, User);
            IndexProjectReferencesVM data = new IndexProjectReferencesVM();
            data.listIndex = page1;

            return View(data);
        }

        public ActionResult LoadData(FilterTgl filters)
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

                // Skip number of Rows count  
                var start = Request.Form["start"].FirstOrDefault();

                // Paging Length 10,20  
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

                // Sort Column Direction (asc, desc)  
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

                // Search Value from (Search box)  
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10, 20, 50,100)  
                int pageSize = length != null ? Convert.ToInt32(length) : 0;

                int skip = start != null ? Convert.ToInt32(start) : 0;

                int recordsTotal = 0;

                /// INI KALAU DI PAKE AJA BUAT CONTOH TRANSAKSI LAEN 
                //DateTime? from = !String.IsNullOrEmpty(filters.tglDari) ? Convert.ToDateTime(filters.tglDari) : null;
                //string convertFrom = from != null ? from.Value.ToString("yyyy-MM-dd") : null;

                //DateTime? to = !String.IsNullOrEmpty(filters.tglSampai) ? Convert.ToDateTime(filters.tglSampai) : null;
                //string convertTo = to != null ? to.Value.ToString("yyyy-MM-dd") : null;


                var projectData = DALProject.FindAsync(new JsonProjectReferencesVM { }, User);



                //Sorting  
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumnDirection == "asc")
                    {
                        if (sortColumn == "Name")
                        {
                            projectData = projectData.OrderBy(x => x.name).ToList();
                        }
                        else if (sortColumn == "Detail")
                        {
                            projectData = projectData.OrderBy(x => x.detail).ToList();
                        }
                        else if (sortColumn == "Building")
                        {
                            projectData = projectData.OrderBy(x => x.building).ToList();
                        }
                        else if (sortColumn == "Project Type")
                        {
                            projectData = projectData.OrderBy(x => x.ProjectType).ToList();
                        }
                        else if (sortColumn == "Location")
                        {
                            projectData = projectData.OrderBy(x => x.location).ToList();
                        }
                        else if (sortColumn == "List Product Used")
                        {
                            projectData = projectData.OrderBy(x => x.listProductUsed).ToList();
                        }
                        else if (sortColumn == "Project Year")
                        {
                            projectData = projectData.OrderBy(x => x.projectYear).ToList();
                        }
                    }
                    else
                    {
                        if (sortColumn == "Name")
                        {
                            projectData = projectData.OrderByDescending(x => x.name).ToList();
                        }
                        else if (sortColumn == "Detail")
                        {
                            projectData = projectData.OrderByDescending(x => x.detail).ToList();
                        }
                        else if (sortColumn == "Building")
                        {
                            projectData = projectData.OrderByDescending(x => x.building).ToList();
                        }
                        else if (sortColumn == "Project Type")
                        {
                            projectData = projectData.OrderByDescending(x => x.ProjectType).ToList();
                        }
                        else if (sortColumn == "Location")
                        {
                            projectData = projectData.OrderByDescending(x => x.location).ToList();
                        }
                        else if (sortColumn == "List Product Used")
                        {
                            projectData = projectData.OrderByDescending(x => x.listProductUsed).ToList();
                        }
                        else if (sortColumn == "Project Year")
                        {
                            projectData = projectData.OrderByDescending(x => x.projectYear).ToList();
                        }
                    }
                }
                //Search  
                if (!string.IsNullOrEmpty(searchValue))
                {
                    projectData = projectData.Where(m =>
                        m.CreatedBy.ToLower().Contains(searchValue.ToLower())
                        || (m.listProductUsed != null && m.name.ToLower().Contains(searchValue.ToLower()))
                        || (m.listProductUsed != null && m.detail.ToLower().Contains(searchValue.ToLower()))
                        || (m.listProductUsed != null && m.building.ToLower().Contains(searchValue.ToLower()))
                      
                        || (m.listProductUsed != null && m.location.ToLower().Contains(searchValue.ToLower()))
                        || (m.listProductUsed != null && m.listProductUsed.ToLower().Contains(searchValue.ToLower()))
                        || (m.projectYear != null && m.projectYear.ToLower().Contains(searchValue.ToLower()))
                        || (m.type != null && m.type.ToLower().Contains(searchValue.ToLower()))
                        ).ToList();
                }

                //total number of rows counts   
                recordsTotal = projectData.Count();
                //Paging   
                var data = projectData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult Create()
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            ProjectReferencesVM data = new ProjectReferencesVM();
            data.ListProjectType = DALProjectType.GetListProjectTypeAsync();
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProjectReferencesVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                var filename = "";

                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlProjectImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.urlImage = filename;
                }
             
                data.CreatedBy = GlobalHelpers.GetEmailFromIdentity(User);
                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var SaveData = _mapper.Map<ProjectReferencesVM, JsonProjectReferencesVM>(data);
                var retrunSave = DALProject.SaveAsync(SaveData, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Create Project References", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                data.ListProjectType = DALProjectType.GetListProjectTypeAsync();
                Alert(errMsg, NotificationType.error);
                return View(data);
            }
            return View(data);
        }

        public IActionResult Edit(String ID)
        {

            var cookiesEmail = GlobalHelpers.GetEmailFromIdentity(User);
            if (cookiesEmail == null)
            {
                return RedirectToAction("Home", "Login", new { area = "" });
            }
            var data = DALProject.GetProjectEditByIdAsync(ID);
            data.ListProjectType = DALProjectType.GetListProjectTypeAsync();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProjectReferencesVM data)
        {
            if (ModelState.IsValid)
            {
                string errMsg = "";
                var filename = "";

                if (data.Upload != null && data.Upload.FileName != null)
                {
                    String folder = _config.GetConnectionString("UrlProjectImage");
                    filename = GlobalHelpers.CopyFile(data.Upload, _hostenv, folder, this.Request);
                    data.urlImage = filename;
                }


                data.LastModifiedBy = GlobalHelpers.GetEmailFromIdentity(User);
                var upddata = _mapper.Map<ProjectReferencesVM, JsonProjectReferencesVM>(data);
                var retrunSave = DALProject.SaveAsync(upddata, User);
                if (retrunSave.result == true)
                {
                    Alert("Success Update Project References", NotificationType.success);
                    return RedirectToAction(nameof(Index));
                }
                upddata.ListProjectType = DALProjectType.GetListProjectTypeAsync();
                Alert(errMsg, NotificationType.error);
                return View(upddata);
            }
            var upddata2 = _mapper.Map<ProjectReferencesVM, JsonProjectReferencesVM>(data);
            upddata2.ListProjectType = DALProjectType.GetListProjectTypeAsync();
            return View(upddata2);
        }

        [HttpPost]
        public IActionResult DeleteData(string id)
        {
            try
            {
                var returns = DALProject.DeleteAsync(id, User);
                if (returns.result == true)
                {
                    return Json("Success");
                }
                else
                {
                    return Json(returns.message);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
