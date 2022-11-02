using Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Database.Repositories
{
    public class UserTypeRepository
    {
        private readonly BusinessModelContext MasterEntities;

        public UserTypeRepository(BusinessModelContext _MasterEntities)
        {
            MasterEntities = _MasterEntities;
        }

        public List<UserType> GetAll()
        {
            try
            {
                List<UserType> Result = MasterEntities.UserTypes.ToList();

                return Result;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
        public List<UserType> GetAllPriviledge()
        {
            try
            {
                List<UserType> Result = MasterEntities.UserTypes.ToList();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public UserType GetByCode(string code)
        {
            try
            {
                UserType Result = MasterEntities.UserTypes.FirstOrDefault(x => x.Code == code);

                return Result;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }


        public UserType GetByID(string id)
        {
            try
            {
                UserType Result = MasterEntities.UserTypes.FirstOrDefault(x => x.ID == (id));

                return Result;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

    }
}
