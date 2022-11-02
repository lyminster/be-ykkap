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
                        finishTimestamp = data.finishTimestamp,
                        startTimeStamp = data.startTimeStamp ,
                        location = data.location ,
                        name = data.name ,
                        type = data.type ,
                        urlYoutube = data.urlYoutube ,
                        listProductUsed = data.listProductUsed.Split(',').ToList()
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
    }
}
