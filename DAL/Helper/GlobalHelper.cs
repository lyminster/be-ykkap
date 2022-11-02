using DAL.DataAccessLayer.JOB;
using Database.ConfigClass;
using Database.InfrastructurClass;
using Database.Models;
using Database.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModel.ViewModels;

namespace DAL.Helper
{

    public static class ConstantVariable
    {


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
    (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
        public enum MODULNAME
        {
            GENERALCLAIM,
            ADVANCEREQUEST,
            ADVANCESETTLEMANT,
        }
        public enum PERMANENTDELETE
        {
            RolePriviledge,
        }
        public static int RowStatusDone = 4;

        public static string DocumentExpiredVendor = "DocumentExpiredVendor";
        public static string JobReminderApproval = "ReminderApproval";
        public static int Settle = 1;
        public static string JobCreatedBy = "JOBHR";

        public static string SubmitDocument = "SubmitDocumentExpenseDevelop";
        public static string UpdateDocument = "UpdateStatusDocumentExpenseDevelop";
        public static string StatusDefault = "Draft";
        public static string IDStatusFinish = "99.1";
        public static string IDStatusReject = "99.2";
        public static string IDStatusReject_BACK = ".2";
        public static string IDStatusRevise_BACK = ".3";

        public static string IDStatusReject1 = "2.2";
        public static string IDStatusReject2 = "3.2";
        public static string IDStatusReject3 = "4.2";
        public static string IDStatusReject4 = "5.2";
        public static string IDStatusReject5 = "6.2";
        public static string IDStatusRejectACChecker = "7.2";
        public static string IDStatusRejectCMChecker = "8.2";
        public static string IDStatusRejectCMApprover = "9.2";

        public static string IDHelperNoWorkflow = "NOWF";
        public static string DocumentTypeReplenish = "9";

        public static string Currency_SAP = "IDR";

        public static string Code_PettyCashPakai = "CLAIM01";
        public static string Code_GeneralClaim = "CLAIM02";
        public static string Code_CC = "CLAIM03";
        public static string Code_WirelessPersonal = "CLAIM04";
        public static string Code_WirelessCorporate = "CLAIM05";
        public static string Code_PettyReplenish = "CLAIM06";
        public static string Code_PettyRequestToko = "CLAIM07";
        public static string Code_AdvOperational = "ADVANCE01";
        public static string Code_AdvTravel = "ADVANCE03";
        public static string Code_SettOper = "SETTLE01";
        public static string Code_SettTravel = "SETTLE02";

        public static string Code_PettOffRequest = "CLAIM08";
        public static string Code_PettOffReimburse = "CLAIM10";
        public static string Code_PettOffReplen = "CLAIM11";

        public static string Code_ReturnPettCash = "RETURN01";
        public static string Code_ReturnPettCashOffice = "RETURN02";
        public static string Expense_CashRegis = "Store Cashier Register";
        public static string Expense_PettyCashReq = "Petty Cash Request";
        public static string Expense_PettyCashOffice = "Petty Cash Office";

        public static string BLART_KR = "KR";
        public static string BLART_SA = "SA";
        public static string BLART_Z1 = "Z1";

        //Petty Cash Office
        public static string CLAIM08 = "CLAIM08";
        //Petty Cash Office Reimbursement (Penggunaan)
        public static string CLAIM10 = "CLAIM10";
        //Petty Cash Office Replenishment (No Workflow)
        public static string CLAIM11 = "CLAIM11";
        public enum EditMode
        {
            INSERT,
            EDIT,
            DELETE,
            NEW
        }
        public enum EnumUserType
        {
            USER

        }

        public static string StatusOK = "Ok";
        public static string IDClient = "VEN";
        public static string XMLJOB = "XMLJOB";
        public static string TxtJOB = "TxtJOB";
        public static string SP_CheckFileLogStatus = "SP_CheckFileLogStatus";



        public static string Filter = "Filter";
        public static string UserType = "UserType";
        public static string UserTypeCC = "UserTypeCC";
        public static string UserTypeBCC = "UserTypeBCC";
        public static string FINISH = "FINISH";



    }
    public static class GlobalHelpers
    {
        public static string GenerateQueryIN(List<String> ListData)
        {
            try
            {
                String IDResult = "";
                if (ListData != null)
                {
                    foreach (var item in ListData.ToList())
                    {
                        IDResult += item + ",";

                    }

                    if (!string.IsNullOrEmpty(IDResult))
                    {
                        if (IDResult.Length > 1)
                        {
                            IDResult = IDResult.Substring(0, IDResult.Length - 1);
                        }
                    }
                }
                return IDResult;
            }
            catch
            {
                return " ";
            }
        }
        public static string CekIsValidEmail(string email, string EmailDefault)
        {
            try
            {

                if (String.IsNullOrWhiteSpace(email) && !GlobalHelpers.IsValidEmail(email))
                {
                    return EmailDefault;
                }
                else
                {
                    return email;
                }

            }
            catch
            {
                return EmailDefault;
            }
        }
        public static bool IsNonStringClass(Type type)
        {
            if (type == null || type == typeof(string))
                return false;
            if (type == null || type == typeof(DateTime))
                return false;
            if (type == null || type == typeof(DateTime?))
                return false;
            if (type == null || type == typeof(decimal))
                return false;
            if (type == null || type == typeof(decimal?))
                return false;
            if (type == null || type == typeof(long))
                return false;
            if (type == null || type == typeof(long?))
                return false;
            if (type == null || type == typeof(float))
                return false;
            if (type == null || type == typeof(float?))
                return false;
            if (type == null || type == typeof(double))
                return false;
            if (type == null || type == typeof(double?))
                return false;
            if (type == null || type == typeof(bool))
                return false;
            if (type == null || type == typeof(bool?))
                return false;
            if (type == null || type == typeof(int))
                return false;
            if (type == null || type == typeof(int?))
                return false;
            return typeof(Type).IsClass;
        }
        public static bool Insert<T>(List<T> datas, string table, String ConString)
        {
            bool result = false;
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            SqlConnection con = new SqlConnection(ConString);
            con.Open();

            try
            {
                foreach (var data in datas)
                {

                    values.Clear();
                    foreach (var item in data.GetType().GetProperties())
                    {
                        if (!IsNonStringClass(item.PropertyType))
                        {
                            String value = "";
                            object valueobc = item.GetValue(data, null);

                            if (valueobc != null && !string.IsNullOrEmpty(valueobc.ToString()))
                                value = valueobc.ToString();

                            values.Add(new KeyValuePair<string, string>(item.Name, value));
                        }
                    }

                    string xQry = getInsertCommand(table, values);
                    SqlCommand cmdi = new SqlCommand(xQry, con);
                    cmdi.ExecuteNonQuery();
                }
                result = true;
            }
            catch (Exception ex)
            { throw ex; }
            finally { con.Close(); }
            return result;
        }

