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
using NPOI.SS.Formula.Functions;

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
                List < JsonCatalogDetailVM > asd = new List<JsonCatalogDetailVM> ();
                foreach (CatalogType datacatalogType2 in dataCatalogType)
                {
                    asd = _mapper.Map<List<CatalogDetail>, List<JsonCatalogDetailVM>>(RepoDetail.GetCatalogDetailByType(datacatalogType2.ID));
                    JsonCatalogVM dataJson = new JsonCatalogVM
                    {
                        name = datacatalogType2.name,
                        description = datacatalogType2.description,
                        imgUrl = datacatalogType2.imgUrl,
                        child = asd
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

        public List<JsonCatalogTypeVM> FindAsync(JsonCatalogTypeVM filter, ClaimsPrincipal claims)
        {
            List<JsonCatalogTypeVM> listCatalogType = new List<JsonCatalogTypeVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<CatalogType, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.name))
                {
                    filterExp = filterExp.And(x => x.name != null);
                    filterExp = filterExp.And(x => x.name.ToLower().Contains(filter.name.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.imgUrl))
                {
                    filterExp = filterExp.And(x => x.imgUrl != null);
                    filterExp = filterExp.And(x => x.imgUrl.ToLower().Contains(filter.imgUrl.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.description))
                {
                    filterExp = filterExp.And(x => x.description != null);
                    filterExp = filterExp.And(x => x.description.ToLower().Contains(filter.description.ToLower()));
                }


                listCatalogType = _mapper.Map<IEnumerable<CatalogType>, List<JsonCatalogTypeVM>>(Repo.QueryCatalogTypes(filterExp, filter.Take, filter.Skip));
                return listCatalogType;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonCatalogTypeVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.name,Save.description,Save.imgUrl,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Catalog Type ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    CatalogType NewData = new CatalogType();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.name = Save.name;
                        NewData.description = Save.description;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetCatalogTypeByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.name = Save.name;
                        NewData.description = Save.description;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetCatalogTypeByID(NewData.ID);

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

                CatalogType DelcatalogType = Repo.GetCatalogTypeByID(ID);
                DelcatalogType.ModelState = ObjectState.SoftDelete;
                DelcatalogType.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelcatalogType);
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

        public JsonCatalogTypeVM GetCatalogTypeBYIdAsync(String ID)
        {
            try
            {
                return _mapper.Map<CatalogType, JsonCatalogTypeVM>(Repo.GetCatalogTypeByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
