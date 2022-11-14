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
    public class ShowroomDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public ShowroomDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public ShowroomRepository _Repo;
        public ShowroomRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new ShowroomRepository(MasterEntities);
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


        public List<JsonShowroomVM> GetListShowroomAsync()
        {
            try
            {
                return _mapper.Map< IEnumerable<Showroom>, List<JsonShowroomVM>>(Repo.GetListShowroom());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonShowroomVM> FindAsync(JsonShowroomVM filter, ClaimsPrincipal claims)
        {
            List<JsonShowroomVM> listShowroom = new List<JsonShowroomVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<Showroom, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.name))
                {
                    filterExp = filterExp.And(x => x.name != null);
                    filterExp = filterExp.And(x => x.name.ToLower().Contains(filter.name.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.address))
                {
                    filterExp = filterExp.And(x => x.address != null);
                    filterExp = filterExp.And(x => x.address.ToLower().Contains(filter.address.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.urlImage))
                {
                    filterExp = filterExp.And(x => x.urlImage != null);
                    filterExp = filterExp.And(x => x.urlImage.ToLower().Contains(filter.urlImage.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.workingHour))
                {
                    filterExp = filterExp.And(x => x.workingHour != null);
                    filterExp = filterExp.And(x => x.workingHour.ToLower().Contains(filter.workingHour.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.telephone))
                {
                    filterExp = filterExp.And(x => x.telephone != null);
                    filterExp = filterExp.And(x => x.telephone.ToLower().Contains(filter.telephone.ToLower()));
                }


                listShowroom = _mapper.Map<IEnumerable<Showroom>, List<JsonShowroomVM>>(Repo.QueryShowrooms(filterExp, filter.Take, filter.Skip));
                return listShowroom;

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public JsonReturn SaveAsync(JsonShowroomVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.name, 
                        Save.urlImage,
                        Save.workingHour,
                        Save.address,
                        Save.telephone,
                        GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Showroom ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    Showroom NewData = new Showroom();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.name = Save.name;
                        NewData.urlImage = Save.urlImage;
                        NewData.workingHour = Save.workingHour;
                        NewData.address = Save.address;
                        NewData.telephone = Save.telephone;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetShowroomByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.name = Save.name;
                        NewData.urlImage = Save.urlImage;
                        NewData.workingHour = Save.workingHour;
                        NewData.address = Save.address;
                        NewData.telephone = Save.telephone;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetShowroomByID(NewData.ID);

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

                Showroom DelShowroom = Repo.GetShowroomByID(ID);
                DelShowroom.ModelState = ObjectState.SoftDelete;
                DelShowroom.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelShowroom);
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

        public JsonShowroomVM GetShowroomAsync(String ID)
        {
            try
            {
                return _mapper.Map<Showroom, JsonShowroomVM>(Repo.GetShowroomByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
