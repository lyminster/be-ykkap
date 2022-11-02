using AutoMapper;
using DAL.DataAccessLayer.Transaction;
using Database.ConfigClass;
using Database.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.ViewModels;

namespace DAL.DataAccessLayer.JOB
{
    public class JobServiceDAL
    {
        BusinessModelContext dbContext;
        private readonly SystemConfig _systemConfig;
        private readonly EmailService _emailService;
        private readonly EmailConfig _emailConfig;
        private readonly DODataAccessLayer _DODAL;

        public JobServiceDAL(BusinessModelContext dBContext, SystemConfig systemConfig, EmailConfig emailConfig, IMapper _mapper, IHostingEnvironment _hostenv, IConfiguration _config)
        {
            dbContext = dBContext;
            _systemConfig = systemConfig;
            _emailService = new EmailService(emailConfig);
            _emailConfig = emailConfig;
            _DODAL = new DODataAccessLayer(dBContext, _mapper, _hostenv, _config);

        }
        public void GenerateDataFromJob(string PathData, String PathDone)
        {

            _emailService.SentMail("Gen Data DO", "start gen data From DO at:" + DateTime.Now.ToString("dd-MM-yy hh:mm"), _emailConfig.DefaultTo, _emailConfig.DefaultCC, _emailConfig.DefaultBCC);

            int waktu = 0;
            string IDTable = "";
            try
            {


                var FilesProduct = Directory.GetFiles(PathData, "*.txt", SearchOption.TopDirectoryOnly);
                string errMessage = "";
                string logpath = String.Empty;
                foreach (var item in FilesProduct.ToList())
                {
                    FileInfo fileInfo = new FileInfo(item);
                    string moveto = PathDone + DateTime.Now.ToString("dd_MMM_yyyy_HH__mm_ss") + fileInfo.Name;
                    if (!Directory.Exists(PathData))
                    {
                        Directory.CreateDirectory(PathData);

                    }
                    
                    if (!Directory.Exists(PathDone))
                    {
                        Directory.CreateDirectory(PathDone);

                    }
                    GenerateDataFromFile(item, moveto);
                }
                dbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                string innerexction = ex.InnerException != null ? ex.InnerException.Message : null;

                _emailService.SentMail("====sent failed " + IDTable, ex.Message + "\n" + innerexction, _emailConfig.DefaultTo, _emailConfig.DefaultCC, _emailConfig.DefaultBCC);


            }

            _emailService.SentMail("Gen Data DO", "finish gen data From DO at:" + DateTime.Now.ToString("dd-MM-yy hh:mm"), _emailConfig.DefaultTo, _emailConfig.DefaultCC, _emailConfig.DefaultBCC);














        }

        public void GenerateDataFromFile(String NamaFile, string moveto)
        {
            try
            {
                String line = "";
                System.IO.StreamReader file = new System.IO.StreamReader(NamaFile);



                // CEK DL ADA GK MAPPINGAN FILE NAME NYA BY MARKET PLACE



                StringBuilder LogFile = new StringBuilder();
                int counter = 0;


                while ((line = file.ReadLine()) != null)
                {

                    try
                    {
                        counter++;

                        List<string> ListData = line.Split(';').ToList();
                        if (ListData.Count() < 10)
                        {
                            LogFile.AppendLine("row " + counter + "error, invalid format   ");
                            continue;

                        }

                        String DONumber = ListData.ElementAt(0) != null ? ListData.ElementAt(0).ToString().Trim() : null;

                        DOVM DOData = new DOVM();
                        try
                        {




                            var temp = _DODAL.ValidateTxtFile(DOData);
                            if (temp.Count >= 1)
                            {
                                string errorTemp = "";
                                foreach (var item in temp.ToList())
                                {
                                    errorTemp += item.ErrorMessage + " ";
                                }
                                LogFile.AppendLine("row " + counter + "error,  " + errorTemp);
                                continue;
                            }
                            var cekdata = _DODAL.GetDOByDONumber(DONumber);
                            if (cekdata != null)
                            {
                                //udpate lah cuk



                                _DODAL.UpdateTransaction(DOData);
                                LogFile.AppendLine("row " + counter + "  Succes Update Data");
                                continue;

                            }
                            else
                            {
                                _DODAL.CreateTransaction(DOData);
                                LogFile.AppendLine("row " + counter + "  Succes Insert Data");
                                continue;

                            }



                        }
                        catch (Exception ex)
                        {
                            string inner = ex.Message;
                            inner += ex.InnerException != null ? ex.InnerException : null;
                            LogFile.AppendLine("row " + counter + "  errorr " + inner);
                            continue;
                        }





                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }




                file.Close();
                //move file 

                File.Move(NamaFile, moveto);
 

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}