        public static bool InsertFileLogHeader(FileLog datas, String ConString)
        {
            if (datas.Remarks == null)
            {
                datas.Remarks = "";
            }

            Console.Write(datas.Remarks);

            using (SqlConnection conn = new SqlConnection(ConString))
            {

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    StringBuilder QUERY = new StringBuilder();

                    QUERY.AppendLine("    INSERT INTO[dbo].[FileLog]             ");
                    QUERY.AppendLine("    ([ID]                                  ");
                    QUERY.AppendLine("    ,[TableName]                           ");
                    QUERY.AppendLine("    ,[FileName]                            ");
                    QUERY.AppendLine("    ,[Status]                              ");
                    QUERY.AppendLine("    ,[Remarks]                             ");
                    QUERY.AppendLine("    ,[CreatedBy]                           ");
                    QUERY.AppendLine("    ,[CreatedTime])                        ");
                    QUERY.AppendLine(" VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7)                            ");


                    cmd.CommandText = QUERY.ToString();

                    cmd.Parameters.AddWithValue("@param1", datas.ID);
                    cmd.Parameters.AddWithValue("@param2", datas.TableName);
                    cmd.Parameters.AddWithValue("@param3", datas.FileName);
                    cmd.Parameters.AddWithValue("@param4", datas.Status);
                    cmd.Parameters.AddWithValue("@param5", datas.Remarks);
                    cmd.Parameters.AddWithValue("@param6", datas.CreatedBy);
                    cmd.Parameters.AddWithValue("@param7", datas.CreatedTime);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        return false;
                    }

                }
            }

