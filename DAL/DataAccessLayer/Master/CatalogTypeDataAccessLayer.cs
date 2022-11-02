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
    public class CatalogTypeDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public CatalogTypeDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public CatalogTypeRepository _Repo;
        public CatalogTypeRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new CatalogTypeRepository(MasterEntities);
                }
                return _Repo;
            }
        }
        public CatalogDetailRepository _RepoDetail;
        public CatalogDetailRepository RepoDetail
        {
            get
            {
                if (_RepoDetail == null)
                {
                    _RepoDetail = new CatalogDetailRepository(MasterEntities);
                }
                return _RepoDetail;
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


        public List<CatalogType> GetListCatalogTypeAsync()
        {
            try
            {
                return Repo.GetCatalogType();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonCatalogVM> GetAllCatalogAsync()
        {
            try
            {
                List<JsonCatalogVM> dataCatalog = new List<JsonCatalogVM>();
                List<CatalogType> dataCatalogType = Repo.GetCatalogType();
                foreach(CatalogType datacatalogType2 in dataCatalogType)
                {
                    
                    JsonCatalogVM dataJson = new JsonCatalogVM
                    {
                        name = datacatalogType2.name,
                        description = datacatalogType2.description,
                        imgUrl = datacatalogType2.imgUrl,
                        child = _mapper.Map<List<CatalogDetail>, List<JsonCatalogDetailVM>>(_RepoDetail.GetCatalogDetailByType(datacatalogType2.ID))
                };
                    dataCatalog.Add(dataJson);
                }
                return dataCatalog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
