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

namespace DAL.DataAccessLayer.Master
{
    public class SocialMediaDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public SocialMediaDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public SocialMediaRepository _Repo;
        public SocialMediaRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new SocialMediaRepository(MasterEntities);
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

        public JsonSocialMediaVM GetSocialMediaAsync()
        {
            try
            {
                return _mapper.Map<SocialMedia, JsonSocialMediaVM>(Repo.GetSocialMedia());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonSocialMediaVM> FindAsync(JsonSocialMediaVM filter, ClaimsPrincipal claims)
        {
            List<JsonSocialMediaVM> listSocialMedia = new List<JsonSocialMediaVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<SocialMedia, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.urlFb))
                {
                    filterExp = filterExp.And(x => x.urlFb != null);
                    filterExp = filterExp.And(x => x.urlFb.ToLower().Contains(filter.urlFb.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlIg))
                {
                    filterExp = filterExp.And(x => x.urlIg != null);
                    filterExp = filterExp.And(x => x.urlIg.ToLower().Contains(filter.urlIg.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlYt))
                {
                    filterExp = filterExp.And(x => x.urlYt != null);
                    filterExp = filterExp.And(x => x.urlYt.ToLower().Contains(filter.urlYt.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlWeb))
                {
                    filterExp = filterExp.And(x => x.urlWeb != null);
                    filterExp = filterExp.And(x => x.urlWeb.ToLower().Contains(filter.urlWeb.ToLower()));
                }


                listSocialMedia = _mapper.Map<IEnumerable<SocialMedia>, List<JsonSocialMediaVM>>(Repo.QuerySocialmedias(filterExp, filter.Take, filter.Skip));
                return listSocialMedia;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonSocialMediaVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(
                        Save.urlFb,
                        Save.urlIg,
                        Save.urlYt,
                        Save.urlWeb,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Social Media ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    SocialMedia NewData = new SocialMedia();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.urlFb = Save.urlFb;
                        NewData.urlIg = Save.urlIg;
                        NewData.urlYt = Save.urlYt;
                        NewData.urlWeb = Save.urlWeb;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetSocialmediaByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.urlFb = Save.urlFb;
                        NewData.urlIg = Save.urlIg;
                        NewData.urlYt = Save.urlYt;
                        NewData.urlWeb = Save.urlWeb;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetSocialmediaByID(NewData.ID);

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

                SocialMedia DelSocialMedia = Repo.GetSocialmediaByID(ID);
                DelSocialMedia.ModelState = ObjectState.SoftDelete;
                DelSocialMedia.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelSocialMedia);
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

        public JsonSocialMediaVM GetSocialMediaAsync(String ID)
        {
            try
            {
                return _mapper.Map<SocialMedia, JsonSocialMediaVM>(Repo.GetSocialmediaByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
