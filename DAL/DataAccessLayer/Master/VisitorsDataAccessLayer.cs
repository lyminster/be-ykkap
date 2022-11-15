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

namespace DAL.DataAccessLayer.Master
{
    public class VisitorsDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper Mapper;
        private readonly IHostingEnvironment hostenv;
        private static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration configFile;
        private readonly string conString;
        private readonly string _key;
        public VisitorsDataAccessLayer(BusinessModelContext _MasterEntities, IMapper _mapper, IHostingEnvironment _hostenv, IConfiguration _config)
        {
            configFile = _config;
            Mapper = _mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            conString = configFile.GetConnectionString("SPConnection");
            _key = configFile.GetConnectionString("EncDecKey");

        }
    }
}
