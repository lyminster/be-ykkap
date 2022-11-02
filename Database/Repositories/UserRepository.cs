using Database.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MainProject.Models;

using Microsoft.Extensions.Configuration;
using ViewModel.Constant;

namespace Database.Repositories
{
    public class UserRepository
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IConfiguration configFile;

        private readonly string connString;

        public UserRepository(BusinessModelContext _MasterEntities, IConfiguration _config)
        {
            MasterEntities = _MasterEntities;
            configFile = _config;
            connString = configFile.GetConnectionString("SPConnection");
        }

        public DataTable FindUserBySP()
        {
            String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

            List<SqlParameter> listparam = new List<SqlParameter>();

            var result = DataTableHelper.ExecStoreProcedure(connString, "Sp_GetUser", null, timeout_duration);
            return result;
        }
        public List<User> FindUser(UserViewModel filter)
        {
            try
            {
                IEnumerable<User> Result = MasterEntities.Users;

                if (filter != null)
                {
                    if (!String.IsNullOrEmpty(filter.Idclient))
                    {
                        Result = Result.Where(x => x.Idclient == filter.Idclient);
                    }


                    if (!String.IsNullOrEmpty(filter.Email))
                    {
                        if (!String.IsNullOrEmpty(filter.Password))
                        {
                            Result = Result.Where(x => x.Email.ToLower() == filter.Email.ToLower())
                            .Where(x => x.Password == filter.Password);
                        }
                        else
                        {
                            Result = Result.Where(x => x.Email.ToLower() == filter.Email.ToLower());
                        }

                    }

                }
                return Result.ToList();
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }




        public User GetUserByID(String ID)
        {
            try
            {
                return MasterEntities.Users.FirstOrDefault(x => x.ID == ID);
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public User GetUserByEmail(string? Email)
        {
            try
            {
                var temp = MasterEntities.Users.ToList();
                return MasterEntities.Users.FirstOrDefault(x => x.Email == Email);
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public void AddNewUser(User newUser)
        {
            try
            {
                MasterEntities.Users.Add(newUser);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public void Commit()
        {
            try
            {
                MasterEntities.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CekIsExist(string Nama)
        {
            try
            {
                return true;
                // return MasterEntities.Users.FirstOrDefault(x => x.Nama.ToLower() == Nama.ToLower()) != null ? true : false;
            }
            catch (Exception ex)
            {
                return false;
                //throw ex;
            }
        }

        public void DeleteUser(User DelUser)
        {
            try
            {
                MasterEntities.Users.Remove(DelUser);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }
    }
}
