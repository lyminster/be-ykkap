using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Database.ConfigClass;
using Database.Models;
using DAL.Helper;

namespace HangfireJob.Service
{
    public class JobLogService 
    {
        BusinessModelContext _dbContext;
        private readonly SystemConfig _systemConfig;
        public JobLogService(BusinessModelContext dBContext, SystemConfig systemConfig)
        {
            _dbContext = dBContext;
            _systemConfig = systemConfig;

        }



















    }
}
