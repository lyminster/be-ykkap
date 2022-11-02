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
    public class UserDataAccessLayer
    {
        private readonly BusinessModelContext MasterEntities;
        private readonly IMapper Mapper;
        private readonly IHostingEnvironment hostenv;
        private static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration configFile;
        private readonly string conString;
        private readonly string _key;

        public UserDataAccessLayer(BusinessModelContext _MasterEntities, IMapper _mapper, IHostingEnvironment _hostenv, IConfiguration _config)
        {
            configFile = _config;
            Mapper = _mapper;
            hostenv = _hostenv;
            MasterEntities = _MasterEntities;
            conString = configFile.GetConnectionString("SPConnection");
            _key = configFile.GetConnectionString("EncDecKey");

        }
        public UserRepository _Repo;
        public UserRepository Repo
        {
            get
            {
                if (_Repo == null)
                {
                    _Repo = new UserRepository(MasterEntities, configFile);
                }
                return _Repo;
            }
        }

        public UserTypeRepository _userTypeRepo;
        public UserTypeRepository userTypeRepo
        {
            get
            {
                if (_userTypeRepo == null)
                {
                    _userTypeRepo = new UserTypeRepository(MasterEntities);
                }
                return _userTypeRepo;
            }
        }



        public List<userTypeDetails> getUserType()
        {
            List<userTypeDetails> l3 = new List<userTypeDetails>();
            List<UserType> listUserType = userTypeRepo.GetAll();
            for (int i = 0; i < listUserType.Count; i++)
            {
                userTypeDetails c3 = new userTypeDetails();
                c3.userTypeName = listUserType[i].Name;
                c3.userTypeCode = listUserType[i].Code;
                c3.id = Convert.ToString(listUserType[i].ID);
                l3.Add(c3);
            }
            return l3;
        }







        #region Method


        public Boolean Save(UserViewModel Save, String Username, String ClientID, out string errorMsg2)
        {
            errorMsg2 = "";
            try
            {
                if (Save.UserType == null)
                {
                    Save.UserType = "1";
                }
                Save.Password = EncryptString(_key, Save.Password);
                if (!String.IsNullOrEmpty(Save.Id))
                {
                    //cek existing user
                    var Em = "";
                    if (Save.isAD)
                    {
                        Em = Save.EmailApproval;
                    }
                    else
                    {
                        Em = Save.Email;
                    }
                    var OldUer = Repo.GetUserByEmail(Em);
                    if (OldUer != null)
                    {
                        errorMsg2 = "User has been register!";
                        return false;
                    }
                    UserType dbType = userTypeRepo.GetByID(Save.UserType);
                    CreateUserJson jsonReq = new CreateUserJson();
                    jsonReq.IDClient = ConstantHelper.IDCLIENT;
                    jsonReq.Email = Em;
                    jsonReq.IDUser = Em;
                    jsonReq.Name = Em;

                    jsonReq.Description = "";
                    jsonReq.PositionName = "";
                    jsonReq.PositionID = "";
                    jsonReq.RolesCode = dbType != null ? dbType.RolesCode : "";
                    jsonReq.CompanyCode = Save.companyList;


                    string errorMsg = "";


                    User NewUser = new User();
                    NewUser.Email = Em;
                    NewUser.Password = Save.Password;
                    NewUser.UserType = Save.UserType;
                    NewUser.IDVendor = Save.IDVendor;
                    NewUser.IsAD = Save.isAD.ToString();


                    Repo.AddNewUser(NewUser);
                    Repo.Commit();

                    //var resRunStore = runStoreUser(Save);

                    if (Save.companyList != null)
                    {
                        var resRunComp = runCompanyUser(Save);
                        if (resRunComp)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                    errorMsg2 = "Failed to add user!";
                    return false;


                }
                else
                {
                    User NewUser = new User();
                    NewUser = Repo.GetUserByID(Save.Id);
                    if (Save.isAD)
                    {
                        NewUser.Email = Save.EmailApproval;
                    }
                    else
                    {
                        NewUser.Email = Save.Email;
                    }
                    //NewUser.Password = Save.Password;
                    NewUser.UserType = Save.UserType;
                    NewUser.IDVendor = Save.IDVendor;
                    NewUser.IsAD = Save.isAD.ToString();



                    //var resRunStore = runStoreUser(Save);
                    if (Save.companyList != null)
                    {
                        var resRunComp = runCompanyUser(Save);
                        if (resRunComp)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    errorMsg2 = "Failed to edit user!";
                    return false;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                errorMsg2 = "Failed to add/edit user!";
                return false;
            }
        }
        public bool runStoreUser(UserViewModel Save)
        {
            try
            {
                var EmailVer = "";
                if (Save.isAD)
                {
                    EmailVer = Save.EmailApproval;
                }
                else
                {
                    EmailVer = Save.Email;
                }
                var delRes = deleteUserStore(EmailVer);
                if (delRes)
                {
                    for (int i = 0; i < Save.storeUserList.Count; i++)
                    {
                        insertUserStore(EmailVer, Save.storeUserList[i]);
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public bool runCompanyUser(UserViewModel Save)
        {
            try
            {
                var EmailVer = "";
                if (Save.isAD)
                {
                    EmailVer = Save.EmailApproval;
                }
                else
                {
                    EmailVer = Save.Email;
                }
                var delRes = deleteUserCompany(EmailVer);
                if (delRes)
                {
                    for (int i = 0; i < Save.companyList.Count; i++)
                    {
                        insertUserCompany(EmailVer, Save.companyList[i]);
                    }
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool deleteUserCompany(string userID)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                List<SqlParameter> listparam = new List<SqlParameter>();
                listparam.Add(new SqlParameter("userID", userID));

                var result = DataTableHelper.ExecStoreProcedure(conString, "Sp_DeleteUserCompany", listparam, timeout_duration);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool deleteUserStore(string userID)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                List<SqlParameter> listparam = new List<SqlParameter>();
                listparam.Add(new SqlParameter("userID", userID));

                var result = DataTableHelper.ExecStoreProcedure(conString, "Sp_DeleteUserStore", listparam, timeout_duration);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool insertUserCompany(string userID, string compCode)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                List<SqlParameter> listparam = new List<SqlParameter>();
                listparam.Add(new SqlParameter("userID", userID));
                listparam.Add(new SqlParameter("compCode", compCode));

                var result = DataTableHelper.ExecStoreProcedure(conString, "Sp_InsertUserCompany", listparam, timeout_duration);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public bool insertUserStore(string userID, string compCode)
        {
            try
            {
                String timeout_duration = configFile.GetConnectionString("TimeoutDuration");

                List<SqlParameter> listparam = new List<SqlParameter>();
                listparam.Add(new SqlParameter("userID", userID));
                listparam.Add(new SqlParameter("idstore", compCode));

                var result = DataTableHelper.ExecStoreProcedure(conString, "Sp_InsertUserStore", listparam, timeout_duration);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<List<AD>> getADData(string parameter)
        {
            String URL = configFile.GetConnectionString("GetListAD");
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
                {
                    try
                    {
                        List<AD> adList = new List<AD>();
                        returnAD param = new returnAD();
                        param.Email = parameter;
                        //param.Email = 
                        request.Content = new StringContent(JsonConvert.SerializeObject(param));
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = await httpClient.SendAsync(request);
                        string result = await response.Content.ReadAsStringAsync();
                        var hasil = result.Replace("&quot;", @"""");
                        adList = JsonConvert.DeserializeObject<List<AD>>(hasil);
                        return adList;
                    }
                    catch (Exception ex)
                    {
                        //return null;
                        throw ex.InnerException;
                    }
                }
            }
        }








        public User GetUser(String ID)
        {
            try
            {
                return Repo.GetUserByID(ID);
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
        public User findByEmail(string email)
        {
            return Repo.GetUserByEmail(email);
        }

        public string GeneratePass()
        {
            Random random = new Random();
            string r = "";
            int i;
            for (i = 1; i < 11; i++)
            {
                r += random.Next(0, 9).ToString();
            }
            return r;
        }

        public Boolean resetPass(string Email, string name)
        {
            try
            {
                string newPass = GeneratePass();
                SendEmail objSendEmail = new SendEmail();
                objSendEmail.IDClient = "TMS";
                objSendEmail.Email = Email;
                objSendEmail.Password = newPass;
                objSendEmail.Name = name;
                objSendEmail.Username = Email;
                objSendEmail.IsRegistered = "2";
                sendEmail(objSendEmail);

                var res = Repo.GetUserByEmail(Email);
                res.Password = EncryptString(configFile.GetConnectionString("EncDecKey"), newPass);
                Repo.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string sendEmail(SendEmail data)
        {
            String URL = configFile.GetConnectionString("sendEmailVendor");
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL))
                {
                    try
                    {
                        string JsonData = JsonConvert.SerializeObject(data);
                        request.Content = new StringContent(JsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult();
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            return "ok";
                        }
                        else
                        {
                            return "error";
                        }

                    }
                    catch (Exception ex)
                    {
                        return "error";
                    }
                }
            }
        }

        public User Find(UserViewModel filter, DateTime? Dari, DateTime? Sampai)
        {
            try
            {
                filter.Password = EncryptString(_key, filter.Password);
 


                List<User> listUserPass = Repo.FindUser(filter);
                if (listUserPass != null && listUserPass.Count > 0)
                {
                    return listUserPass.FirstOrDefault();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public List<UserViewModel> FindHome(UserViewModel filter)
        {
            try
            {

                //var result = Repo.FindUser(filter);
                //return Mapper.Map<List<User>, List<UserViewModel>>(result);
                List<UserViewModel> listUser = new List<UserViewModel>();
                var dt = Repo.FindUserBySP();
                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        UserViewModel items = new UserViewModel();

                        items.Id = (row["ID"].ToString());
                        items.IDVendor = row["IDVendor"].ToString();
                        items.isAD = Convert.ToBoolean(row["isAD"].ToString());
                        items.Email = row["Email"].ToString();
                        items.Password = row["Password"].ToString();
                        items.UserType = row["UserType"].ToString();
                        items.UserTypeName = row["UserTypeName"].ToString();
                        items.VendorCode = row["VendorCode"].ToString();
                        items.VendorName = row["VendorName"].ToString();

                        listUser.Add(items);
                    }
                }
                return listUser;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }

        public async void cekMAPUser(jsonLogin filter)
        {
            String URL = configFile.GetConnectionString("GetADMAP");

            returnJson reJes = new returnJson();
            StringContent content = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync(URL, content))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                reJes = JsonConvert.DeserializeObject<returnJson>(apiResponse);
            }
        }

        public Boolean isLogin()
        {
            return true;
        }

        public Boolean changePass(ChangePass data, string email)
        {
            try
            {
                var res = Repo.GetUserByEmail(email);
                res.Password = EncryptString(_key, data.newPassword);
                //Repo.AddNewUser(res);
                Repo.Commit();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean Delete(string id)
        {
            try
            {
                User DelUser = Repo.GetUserByID(id);
                Repo.DeleteUser(DelUser);
                Repo.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //encrypt
        public string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        //decrypt
        public string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        #endregion


    }

}