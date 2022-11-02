using AutoMapper;
using ConsoleJobTMS.Service;
using DAL.DataAccessLayer.JOB;
using DAL.Helper;
using Database.ConfigClass;
using Database.Models;
using Database.ViewModels;
using HangfireJob.Helpers;
using HangfireJob.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using VendorRegis.Automapper;

namespace ConsoleJobTMS
{
    class Program
    {
        private static IConfiguration Configuration;
        private static SystemConfig _SystemConfig;
        private static EmailConfig _EmailConfig;
        private static TxtConsumerFile _TxtFileConsumerFile;

        private static EmailService _EmailService;
        private static IMapper _Mapper;

        private static readonly IHostingEnvironment _hostenv;
        private static readonly IMapper _mapper;

        private static BusinessModelContext _DbContext;

        private static string TypeJob = "";
        private static string KODETRANSPORTER = "";
     
        static async System.Threading.Tasks.Task Main(string[] args)
        {




            Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .AddCommandLine(args)
           .Build();

            var services = new ServiceCollection();

            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();




            _DbContext = serviceProvider.GetService<BusinessModelContext>();

            _EmailConfig = serviceProvider.GetService<EmailConfig>();
            _SystemConfig = serviceProvider.GetService<SystemConfig>();




            _TxtFileConsumerFile = new TxtConsumerFile(_DbContext, _SystemConfig, _EmailConfig);
            _EmailService = new EmailService(_EmailConfig);


            if (Convert.ToBoolean(_SystemConfig.IsTesting))
            {
                Console.WriteLine("Enter job type to run: ");

                // using the method
                // typecasting not needed
                // as ReadLine returns string
                TypeJob = Console.ReadLine();




            }
            else
            {
                Console.WriteLine("running ==>" + TypeJob);
                clsUtility.systemConfig = _SystemConfig;
                FICHConverterClass JOBConverter = new FICHConverterClass();
                JOBConverter.openConn();
                JOBConverter.timer1_Tick(_SystemConfig);
            }



  




            string ConsolePath = AppDomain.CurrentDomain.BaseDirectory;

           

            var ListJob = _DbContext.HelperTables.Where(x => x.Code == "JOBSP").ToList();
            foreach (var item in ListJob)
            {
                HangfireJob.Models.JobParameter jobParam = new HangfireJob.Models.JobParameter();
                jobParam.IDJob = item.ID;
                jobParam.JsonObject = item.Description;


                //List<HelperTable> ListJOBTRANSPORTER = _DbContext.HelperTables.Where(X => X.Code != null).ToList().Where(x => x.Code != null).ToList().Where(x => x.Code.ToUpper() == ConstantVariable.TRANSPOTER_JOB.ToUpper()).ToList();
                List<HelperTable> ListJobTxt = _DbContext.HelperTables.Where(X => X.Code != null).ToList().Where(x => x.Code.ToUpper() == ConstantVariable.XMLJOB.ToUpper()).ToList();





                if (TypeJob == ConstantVariable.XMLJOB)
                {
                    if (item.ID == ConstantVariable.XMLJOB)
                    {

                        if (ListJobTxt != null)
                        {
                            foreach (var data in ListJobTxt.ToList())
                            {
                                Console.WriteLine("running ==>" + TypeJob + "->" + data.ID);
                                _TxtFileConsumerFile.GenerateDataFromSourceFile(data.Value, data.Description, data.Name);
                            }
                        }
                    }
                    if (item.ID == ConstantVariable.TxtJOB)
                    {
                        
                    }
                }






            }

            Console.WriteLine("finish running at:" + DateTime.Now.ToString("dd-MMM-yy HH:mm"));
            Thread.Sleep(10000);
            Environment.Exit(0);























        }





        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BusinessModelContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));





            services.AddDbContext<BusinessModelContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));






            var emailConfig = Configuration.GetSection("Smtp").Get<EmailConfig>();
            services.AddSingleton(emailConfig);


            var systemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            services.AddSingleton(systemConfig);



            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


        }







        public static Boolean CheckCronnExpresionIsNow(string Cronn)
        {
            try
            {

                var date = DateTime.Now;
                var expression = new CronExpression(Cronn) { TimeZone = TimeZoneInfo.Utc };
                var next = Convert.ToDateTime(expression.GetNextValidTimeAfter(date));
                if (date.Hour == next.Hour && date.Date == next.Date && date.Minute == next.Minute)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }

        }





    }
}
