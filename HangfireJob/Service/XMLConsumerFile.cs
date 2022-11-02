using DAL.DataAccessLayer.JOB;
using DAL.Helper;
using Database.ConfigClass;
using Database.Models;
using Database.ViewModels;
using Hangfire;
using HangfireJob.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HangfireJob.Service
{
    public class XMLConsumerFile
    {
        BusinessModelContext _dbContext;
        private readonly SystemConfig _systemConfig;
        private readonly EmailConfig _emailConfig;
        private readonly EmailService _emailService;
        public XMLConsumerFile(BusinessModelContext dBContext, SystemConfig systemConfig, EmailConfig emailConfig)
        {
            _dbContext = dBContext;
            _systemConfig = systemConfig;
            _emailConfig = emailConfig;
            _emailService = new EmailService(emailConfig);
        }

        String CreatedBy = ConstantVariable.XMLJOB;

        [DisableConcurrentExecution(10 * 60)]
        public void GenerateDataFromSourceFile(string PathData, string PathDone, string jobname)
        {
            PathData = PathData.Trim();
            PathDone = PathDone.Trim();

            GlobalHelper.PostJobsLog(CreatedBy, jobname, "start gen data " + CreatedBy + " at:" + DateTime.Now.ToString("dd-MMM-yy HH:mm"), Guid.NewGuid().ToString(), _dbContext, _emailService, true);

            int waktu = 0;
            int totalrow = 0;
            int totalprocess = 0;
            string IDTable = "";

            try
            {


                var FilesProduct = Directory.GetFiles(PathData, "*.txt", SearchOption.TopDirectoryOnly).OrderByDescending(d => new FileInfo(d).CreationTime);

                string errMessage = "";
                string logpath = String.Empty;
                List<HelperTable> helperTable = _dbContext.HelperTables.Where(x => x.Code == CreatedBy + "_PREFIX").ToList();
                foreach (var item in FilesProduct.ToList())
                {
                    totalrow++;
                    FileInfo fileinfx = new FileInfo(item);
                    GlobalHelper.PostJobsLog(CreatedBy, jobname, fileinfx.Name + " checking file name" + DateTime.Now.ToString("dd-MMM-yy HH:mm"), Guid.NewGuid().ToString(), _dbContext, _emailService);
                    bool b = helperTable.Select(x => x.Value).Distinct().ToList().Any(s => item.Contains(s));
                    if (!b)
                    {
                        continue;
                    }
                    totalprocess++;
                    GlobalHelper.PostJobsLog(CreatedBy, jobname, fileinfx.Name + " file name contain start process " + DateTime.Now.ToString("dd-MMM-yy HH:mm"), Guid.NewGuid().ToString(), _dbContext, _emailService);

                    FileInfo fileInfo = new FileInfo(item);

                    string moveto = PathDone + fileInfo.Name;
                    if (!Directory.Exists(PathData))
                    {
                        Directory.CreateDirectory(PathData);

                    }
                    if (!Directory.Exists(PathDone))
                    {
                        Directory.CreateDirectory(PathDone);

                    }

                    ExecuteFile(item, moveto);
                }





            }
            catch (Exception ex)
            {
                string innerexction = ex.InnerException != null ? ex.InnerException.Message : null;

                GlobalHelper.PostJobsLog("====sent failed " + IDTable, jobname, ex.Message + "\n" + innerexction, (IDTable), _dbContext, _emailService);
                // StatusLoading = ("Error at : " + IDTable + "\n" + ex.Message + "\n" + innerexction);

            }

            GlobalHelper.PostJobsLog(CreatedBy, jobname, "finish gen " + CreatedBy + " at:" + DateTime.Now.ToString("dd-MMM-yy HH:mm"), Guid.NewGuid().ToString(), _dbContext, _emailService);


        }




        [DisableConcurrentExecution(10 * 60)]
        public void ExecuteFile(String NamaFile, string moveto)
        {
            try
            {
                String linedata = "";
                Console.WriteLine("start consume file ");
                System.IO.StreamReader file = new System.IO.StreamReader(NamaFile);
                FileInfo fileInfo = new FileInfo(NamaFile);


                // CEK DL ADA GK MAPPINGAN FILE NAME NYA BY MARKET PLACE
                ///cek dl pernah di proses belum neh file
                var cekfile = _dbContext.FileLogs.FirstOrDefault(x => x.FileName == fileInfo.Name && x.Remarks.Contains("FINISH"));
                FileLog headerlog = new FileLog();
                if (cekfile != null)
                {

                    headerlog.CreatedBy = CreatedBy;
                    headerlog.CreatedTime = DateTime.Now;
                    headerlog.FileName = fileInfo.Name;
                    headerlog.ID = Guid.NewGuid().ToString();
                    headerlog.TableName = CreatedBy;
                    headerlog.Status = false;
                    headerlog.Remarks = "File Allready Process".ToUpper();
                    GlobalHelper.InsertFileLogHeader(headerlog, _dbContext.Database.GetConnectionString());
                    return;
                }
                headerlog = new FileLog();

                headerlog.CreatedBy = CreatedBy;
                headerlog.CreatedTime = DateTime.Now;
                headerlog.FileName = fileInfo.Name;
                headerlog.ID = Guid.NewGuid().ToString();
                headerlog.TableName = CreatedBy;
                headerlog.Status = true;
                headerlog.Remarks = "ON PROCESS";

                GlobalHelper.InsertFileLogHeader(headerlog, _dbContext.Database.GetConnectionString());



                int counter = 0;

                List<System.Data.SqlClient.SqlParameter> ListParam = new List<System.Data.SqlClient.SqlParameter>();

                ITX_CLOSE_EXPORT myObject = new ITX_CLOSE_EXPORT();
                string xml = File.ReadAllText(@NamaFile);
              
                myObject = GlobalHelpers.DeserializeObject<ITX_CLOSE_EXPORT>(xml);



                Console.WriteLine("start hit sp1 :" + "SP_GenerateDOFromSAP_1");
                ListParam = new List<System.Data.SqlClient.SqlParameter>();
                GlobalHelpers.ExecStoreProcedure(_dbContext.Database.GetConnectionString(), "SP_GenerateDOFromSAP_1", null);
                Console.WriteLine("end hit sp1 :" + "SP_GenerateDOFromSAP_1");

                GlobalHelper.UpdateLogHeader(headerlog, _dbContext.Database.GetConnectionString());
                Console.WriteLine("end consume file ");



                file.Close();

                //move file 
                try
                {
                    Console.WriteLine("start hit sp :" + "SP_GenerateDOFromSAP_1");
                    ListParam = new List<System.Data.SqlClient.SqlParameter>();
                    GlobalHelpers.ExecStoreProcedure(_dbContext.Database.GetConnectionString(), "SP_GenerateDOFromSAP_1", null);
                    Console.WriteLine("end hit sp :" + "SP_GenerateDOFromSAP_1");
                }
                catch (Exception ex)
                {
                    string IE = ex.InnerException != null ? ex.InnerException.Message : null;
                    Console.WriteLine("failed hit sp :" + "SP_GenerateDOFromSAP_1" + ex.Message + IE);
                }



                Console.WriteLine("end process");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }
}
