using AutoMapper;
using Database.Models;
using Database.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static DAL.Helper.GlobalHelpers;

namespace DAL.DataAccessLayer
{

    public class UserAccessDAL  
    {


        private readonly BusinessModelContext _dbcontext;
 
        private readonly IMapper _mapper;
        public UserAccessDAL(BusinessModelContext dbcontext, IMapper mapper, ClaimsPrincipal claimprincipal)
        {
 
            _mapper = mapper;

        }
        public JsonReturn CheckActionUser(TableName tableName, ActionEnum actionEnum, ClaimsPrincipal claims, string IDTransaksi, object JsonData)
        {
            Thread.CurrentPrincipal = claims;
            JsonReturn jsonReturn = new JsonReturn(true);
            try
            {
                String Error = "";
                if (tableName == TableName.AccomodationCost)
                {
                    if (ActionEnum.Update == actionEnum)
                    {

                    }
                    if (ActionEnum.Delete == actionEnum)
                    {

                    }
                }



                jsonReturn = new JsonReturn(true);
                jsonReturn.message = "";
                return (jsonReturn);
            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