            return true;
        }

        public static bool UpdateLogHeader(FileLog datas, String ConString)
        {
            if (datas.Remarks == null)
            {
                datas.Remarks = "";
            }
            using (SqlConnection conn = new SqlConnection(ConString))
            {

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    StringBuilder QUERY = new StringBuilder();

                    QUERY.AppendLine("    Update [dbo].[FileLog]    set Status = '" + datas.Status + "',Remarks = '" + ConstantVariable.IDStatusFinish + "' where  ID = '" + datas.ID + "' ");


                    cmd.CommandText = QUERY.ToString();


                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        return false;
                    }

                }
            }

            return true;
        }



        public static bool UpdateLogHeaderRetry(FileLog datas, String ConString)
        {
            if (datas.Remarks == null)
            {
                datas.Remarks = "";
            }
            using (SqlConnection conn = new SqlConnection(ConString))
            {

                using (SqlCommand cmd = conn.CreateCommand())
                {

                    StringBuilder QUERY = new StringBuilder();

                    QUERY.AppendLine("    Update [dbo].[FileLog]    set Status = '" + datas.Status + "',Remarks = 'RETRY Today' where  ID = '" + datas.ID + "' ");


                    cmd.CommandText = QUERY.ToString();


                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        return false;
                    }

                }
            }

            return true;




        }
        public static bool InsertFileLogDetail(List<FileLogDetail> ListData, String ConString)
        {
            foreach (var datas in ListData)
            {
                using (SqlConnection conn = new SqlConnection(ConString))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {

                        StringBuilder QUERY = new StringBuilder();
                        QUERY.AppendLine("  INSERT INTO[dbo].[FileLogDetail]        ");
                        QUERY.AppendLine("  ([ID]                                   ");
                        QUERY.AppendLine("  ,[IDFileLog]                            ");
                        QUERY.AppendLine("  ,[OrderNo]                              ");
                        QUERY.AppendLine("  ,[Status]                               ");
                        QUERY.AppendLine("  ,[Remarks]                              ");
                        QUERY.AppendLine("  ,[CreatedBy]                            ");
                        QUERY.AppendLine("  ,[CreatedTime])                         ");
                        QUERY.AppendLine(" VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7)                            ");


                        cmd.CommandText = QUERY.ToString();

                        cmd.Parameters.AddWithValue("@param1", datas.ID);
                        cmd.Parameters.AddWithValue("@param2", datas.IDFileLog);
                        cmd.Parameters.AddWithValue("@param3", datas.OrderNo);
                        cmd.Parameters.AddWithValue("@param4", datas.Status);
                        cmd.Parameters.AddWithValue("@param5", datas.Remarks != null ? datas.Remarks : " ");
                        cmd.Parameters.AddWithValue("@param6", datas.CreatedBy);
                        cmd.Parameters.AddWithValue("@param7", datas.CreatedTime);

                        try
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();

                        }
                        catch (SqlException e)
                        {
                            return false;
                        }

                    }
                }
            }

            return true;
        }
        public static bool InsertFileLogDetail(FileLogDetail datas, String ConString, string namatable)
        {

            using (SqlConnection conn = new SqlConnection(ConString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    StringBuilder QUERY = new StringBuilder();
                    QUERY.AppendLine("  INSERT INTO[dbo].[FileLogDetail" + namatable + "]        ");
                    QUERY.AppendLine("  ([ID]                                   ");
                    QUERY.AppendLine("  ,[IDFileLog]                            ");
                    QUERY.AppendLine("  ,[OrderNo]                              ");
                    QUERY.AppendLine("  ,[Status]                               ");
                    QUERY.AppendLine("  ,[Remarks]                              ");
                    QUERY.AppendLine("  ,[SourceTxt]                            ");
                    QUERY.AppendLine("  ,[CreatedBy]                            ");
                    QUERY.AppendLine("  ,[CreatedTime]                         ");
                    QUERY.AppendLine("  ,[CodeData])                         ");
                    QUERY.AppendLine(" VALUES(@param1,@param2,@param3,@param4,@param5,@param6,@param7,@Param8, @param9)                            ");


                    cmd.CommandText = QUERY.ToString();

                    cmd.Parameters.AddWithValue("@param1", datas.ID);
                    cmd.Parameters.AddWithValue("@param2", datas.IDFileLog);
                    cmd.Parameters.AddWithValue("@param3", datas.OrderNo);
                    cmd.Parameters.AddWithValue("@param4", datas.Status);
                    cmd.Parameters.AddWithValue("@param5", datas.Remarks != null ? datas.Remarks : " ");
                    cmd.Parameters.AddWithValue("@param6", datas.SourceTxt);
                    cmd.Parameters.AddWithValue("@param7", datas.CreatedBy);
                    cmd.Parameters.AddWithValue("@param8", datas.CreatedTime);
                    cmd.Parameters.AddWithValue("@param9", datas.CodeData);
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {
                        Console.WriteLine("error at : " + e.Message);
                        return false;
                    }

                }
            }


            return true;
        }



        private static string getInsertCommand(string table, List<KeyValuePair<string, string>> values)
        {
            string query = null;
            query += "INSERT INTO " + table + " ( ";
            foreach (var item in values)
            {
                query += item.Key;
                query += ", ";
            }
            query = query.Remove(query.Length - 2, 2);
            query += ") VALUES ( ";
            foreach (var item in values)
            {
                if (item.Key.GetType().Name == "System.Int") // or any other numerics
                {
                    query += item.Value;
                }
                else if (item.Key.GetType().Name == "System.Datetime") // or any other numerics
                {
                    query += Convert.ToDateTime(item.Value).ToString("dd-MMM-yyyy HH:mm");
                }
                else
                {
                    query += "'";
                    query += item.Value;
                    query += "'";
                }
                query += ", ";
            }
            query = query.Remove(query.Length - 2, 2);
            query += ")";
            return query;
        }

        public static JobsLog PostJobsLog(String JobName, string SpecificName, String JobDescription, string IDTable, BusinessModelContext _dbContext, EmailService service, Boolean sentEmail = false)
        {
            JobsLog newLog = new JobsLog();
            newLog.CreatedTime = DateTime.Now;
            newLog.Name = JobName.ToLower() + "-" + SpecificName.ToLower();
            newLog.Description = JobDescription.ToLower();
            newLog.TableKey = IDTable;
            newLog.JobRunning = DateTime.Now;
            newLog.CreatedBy = "System";
            newLog.LastModifiedBy = "System";
            newLog.LastModifiedTime = DateTime.Now;
            newLog.SetRowStatus(RowStatus.Active);

            string constring = _dbContext.Database.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(constring))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    StringBuilder QUERY = new StringBuilder();



                    QUERY.AppendLine("     INSERT INTO[dbo].[JobsLog]          ");
                    QUERY.AppendLine("     ([Name]                          ");
                    QUERY.AppendLine("     ,[Description]            ");
                    QUERY.AppendLine("     ,[JobRunning]                       ");
                    QUERY.AppendLine("     ,[CreatedBy]                        ");
                    QUERY.AppendLine("     ,[CreatedTime]                      ");
                    QUERY.AppendLine("     ,[LastModifiedBy]                   ");
                    QUERY.AppendLine("     ,[LastModifiedTime]                 ");
                    QUERY.AppendLine("     ,[RowStatus]                       ");
                    QUERY.AppendLine("     ,[TableKey] )                         ");
                    QUERY.AppendLine("                                    ");


                    QUERY.AppendLine(" VALUES(@param2,@param3,@param4,@param5,@param6,@param7,@param8,@param9,@param10)                            ");


                    cmd.CommandText = QUERY.ToString();

                    cmd.Parameters.AddWithValue("@param2", newLog.Name);
                    cmd.Parameters.AddWithValue("@param3", newLog.Description);
                    cmd.Parameters.AddWithValue("@param4", newLog.JobRunning);
                    cmd.Parameters.AddWithValue("@param5", newLog.CreatedBy);
                    cmd.Parameters.AddWithValue("@param6", newLog.CreatedTime);
                    cmd.Parameters.AddWithValue("@param7", newLog.LastModifiedBy);
                    cmd.Parameters.AddWithValue("@param8", newLog.LastModifiedTime);
                    cmd.Parameters.AddWithValue("@param9", newLog.RowStatus);
                    cmd.Parameters.AddWithValue("@param10", newLog.TableKey);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                    catch (SqlException e)
                    {

                    }

                }
            }



            try
            {

                Console.WriteLine(JobName + "->" + JobDescription);
                if (sentEmail)
                {
                    service.SentMailToDefault(JobName, JobDescription);
                }

            }
            catch (Exception ex)
            {
                string IE = ex.InnerException != null ? ex.InnerException.Message : null;
                Console.WriteLine("error sent email:" + ex.Message + "/" + IE);
            }




            return (newLog);
        }

        public static JobsLog DeleteJobsLog(string id, BusinessModelContext _dbContext, bool permanent = false)
        {
            JobsLog JobsLog = _dbContext.JobsLogs.FirstOrDefault(x => x.ID == id);
            _dbContext.JobsLogs.Remove(JobsLog);
            _dbContext.SaveChanges();
            return (JobsLog);

        }
        public static void DeleteJobsLogByHour(BusinessModelContext _dbContext)
        {
            var lasthiur = DateTime.Now.AddDays(1);
            var lasthiur2 = DateTime.Now.AddDays(10);
            IEnumerable<JobsLog> LogDel = _dbContext.JobsLogs.Where(a => a.CreatedTime < lasthiur);
            _dbContext.JobsLogs.RemoveRange(LogDel);
            _dbContext.SaveChanges();


        }




        public static bool IsDate(string tempDate, Boolean test)
        {
            DateTime fromDateValue;
            var formats = new[] { "dd/MM/yyyy", "yyyyMMdd", "yyyy-MM-dd", "dd-MMM-yyyy", "dd-MMM-yyyy HH:mm" };
            if (DateTime.TryParseExact(tempDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsTime(string tempDate, Boolean test)
        {
            DateTime fromDateValue;
            var formats = new[] { "hh:mm", "HH:mm", "hhmmss", "HHmmss" };
            if (DateTime.TryParseExact(tempDate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out fromDateValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static bool IsValidEmail(string email)
        {
            Regex checkRegex =
                new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            return checkRegex.IsMatch(email);
        }

        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if (Expression == null || Expression is DateTime)
                return false;

            if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try
            {
                if (Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                return true;
            }
            catch { } // just dismiss errors but return false
            return false;
        }
        public static ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }
        public static Boolean ContainColumn(string columnName, DataTable table)
        {
            Boolean Result = false;
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                Result = true;
            }

            return Result;
        }

        public static DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        ? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        : cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static HelperTable GetHelperTable(String TableID, String TableCode, string IDKlien, BusinessModelContext BM)
        {

            HelperTable data = BM.HelperTables.FirstOrDefault(x => x.ID == TableID && x.Code == TableCode);
            if (data == null)
            {
                data = new HelperTable();
            }

            return data;
        }


        public enum EnumClaims
        {
            ID
        , IDRole
                , RolesCode
        , Nama
        , NIK
        , IDVendor
        , Username
        , IsAD
        , Password
        , CreatedBy
        , CreatedDate
        , ModifiedBy
        , UserType
        , Email
        , IDClient
        , IDCostCenter
        , IDLokasi
        , IDLevel
        , RowStatus
        , IDEmployee
        , IDUserGroup
        , IDPositionEmployee
        , IDCompany
        , CompanyName
        , LevelName
        }
        public enum MethodEnum
        {
            POST,
            GET

        }
        public enum ActionEnum
        {
            Update,
            Delete,
            View,
            ViewDetail,
            Download,
            Add
        }

        public static HttpStatusCode SentRequest(String JsonData, String URL, String Method, Boolean isAuthorize, String Token, out string returndata)
        {
            returndata = "";

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod(Method), URL))
                {
                    try
                    {
                        if (isAuthorize)
                        {
                            request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + Token);

                        }


                        request.Content = new StringContent(JsonData);
                        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                        var response = httpClient.SendAsync(request).ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        string result = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult(); ;
                        returndata = result;
                        return response.StatusCode;



                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                }



            }
        }

        public enum TableName
        {
            StoreMappingUser,
            DocumentType,
            BudgetType,
            BudgetLimitCost,
            Transportation,
            RolePriviledge,
            Currency,
            Vendor,
            UserRole,
            GLAccount,
            User,
            DocumentTypeClaim,
            DocumentSetting,
            DocumentWorkflow,
            DocumentHeaderExtend,
            DocumentSubType,
            ActivityDefinitions,
            ActivityInstances,
            BlockingActivities,
            DocumentDetail,
            ConnectionDefinitions,
            DocumentHeader,
            WorkflowDefinitionVersions,
            WorkflowInstances,
            DocumentSettingDetail,
            BankAccount,
            Employee,
            AttachmentTypeDocument,
            GradeEmployee,
            LevelEmployee,
            City,
            Company,
            CostCenter,
            Country,
            Department,
            DocumentAttachment,
            DocumentAttachmentAccess,
            PositionEmployee,
            DocumentHistory,
            DocumentWorkflowDetail,
            EmailJob,
            AccomodationCost,
            ExpenseGroup,
            ExpenseType,
            PerDiemCost,
            StatusDocument,
            Store,
            DocumentSettingConnectedList,
            SubBusinessUnit,
            DocumentSettingConnected
        }
        //public static HSSFWorkbook GenerateExcelFileToByte(DataTable DT)
        //{
        //    // Below code is create datatable and add one row into datatable.  

        //    // Declare HSSFWorkbook object for create sheet  
        //    var workbook = new HSSFWorkbook();
        //    HSSFWorkbook worksheet = workbook.Worksheets.Add("Workflow Matrix");

        //    int row = 1;


        //    //Below loop is create header  
        //    for (int i = 0; i < DT.Columns.Count; i++)
        //    {
        //        worksheet.Cell(row, i + 1).Value = DT.Columns[i].ColumnName.ToUpper();

        //    }

        //    //Below loop is fill content  
        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        row++;


        //        for (int j = 0; j < DT.Columns.Count; j++)
        //        {


        //            var Data = DT.Rows[i][j];
        //            DateTime temp;
        //            if (DateTime.TryParse(Data.ToString(), out temp))
        //            {

        //                worksheet.Cell(row, j + 1).Value = Convert.ToDateTime(Data.ToString()).ToString("dd-MMM-yyyy");
        //            }
        //            else
        //            {


        //                worksheet.Cell(row, j + 1).Value = Data.ToString();
        //            }


        //        }
        //    }




        //    return workbook;
        //}
        public static string ActionChecking(TableName tableName, ActionEnum actionEnum, string ID)
        {
            string Err = "";

            try
            {
                if (tableName == TableName.AccomodationCost)
                {

                }
                Err = "data cannot update its using by other data";
                return Err;
            }
            catch (Exception)
            {
                return Err;
            }


        }

        public static String ReGenerateThreadClaim(ClaimsPrincipal User)
        {
            String Result = "";
            try
            {
                System.Threading.Thread.CurrentPrincipal = User;
            }
            catch (Exception)
            {

                throw;
            }

            return Result;
        }
        public static String GetClaimValueByType(String Type, ClaimsPrincipal User)
        {
            String Result = "";
            try
            {
                List<Claim> ListClaim = User.Claims.ToList();
                if (ListClaim != null)
                {
                    Claim Claim = ListClaim.FirstOrDefault(x => x.Type == Type);
                    if (Claim != null)
                    {
                        Result = Claim.Value;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Result;
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static String GetClaimIdentityByTipe(String Type, ClaimsPrincipal User)
        {
            String Result = "";
            try
            {
                List<Claim> ListClaim = User.Claims.ToList();
                if (ListClaim != null)
                {
                    Claim Claim = ListClaim.FirstOrDefault(x => x.Type == Type);
                    if (Claim != null)
                    {
                        Result = Claim.Value;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return Result;
        }
        public static bool IsValidTime(string thetime)
        {
            Regex checktime =
                new Regex(@"^(20|21|22|23|[01]d|d)(([:][0-5]d){1,2})$");

            return checktime.IsMatch(thetime);
        }



        public static string CopyFile(IFormFile fileInput, IHostingEnvironment _hostenv)
        {

            var fileName2 = System.IO.Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 7) + "-" + fileInput.FileName);
            // Create new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(Path.Combine(_hostenv.WebRootPath, "Upload/") + fileName2))
            using (var uploadedFile = fileInput.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return fileName2;
        }

        public static string CopyImportantFile(IFormFile fileInput, IHostingEnvironment _hostenv, string path)
        {

            var fileName2 = System.IO.Path.GetFileName(Guid.NewGuid().ToString().Substring(0, 7) + "-" + fileInput.FileName);
            // Create new local file and copy contents of uploaded file
            //using (var localFile = System.IO.File.OpenWrite(Path.Combine(_hostenv.WebRootPath, path) + fileName2))
            using (var localFile = System.IO.File.OpenWrite(path + fileName2))
            using (var uploadedFile = fileInput.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            return fileName2;
        }


        public enum ClaimIdentity
        {
            Username,
            ClientID,
            ID,
            Nama,


        }


        public static string GetEmailFromIdentity(ClaimsPrincipal User)
        {
            String result = null;
            try
            {
                var resultx = User.Claims.ToList().FirstOrDefault(x => x.Type == EnumClaims.Email.ToString());
                result = resultx != null ? resultx.Value : null;

                return result;
            }
            catch (Exception)
            {

                return null;
            }

        }




        public static async Task<bool> GetAPIKeyValidationAndGenerateCookiesAsync(String Username, BusinessModelContext DBContext, HttpContext HttpContext)
        {
            try
            {
                string encryptionKey = "EDS";
                string dStr = Encryption.passwordDecrypt(Username, encryptionKey);
                var Userx = DBContext.Users.FirstOrDefault(x => x.Email == dStr);
                if (Userx != null)
                {
                    //di clear dl logout dl
                  await HttpContext.SignOutAsync(
   CookieAuthenticationDefaults.AuthenticationScheme);





                    // generate lah tokennya di sini
                    String IDVendor = "";

                    if (!String.IsNullOrEmpty(Userx.IDVendor))
                    {
                        IDVendor = Userx.IDVendor;
                    }

                    var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Userx.Email),
            new Claim(EnumClaims.Username.ToString(), Userx.Email),
            new Claim(EnumClaims.IsAD.ToString(), Userx.IsAD),
            new Claim(EnumClaims.UserType.ToString(), Userx.UserType),
            new Claim(EnumClaims.Email.ToString(), Userx.Email),
            new Claim(EnumClaims.IDClient.ToString(), Userx.Idclient),
            new Claim(EnumClaims.IDVendor.ToString(), IDVendor),
        };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        IssuedUtc = DateTime.Now,
                        // The time at which the authentication ticket was issued.

                        RedirectUri = "/Home/"
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                      CookieAuthenticationDefaults.AuthenticationScheme,
                      new ClaimsPrincipal(claimsIdentity),
                      authProperties);
                    System.Threading.Thread.CurrentPrincipal = new ClaimsPrincipal(claimsIdentity);


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


        public static string GetErrorMessage(Exception ex)
        {
            String Message = ex.InnerException != null ? ex.InnerException.Message : null;
            if (ex.InnerException != null && ex.InnerException.InnerException != null)
            {
                Message += ex.InnerException.InnerException.Message;
            }

            return "Error At : \n" + ex.Message + "\n" + Message;

        }


        public static string GetErrorMessageModelState(ModelStateDictionary Model)
        {

            String Message = "";
            //var message = string.Join(" | ", Model.Values
            //    .SelectMany(v => v.Errors)
            //    .Select(e => e.ErrorMessage));


            foreach (var item in Model.Keys.ToList())
            {
                Message += "|" + item.Replace("jsonData.", "") + "*";
            }
            return Message;

        }

    }

}
