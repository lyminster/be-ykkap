using AutoMapper;
using DAL.Helper;
using Database.Models;
using Database.Repositories;
using Database.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MainProject.Models;
using ViewModel.ViewModels;

namespace DAL.DataAccessLayer.Master
{
    public class FileLogDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper Mapper;
        private readonly IHostingEnvironment hostenv;
        private readonly IConfiguration configFile;
        private UnitOfWork UnitOfWork;

        public FileLogDataAccessLayer(BusinessModelContext _MasterEntities, IMapper _mapper, IHostingEnvironment _hostenv, IConfiguration _configFile)
        {
            Mapper = _mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            configFile = _configFile;
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


        public List<FileLogDetailVM> GetListRequest()
        {
            List<FileLogDetailVM> listCost = new List<FileLogDetailVM>();
            var dt = FileLogRepo.GetListFileLogDetail();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    listCost.Add(new FileLogDetailVM
                    {
                        Remarks = row["Remarks"].ToString(),
                        CreatedBy = row["CreatedBy"].ToString(),
                        CreatedTime = row["CreatedTime"].ToString() != null ? Convert.ToDateTime(row["CreatedTime"].ToString()) : null,
                        FileName = row["FileName"].ToString(),

                    });

                }

            }

            return listCost;
        }




        public List<FileLogDetailVM> GetListFileLogTransporter(FileLogVM filter)
        {
            var result = FileLogRepo.GetAllFileLogDetail();
            return Mapper.Map<List<FileLogDetail>, List<FileLogDetailVM>>(result);
        }


        public List<FileLogDetailVM> FilterFileLogTransporter(IndexFileLogVM filter, String TglDari, String TglSampai, bool? status)
        {
            var codeData = filter.FilterDONumber;
            var dari = Convert.ToDateTime(TglDari);
            var sampai = Convert.ToDateTime(TglSampai);
            var result = FileLogRepo.FilterFileLogDetail(codeData, dari, sampai, status);
            return result;

        }

        public List<FileLogDetailVM> FilterFileLogDetailSingle(string ID)
        { 
            var result = FileLogRepo.FilterFileLogDetailSingle(ID);
            return result;

        }

        

        public List<FileLogVM> GetListFileLog(FileLogVM filter)
        {
            var result = FileLogRepo.GetAll(filter);
            return Mapper.Map<List<FileLog>, List<FileLogVM>>(result);
        }

        public List<FileLogVM> GetAllCreatedByFileLog()
        {
            var result = FileLogRepo.GetAllCreatedByFileLog();
            return result.Select(x => new FileLogVM { CreatedBy = x }).ToList();
        }
        public List<FileLogDetailVM> GetListFileLogDetail(string idFileLog)
        {
            var result = FileLogRepo.GetFileLogDetail(idFileLog);
            return Mapper.Map<List<FileLogDetail>, List<FileLogDetailVM>>(result);
        }

        public JsonReturn Create(FileLogVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                FileLog updateData = new FileLog();
                updateData.ID = data.ID;
                updateData.TableName = data.TableName;
                updateData.FileName = data.FileName;

                updateData.ModelState = HelperFunction.ObjectState.Added;
                UnitOfWork.InsertOrUpdate(updateData);
                UnitOfWork.Commit();
         
                jsonReturn.message = "Success";
                jsonReturn.result = true;
                return jsonReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public JsonReturn CreateUploadLog(FileLogVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                FileLog updateData = new FileLog();
                updateData.ID = data.ID;
                updateData.TableName = data.TableName;
                updateData.FileName = data.FileName;

                updateData.ModelState = HelperFunction.ObjectState.Added;
                UnitOfWork.InsertOrUpdate(updateData);
                UnitOfWork.Commit();

                jsonReturn.message = "Success";
                jsonReturn.result = true;
                return jsonReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonReturn UpdateStatusHeader(FileLogVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);
            try
            {
                if (data != null)
                {
                    FileLog logHeader = MasterEntities.FileLogs.FirstOrDefault(x => x.ID == data.ID);
                    logHeader.Status = data.Status;
                    logHeader.Remarks = data.Remarks;


                    logHeader.ModelState = HelperFunction.ObjectState.Added;
                    UnitOfWork.InsertOrUpdate(logHeader);
                    UnitOfWork.Commit();
                    jsonReturn.message = "Success";
                    jsonReturn.result = true;

                }
                return jsonReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonReturn CreateDetail(FileLogDetailVM data)
        {
            JsonReturn jsonReturn = new JsonReturn(false);

            try
            {
                FileLogDetail updateData = new FileLogDetail();
                updateData.ID = data.ID;
                updateData.IDFileLog = data.IDFileLog;
                updateData.OrderNo = data.OrderNo;
                updateData.Status = data.Status;
                updateData.Remarks = data.Remarks;
 

            
                jsonReturn.message = "Success";
                jsonReturn.result = true;
                return jsonReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
