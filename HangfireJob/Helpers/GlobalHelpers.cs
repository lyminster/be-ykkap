using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HangfireJob.Helpers
{
    public static class GlobalHelpers
    {
        public static string SerializeObject<T>(T dataObject)
        {
            if (dataObject == null)
            {
                return string.Empty;
            }
            try
            {
                using (StringWriter stringWriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(stringWriter, dataObject);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static T DeserializeObject<T>(string xml)
             where T : new()
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new T();
            }
            try
            {
                using (var stringReader = new StringReader(xml))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    return (T)serializer.Deserialize(stringReader);
                }
            }
            catch (Exception ex)
            {
                return new T();
            }
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                if (!String.IsNullOrEmpty(email))
                {
                    email = email.ToLower();
                }
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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

        public static DataTable ExecStoreProcedure(String ConnString, String SPName, List<SqlParameter> ListParam)
        {
            try
            {
                string constring = ConnString;
                DataTable dt = new DataTable();
                SqlDataReader reader = null;

                using (SqlConnection conn = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(SPName, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (ListParam != null)
                        {
                            foreach (var item in ListParam.ToList())
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        conn.Open();
                        reader = cmd.ExecuteReader();



                        dt.Load(reader);

                        reader.Close();
                        conn.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        public static DataTable ExecStoreProcedureNew(String ConnString, String SPName, List<SqlParameter> ListParam)
        {
            try
            {
                string constring = ConnString;
                DataTable dt = new DataTable();
                SqlDataReader reader = null;

                using (SqlConnection conn = new SqlConnection(constring))
                {
                    using (SqlCommand cmd = new SqlCommand(SPName, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        if (ListParam != null)
                        {
                            foreach (var item in ListParam.ToList())
                            {
                                cmd.Parameters.Add(item);
                            }
                        }
                        conn.Open();
                        reader = cmd.ExecuteReader();



                        dt.Load(reader);

                        reader.Close();
                        conn.Close();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

    }
}
