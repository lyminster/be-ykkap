using AutoMapper;
using DAL.Helper;
using Database.Models;
using Database.Repositories;
using Database.ViewModels;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using MainProject.Models;
using ViewModel.ViewModels;
using static DAL.Helper.GlobalHelpers;
using static Database.Models.HelperFunction;
using ViewModel.ViewModels.Master;

namespace DAL.DataAccessLayer.Master
{
    public class ProjectReferencesDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public ProjectReferencesDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public ProjectReferencesRepository _Repo;
        public ProjectReferencesRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new ProjectReferencesRepository(MasterEntities);
                }
                return _Repo;
            }
        }
        public FileLogRepository _FileLogRepo;
        public FileLogRepository FileLogRepo
        {
            get
            {
                if (_FileLogRepo == null)
                {
                    _FileLogRepo = new FileLogRepository(MasterEntities, configFile);
                }
                return _FileLogRepo;
            }
        }


        public List<JsonProjectReferencesVM> GetProjectReferencesAsync()
        {
            try
            {
                List<ProjectReferences> listProject = Repo.GetListProjectReferences();
                List<JsonProjectReferencesVM> dataJson = new List<JsonProjectReferencesVM>();
                foreach (ProjectReferences data in listProject)
                {
                    JsonProjectReferencesVM jsonbuild = new JsonProjectReferencesVM
                    {
                        building = data.building,
                        detail = data.detail,
                        urlImage = data.urlImage,
                        projectYear = data.projectYear,
                        location = data.location ,
                        name = data.name ,
                        type = data.type ,
                        urlYoutube = data.urlYoutube ,
                        listProductUsed = data.listProductUsed
                    };
                    dataJson.Add(jsonbuild);
                }
                return dataJson;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonProjectReferencesVM> FindAsync(JsonProjectReferencesVM filter, ClaimsPrincipal claims)
        {
            List<JsonProjectReferencesVM> listProjectReferences = new List<JsonProjectReferencesVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<ProjectReferences, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.name))
                {
                    filterExp = filterExp.And(x => x.name != null);
                    filterExp = filterExp.And(x => x.name.ToLower().Contains(filter.name.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.detail))
                {
                    filterExp = filterExp.And(x => x.detail != null);
                    filterExp = filterExp.And(x => x.detail.ToLower().Contains(filter.detail.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlImage))
                {
                    filterExp = filterExp.And(x => x.urlImage != null);
                    filterExp = filterExp.And(x => x.urlImage.ToLower().Contains(filter.urlImage.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.building))
                {
                    filterExp = filterExp.And(x => x.building != null);
                    filterExp = filterExp.And(x => x.building.ToLower().Contains(filter.building.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.type))
                {
                    filterExp = filterExp.And(x => x.type != null);
                    filterExp = filterExp.And(x => x.type.ToLower().Contains(filter.type.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.location))
                {
                    filterExp = filterExp.And(x => x.location != null);
                    filterExp = filterExp.And(x => x.location.ToLower().Contains(filter.location.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlYoutube))
                {
                    filterExp = filterExp.And(x => x.urlYoutube != null);
                    filterExp = filterExp.And(x => x.urlYoutube.ToLower().Contains(filter.urlYoutube.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.listProductUsed))
                {
                    filterExp = filterExp.And(x => x.listProductUsed != null);
                    filterExp = filterExp.And(x => x.listProductUsed.ToLower().Contains(filter.listProductUsed.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.projectYear))
                {
                    filterExp = filterExp.And(x => x.projectYear != null);
                    filterExp = filterExp.And(x => x.projectYear.ToLower().Contains(filter.projectYear.ToLower()));
                }


                listProjectReferences = _mapper.Map<IEnumerable<ProjectReferences>, List<JsonProjectReferencesVM>>(Repo.QueryProjectReferencess(filterExp, filter.Take, filter.Skip));
                return listProjectReferences;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonProjectReferencesVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.name,
                        Save.detail,
                        Save.building,
                        Save.urlImage,
                        Save.type,
                        Save.location,
                        Save.listProductUsed,
                        Save.projectYear,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Project Reference ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    ProjectReferences NewData = new ProjectReferences();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.name = Save.name;
                        NewData.detail = Save.detail;
                        NewData.building = Save.building;
                        NewData.urlImage = Save.urlImage;
                        NewData.type = Save.type;
                        NewData.location = Save.location;
                        NewData.urlYoutube = Save.urlYoutube;
                        NewData.listProductUsed = Save.listProductUsed;
                        NewData.projectYear = Save.projectYear;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetProjectByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.name = Save.name;
                        NewData.detail = Save.detail;
                        NewData.building = Save.building;
                        NewData.urlImage = Save.urlImage;
                        NewData.type = Save.type;
                        NewData.location = Save.location;
                        NewData.urlYoutube = Save.urlYoutube;
                        NewData.listProductUsed = Save.listProductUsed;
                        NewData.projectYear = Save.projectYear;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetProjectByID(NewData.ID);

                    jsonReturn = new JsonReturn(true);
                    jsonReturn.message = "Data Sukses Di Simpan";
                    jsonReturn.ObjectValue = NewData;

                }

                return jsonReturn;

            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        public JsonReturn DeleteAsync(String ID, ClaimsPrincipal User)
        {
            try
            {
                JsonReturn ReturnJson = new JsonReturn(false);
                GlobalHelpers.ReGenerateThreadClaim(User);

                ProjectReferences delProject = Repo.GetProjectByID(ID);
                delProject.ModelState = ObjectState.SoftDelete;
                delProject.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(delProject);
                UnitOfWork.Commit();
                ReturnJson = new JsonReturn(true);
                ReturnJson.message = "Sukses Delete Data";
                return ReturnJson;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public JsonProjectReferencesVM GetProjectByIdAsync(String ID)
        {
            try
            {
                return _mapper.Map<ProjectReferences, JsonProjectReferencesVM>(Repo.GetProjectByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonProjectReferencesVM GetProjectEditByIdAsync(String ID)
        {
            try
            {
                return _mapper.Map<ProjectReferences, JsonProjectReferencesVM>(Repo.GetProjectByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
