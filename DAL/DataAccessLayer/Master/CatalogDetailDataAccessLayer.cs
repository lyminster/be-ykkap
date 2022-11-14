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
    public class CatalogDetailDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public CatalogDetailDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public CatalogDetailRepository _Repo;
        public CatalogDetailRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new CatalogDetailRepository(MasterEntities);
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


        public List<CatalogDetail> GetListCatalogDetailAsync()
        {
            try
            {
                return Repo.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonCatalogDetailVM> FindAsync(JsonCatalogDetailVM filter, ClaimsPrincipal claims)
        {
            List<JsonCatalogDetailVM> listCatalogDetail = new List<JsonCatalogDetailVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<CatalogDetail, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.name))
                {
                    filterExp = filterExp.And(x => x.name != null);
                    filterExp = filterExp.And(x => x.name.ToLower().Contains(filter.name.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.CatalogType))
                {
                    filterExp = filterExp.And(x => x.CatalogType != null);
                    filterExp = filterExp.And(x => x.CatalogType.ToLower().Contains(filter.CatalogType.ToLower()));
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
                if (!String.IsNullOrEmpty(filter.enPdfUrl))
                {
                    filterExp = filterExp.And(x => x.enPdfUrl != null);
                    filterExp = filterExp.And(x => x.enPdfUrl.ToLower().Contains(filter.enPdfUrl.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.idPdfUrl))
                {
                    filterExp = filterExp.And(x => x.idPdfUrl != null);
                    filterExp = filterExp.And(x => x.idPdfUrl.ToLower().Contains(filter.idPdfUrl.ToLower()));
                }


                listCatalogDetail = _mapper.Map<IEnumerable<CatalogDetail>, List<JsonCatalogDetailVM>>(Repo.QueryCatalogDetails(filterExp, filter.Take, filter.Skip));
                return listCatalogDetail;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonCatalogDetailVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.name,
                        Save.CatalogType,
                        Save.description,
                        Save.imgUrl,
                        Save.enPdfUrl,
                        Save.idPdfUrl,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Catalog Detail ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    CatalogDetail NewData = new CatalogDetail();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.name = Save.name;
                        NewData.CatalogType = Save.CatalogType;
                        NewData.description = Save.description;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.enPdfUrl = Save.enPdfUrl;
                        NewData.idPdfUrl = Save.idPdfUrl;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetCatalogDetailByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.name = Save.name;
                        NewData.name = Save.name;
                        NewData.CatalogType = Save.CatalogType;
                        NewData.description = Save.description;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.enPdfUrl = Save.enPdfUrl;
                        NewData.idPdfUrl = Save.idPdfUrl;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetCatalogDetailByID(NewData.ID);

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

                CatalogDetail DelcatalogDetail = Repo.GetCatalogDetailByID(ID);
                DelcatalogDetail.ModelState = ObjectState.SoftDelete;
                DelcatalogDetail.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelcatalogDetail);
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

        public JsonCatalogDetailVM GetCatalogDetailbyId(String ID)
        {
            try
            {
                return _mapper.Map<CatalogDetail, JsonCatalogDetailVM>(Repo.GetCatalogDetailByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
