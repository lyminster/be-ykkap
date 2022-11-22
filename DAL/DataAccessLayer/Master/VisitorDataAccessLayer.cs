using AutoMapper;
using Database.Models;
using Database.Repositories;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using MainProject.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Database.ViewModels;
using System.Net.Http.Headers;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using ViewModel.ViewModels;
using ViewModel.Constant;
using System.Threading.Tasks;
using System.Linq;
using ViewModel.ViewModels.Master;
using System.Security.Claims;
using DAL.Helper;
using static DAL.Helper.GlobalHelpers;
using System.Linq.Expressions;
using static Database.Models.HelperFunction;

namespace DAL.DataAccessLayer.Master
{
    public class VisitorDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        private readonly HttpClient client = new HttpClient();
        private readonly IConfiguration configFile;
        private readonly string conString;
        private readonly string _key;
        private UnitOfWork UnitOfWork;
        public VisitorDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _config)
        {
            configFile = _config;
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            conString = configFile.GetConnectionString("SPConnection");
            _key = configFile.GetConnectionString("EncDecKey");

        }

        public VisitorRepository _Repo;
        public VisitorRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new VisitorRepository(MasterEntities);
                }
                return _Repo;
            }
        }



        public List<JsonVisitorVM> GetListVisitorAsync()
        {
            try
            {
                return _mapper.Map<IEnumerable<Visitor>, List<JsonVisitorVM>>(Repo.GetListVisitor());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonVisitorVM> FindAsync(JsonVisitorVM filter, ClaimsPrincipal claims, DateTime? tglFrom, DateTime? tglTo)
        {
            List<JsonVisitorVM> listVisitor = new List<JsonVisitorVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<Visitor, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.Name))
                {
                    filterExp = filterExp.And(x => x.Name != null);
                    filterExp = filterExp.And(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.Email))
                {
                    filterExp = filterExp.And(x => x.Email != null);
                    filterExp = filterExp.And(x => x.Email.ToLower().Contains(filter.Email.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.PhoneNumber))
                {
                    filterExp = filterExp.And(x => x.PhoneNumber != null);
                    filterExp = filterExp.And(x => x.PhoneNumber.ToLower().Contains(filter.PhoneNumber.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.AccessFrom))
                {
                    filterExp = filterExp.And(x => x.AccessFrom != null);
                    filterExp = filterExp.And(x => x.AccessFrom.ToLower().Contains(filter.AccessFrom.ToLower()));
                }
                if(tglFrom != null)
                {
                    filterExp = filterExp.And(x => x.CreatedTime >= tglFrom);
                }
                if (tglTo != null)
                {
                    filterExp = filterExp.And(x => x.CreatedTime <= tglTo);
                }

                var cek = Repo.QueryVisitors(filterExp, filter.Take, filter.Skip).ToList();
                listVisitor = _mapper.Map<List<Visitor>, List<JsonVisitorVM>>(cek);
                return listVisitor;

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

                Visitor DelVisitor = Repo.GetVisitorByID(ID);
                DelVisitor.ModelState = ObjectState.SoftDelete;
                DelVisitor.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelVisitor);
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
        public JsonReturn SaveAsync(JsonVisitorVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    Visitor NewData = new Visitor();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.Name = Save.Name;
                        NewData.Email = Save.Email;
                        NewData.PhoneNumber = Save.PhoneNumber;
                        NewData.AccessFrom = Save.AccessFrom;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetVisitorByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.Name = Save.Name;
                        NewData.Email = Save.Email;
                        NewData.PhoneNumber = Save.PhoneNumber;
                        NewData.AccessFrom = Save.AccessFrom;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetVisitorByID(NewData.ID);

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
    }
}
