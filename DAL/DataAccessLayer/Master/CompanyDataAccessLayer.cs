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
    public class CompanyDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment hostenv;
        FileLogDataAccessLayer DALFileLog;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public CompanyDataAccessLayer(BusinessModelContext _MasterEntities, IMapper mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            _mapper = mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            UnitOfWork = new UnitOfWork(_MasterEntities);
            configFile = _configFile;
            DALFileLog = new FileLogDataAccessLayer(MasterEntities, _mapper, _hostenv, configFile);
        }

        public CompanyRepository _Repo;
        public CompanyRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new CompanyRepository(MasterEntities);
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
        #region Method

        public JsonReturn SaveAsync(JsonCompanyVM Save, ClaimsPrincipal claims)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                String Error = "";
                if (String.IsNullOrEmpty(Save.Id))
                {
                    if (Repo.IsUniqueKeyCodeExist(Save.Code, Save.Name, GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims))) Error = "Company ini sudah ada di Database";

                    jsonReturn = new JsonReturn(false);
                    jsonReturn.message = Error;

                }
                if (String.IsNullOrEmpty(Error))
                {

                    GlobalHelpers.ReGenerateThreadClaim(claims);
                    Company NewData = new Company();
                    if (String.IsNullOrEmpty(Save.Id))
                    {
                        NewData.ID = Guid.NewGuid().ToString("N").ToUpper();
                        NewData.Code = Save.Code;
                        NewData.Name = Save.Name;
                        NewData.ModelState = ObjectState.Added;

                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();
                    }
                    if (!String.IsNullOrEmpty(Save.Id))
                    {
                        NewData = Repo.GetCompanyByID(Save.Id);
                        if (NewData == null)
                        {
                            jsonReturn = new JsonReturn(false);
                            jsonReturn.message = "Data tidak ditemukan";
                            return jsonReturn;
                        }

                        NewData.Code = Save.Code;
                        NewData.Name = Save.Name;
                        NewData.ModelState = ObjectState.Modified;
                        UnitOfWork.InsertOrUpdate(NewData);
                        UnitOfWork.Commit();

                    }
                    NewData = Repo.GetCompanyByID(NewData.ID);

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

        public JsonCompanyVM GetCompanyAsync(String ID)
        {
            try
            {
                return _mapper.Map<Company, JsonCompanyVM>(Repo.GetCompanyByID(ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<JsonCompanyVM> FindAsync(JsonCompanyVM filter, ClaimsPrincipal claims)
        {
            List<JsonCompanyVM> listCompany = new List<JsonCompanyVM>();
            JsonReturn returnData = new JsonReturn(false);
            try
            {
                returnData = new JsonReturn(true);
                String IDClient = GlobalHelpers.GetClaimValueByType(EnumClaims.IDClient.ToString(), claims);
                Expression<Func<Company, bool>> filterExp = c => true && c.Idclient == IDClient;
                if (!String.IsNullOrEmpty(filter.Id)) filterExp = filterExp.And(x => x.ID == filter.Id);
                if (!String.IsNullOrEmpty(filter.Code))
                {
                    filterExp = filterExp.And(x => x.Code != null);
                    filterExp = filterExp.And(x => x.Code.ToLower().Contains(filter.Code.ToLower()));
                }
                if (!String.IsNullOrEmpty(filter.Name))
                {
                    filterExp = filterExp.And(x => x.Name != null);
                    filterExp = filterExp.And(x => x.Name.ToLower().Contains(filter.Name.ToLower()));
                }


                listCompany = _mapper.Map<IEnumerable<Company>, List<JsonCompanyVM>>(Repo.QueryCompanys(filterExp, filter.Take, filter.Skip));
                return listCompany;

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

                Company DelCompany = Repo.GetCompanyByID(ID);
                DelCompany.ModelState = ObjectState.SoftDelete;
                DelCompany.SetRowStatus(RowStatus.Deleted);
                UnitOfWork.InsertOrUpdate(DelCompany);
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


        #endregion


        public JsonReturn GenerateData(string filename, string pathLog, string idLog, string createdby, out string result, out string logpath)
        {
            JsonReturn dataRespon = new JsonReturn(true);
            try
            {
                result = "";
                logpath = String.Empty;

                String FileName = filename;
                String PathHistory = pathLog + @"\History";

                string FileNameWithoutExt = Path.GetFileNameWithoutExtension(filename); ;

                try
                {
                    JsonReturn _json = GenerateImport(pathLog, pathLog + "/" + FileNameWithoutExt + "_LOGEXPORT.txt", idLog, createdby);
                    if (_json.result == false)
                    {
                        dataRespon.result = false;
                        dataRespon.message = _json.message;
                    }
                }
                catch (Exception ex)
                {
                    String InnerException = ex.InnerException != null ? ex.InnerException.Message : null;
                    var results = ex.Message + InnerException;
                    dataRespon.message = results;
                    dataRespon.result = false;
                    return dataRespon;
                }
                return dataRespon;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JsonReturn GenerateImport(String NamaFile, string PathLog, String IDFileLog, String createdby)
        {
            JsonReturn jsonReturn = new JsonReturn(true);
            try
            {
                StringBuilder LogFile = new StringBuilder();
                int counter = 0;

                DataTable DTGENERATE = ExcelHelper.GetRequestsDataFromExcel(NamaFile);
                if (DTGENERATE != null && DTGENERATE.Rows.Count > 0)
                {
                    if (ExcelHelper.ContainColumn("CompanyCode", DTGENERATE)
                        && ExcelHelper.ContainColumn("CompanyName", DTGENERATE)
                        )
                    {
                        //declare log file
                        List<FileLogDetailVM> logDetail = new List<FileLogDetailVM>();
                        int no = 0;

                        for (int i = 0; i < DTGENERATE.Rows.Count; i++)
                        {
                            String CompanyCode = DTGENERATE.Rows[i]["CompanyCode"].ToString();
                            String CompanyName = DTGENERATE.Rows[i]["CompanyName"].ToString();

                            FileLogDetailVM itemLogDetail = new FileLogDetailVM();
                            itemLogDetail.Status = true;
                            List<CompanyVM> company_temp = new List<CompanyVM>();

                            if (String.IsNullOrEmpty(CompanyCode))
                            {
                                itemLogDetail.Status = false;
                                itemLogDetail.Remarks = itemLogDetail.Remarks + "CompanyCode is mandatory! ";
                            }

                            if (String.IsNullOrEmpty(CompanyName))
                            {
                                itemLogDetail.Status = false;
                                itemLogDetail.Remarks = itemLogDetail.Remarks + "CompanyName is mandatory! ";
                            }

                            if (itemLogDetail.Status == true)
                            {
                                //cek data ke database
                                Company CekData = MasterEntities.Companies.FirstOrDefault(x => x.Code.ToUpper() == CompanyCode.ToUpper());

                                if (CekData != null)
                                {
                                    CekData.Name = CompanyName;


                                    itemLogDetail.Remarks = "Updated Existing CompanyCode : " + CompanyCode;
                                    CekData.ModelState = ObjectState.Modified;

                                    UnitOfWork.InsertOrUpdate(CekData);
                                    UnitOfWork.Commit();
                                }
                                else
                                {
                                    Company new_company = new Company();
                                    new_company.ID = CompanyCode;
                                    new_company.Code = CompanyCode;
                                    new_company.Name = CompanyName;



                                    itemLogDetail.Remarks = "Inserted New CompanyCode : " + CompanyCode;

                                    MasterEntities.Companies.Add(new_company);
                                    new_company.ModelState = ObjectState.Added;

                                    UnitOfWork.InsertOrUpdate(CekData);
                                    UnitOfWork.Commit();
                                }




                            }

                            //FileLog
                            itemLogDetail.ID = Guid.NewGuid().ToString();
                            itemLogDetail.IDFileLog = IDFileLog;
                            itemLogDetail.Status = true;
                            itemLogDetail.CreatedBy = createdby;
                            itemLogDetail.CreatedTime = DateTime.Now;
                            itemLogDetail.OrderNo = no + 1;
                            DALFileLog.CreateDetail(itemLogDetail);

                        }

                    }
                    else
                    {
                        var remarks = "wrong format";
                        DALFileLog.UpdateStatusHeader(new FileLogVM
                        {
                            ID = IDFileLog,
                            Status = false,
                            Remarks = remarks
                        });
                        jsonReturn.message = remarks;
                        jsonReturn.result = false;
                    }
                }

            }
            catch (Exception ex)
            {
                var remarks = "wrong format";
                DALFileLog.UpdateStatusHeader(new FileLogVM
                {
                    ID = IDFileLog,
                    Status = false,
                    Remarks = ex.Message
                });
                jsonReturn.message = remarks;
                jsonReturn.result = false;
            }

            return jsonReturn;
        }

    }
}
