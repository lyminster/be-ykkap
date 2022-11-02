 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace RetryJob
{
    class Program
    {
        private static IConfiguration Configuration;
        private static SystemConfig _SystemConfig;
        static void Main(string[] args)
        {
       

            Configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables()
           .AddCommandLine(args)
           .Build();

            var services = new ServiceCollection();

            ConfigureServices(services);






            Process p = new Process();
            p.StartInfo.FileName = _SystemConfig.PathJob;
            p.StartInfo.Arguments = _SystemConfig.TypeJob;

            Console.WriteLine("Run Exe :" + _SystemConfig.PathJob + " param : " + _SystemConfig.TypeJob);
            p.Start();

        }

        public static void ConfigureServices(IServiceCollection services)
        {


            var systemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            services.AddSingleton(systemConfig);


       




        }





    }
    public class SystemConfig
    {















        [JsonPropertyName("ConnectionString")]
        public string ConnectionString { get; set; }
        [JsonPropertyName("IsTesting")]
        public string IsTesting { get; set; }


        [JsonPropertyName("PathJob")]
        public string PathJob { get; set; }

        [JsonPropertyName("TypeJob")]
        public string TypeJob { get; set; }

    }

}
