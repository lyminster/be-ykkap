using Database.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ViewModel.Constant;
using ViewModel.ViewModels;

namespace Database.Repositories
{
    public class FileLogRepository
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly string connString;
        private readonly IConfiguration configFile;
        public FileLogRepository(BusinessModelContext _MasterEntities, IConfiguration _config)
        {
            MasterEntities = _MasterEntities;
            configFile = _config;
            connString = configFile.GetConnectionString("SPConnection");
        }

        public List<FileLog> GetAll(FileLogVM filter)
        {
            try
            {
                IEnumerable<FileLog> Result = MasterEntities.FileLogs;
                if (!String.IsNullOrEmpty(filter.TableName))
                {
                    Result = Result.Where(x => x.TableName == filter.TableName);
                }
                if (filter.Status != null)
                {
                    Result = Result.Where(x => x.Status == filter.Status);
                }

                //get last 8 days
                var dateCriteria = DateTime.Now.Date.AddDays(-7);

                Result = Result.Where(x => x.CreatedTime >= dateCriteria).OrderByDescending(x => x.CreatedTime);

                return Result.ToList();
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public IQueryable<String> GetAllCreatedByFileLog()
        {
            try
            {
                return MasterEntities.FileLogDetails.Select(x => x.CreatedBy).Distinct();

            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public DataTable GetListFileLogDetail()
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");
                var result = DataTableHelper.ExecStoreProcedure(connString, "SPT_GetViewFileLogTransporter", null, timeout_duration);
                return result;

            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public FileLog GetFileLog(string id)
        {
            try
            {
                return MasterEntities.FileLogs.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<FileLogDetail> GetAllFileLogDetail()
        {
            try
            {
                return MasterEntities.FileLogDetails.OrderByDescending(x => x.CreatedTime).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<FileLogDetail> FilterFileLogDetail(String filterCodeData, DateTime? dari, DateTime? sampai)
        //{
        //    try
        //    {
        //        IEnumerable<FileLogDetail> data = MasterEntities.FileLogDetails;
        //        if(filterCodeData != null)
        //        {
        //            data = data.Where(x=>x.CodeData == filterCodeData);
        //        }
        //        if (dari != null)
        //        {
        //            data = data.Where(x => x.CreatedTime >= dari);
        //        }
        //        if (sampai != null)
        //        {
        //            data = data.Where(x => x.CreatedTime <= sampai);
        //        }

        //        return data.ToList();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public List<FileLogDetailVM> FilterFileLogDetail(String filterCodeData, DateTime? dari, DateTime? sampai, bool? status)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                string constring = connString;
                DataTable dt = new DataTable();
                SqlDataReader reader = null;
                var namaSP = "SPT_GetFileLogByID";
                List<FileLogDetailVM> ListResult = new List<FileLogDetailVM>();

                using (SqlConnection conn = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(namaSP, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                        if (!String.IsNullOrEmpty(filterCodeData))
                        {
                            cmd.Parameters.AddWithValue("@IDFileLog", filterCodeData);
                        }
                        cmd.Parameters.AddWithValue("@TanggalDari", dari);
                        cmd.Parameters.AddWithValue("@TanggalSampai", sampai);

                        if(status != null)
                        {
                            cmd.Parameters.AddWithValue("@Status", status);
                        }
                        


                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = Convert.ToInt32(timeout_duration);

                        conn.Open();
                        reader = cmd.ExecuteReader();

                        dt.Load(reader);

                        reader.Close();
                        conn.Close();


                    }
                }


                //resultGR = Mapper.Map<List<VDODetail>, List<VDODetailVM>>(CData);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ListResult.Add(new FileLogDetailVM
                        {

                            ID = row["ID"].ToString(),
                            Remarks = row["DONumber"].ToString(),
                            CreatedBy = row["CreatedBy"].ToString(),
                            CodeData = row["CodeData"].ToString(),
                            CreatedTime = Convert.ToDateTime(row["CreatedTime"].ToString())
                        });
                    }
                }


                return ListResult;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
        public List<FileLogDetailVM> FilterFileLogDetailSingle(String ID)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                string constring = connString;
                DataTable dt = new DataTable();
                SqlDataReader reader = null;
                var namaSP = "SPT_GetFileLogSingle";
                List<FileLogDetailVM> ListResult = new List<FileLogDetailVM>();

                using (SqlConnection conn = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(namaSP, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;


                         
                            cmd.Parameters.AddWithValue("@IDFileLog", ID);
                         


                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandTimeout = Convert.ToInt32(timeout_duration);

                        conn.Open();
                        reader = cmd.ExecuteReader();

                        dt.Load(reader);

                        reader.Close();
                        conn.Close();


                    }
                }


                //resultGR = Mapper.Map<List<VDODetail>, List<VDODetailVM>>(CData);
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        ListResult.Add(new FileLogDetailVM
                        {

                            ID = row["ID"].ToString(),
                            Remarks = row["DONumber"].ToString(),
                            CreatedBy = row["CreatedBy"].ToString(),
                            CodeData = row["CodeData"].ToString(),
                            CreatedTime = Convert.ToDateTime(row["CreatedTime"].ToString())
                        });
                    }
                }


                return ListResult;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public List<FileLogDetail> GetFileLogDetail(string id)
        {
            try
            {
                return MasterEntities.FileLogDetails.Where(x => x.IDFileLog == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

    }
}
