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
    public class CompanyProfileDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public CompanyProfileDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public CompanyProfileRepository _Repo;
        public CompanyProfileRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new CompanyProfileRepository(MasterEntities);
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


        public JsonCompanyProfileVM GetCompanyProfileAsync()
        {
            try
            {
                return _mapper.Map<CompanyProfile, JsonCompanyProfileVM>(Repo.GetCompanyProfile());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonCompanyProfileVM> FindAsync(JsonCompanyProfileVM filter, ClaimsPrincipal claims)
        {
            List<JsonCompanyProfileVM> listCompanyProfile = new List<JsonCompanyProfileVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<CompanyProfile, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.about))
                {
                    filterExp = filterExp.And(x => x.about != null);
                    filterExp = filterExp.And(x => x.about.ToLower().Contains(filter.about.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.visionMission))
                {
                    filterExp = filterExp.And(x => x.visionMission != null);
                    filterExp = filterExp.And(x => x.visionMission.ToLower().Contains(filter.visionMission.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.imgUrl))
                {
                    filterExp = filterExp.And(x => x.imgUrl != null);
                    filterExp = filterExp.And(x => x.imgUrl.ToLower().Contains(filter.imgUrl.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.youtubeId))
                {
                    filterExp = filterExp.And(x => x.youtubeId != null);
                    filterExp = filterExp.And(x => x.youtubeId.ToLower().Contains(filter.youtubeId.ToLower()));
                }


                listCompanyProfile = _mapper.Map<IEnumerable<CompanyProfile>, List<JsonCompanyProfileVM>>(Repo.QueryCompanyProfiles(filterExp, filter.Take, filter.Skip));
                return listCompanyProfile;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonCompanyProfileVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(
                        Save.about,
                        Save.visionMission,
                        Save.imgUrl,
                        Save.youtubeId,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Company Profile ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    CompanyProfile NewData = new CompanyProfile();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.about = Save.about;
                        NewData.visionMission = Save.visionMission;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.youtubeId = Save.youtubeId;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetCompanyProfileByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.about = Save.about;
                        NewData.visionMission = Save.visionMission;
                        NewData.imgUrl = Save.imgUrl;
                        NewData.youtubeId = Save.youtubeId;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetCompanyProfileByID(NewData.ID);

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

                CompanyProfile DelCompanyProfile = Repo.GetCompanyProfileByID(ID);
                DelCompanyProfile.ModelState = ObjectState.SoftDelete;
                DelCompanyProfile.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelCompanyProfile);
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

        public JsonCompanyProfileVM GetCompanyProfileAsync(String ID)
        {
            try
            {
                return _mapper.Map<CompanyProfile, JsonCompanyProfileVM>(Repo.GetCompanyProfileByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
