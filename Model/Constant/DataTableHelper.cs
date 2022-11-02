using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ViewModel.Constant
{
    public static class DataTableHelper
    {
        public static T ToObject<T>(this DataRow row) where T : class, new()
        {
            T obj = new T();

            foreach (var prop in obj.GetType().GetProperties())
            {
                try
                {
                    if (prop.PropertyType.IsGenericType && prop.PropertyType.Name.Contains("Nullable"))
                    {
                        if (!string.IsNullOrEmpty(row[prop.Name].ToString()))
                            prop.SetValue(obj, Convert.ChangeType(row[prop.Name],
                            Nullable.GetUnderlyingType(prop.PropertyType), null));
                        //else do nothing
                    }
                    else
                        prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType), null);
                }
                catch
                {
                    continue;
                }
            }
            return obj;
        }
        /// <summary>
        /// Converts a DataTable to a list with generic objects
        /// </summary>
        /// <typeparam name="T">Generic object</typeparam>
        /// <param name="table">DataTable</param>
        /// <returns>List with generic objects</returns>
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    var obj = row.ToObject<T>();

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }


        public static DataTable DataReaderMapToList<T>(string storedProcedureorCommandText, String ConnectionString, List<SqlParameter> ListParam, bool isStoredProcedure = true)
        {
            var dataTable = new DataTable();
            SqlDataReader dr;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand())
                {
                    command.CommandTimeout = 1000000;
                    command.Connection = connection;
                    command.CommandType = CommandType.StoredProcedure;
                    if (!isStoredProcedure)
                    {
                        command.CommandType = CommandType.Text;
                    }
                    else
                    {
                        if (ListParam != null)
                        {
                            foreach (var item in ListParam)
                            {
                                command.Parameters.Add(item);
                            }
                        }
                    }

                    command.CommandText = storedProcedureorCommandText;
                    connection.Open();

                    dr = command.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dataTable.Load(dr);
                        return dataTable;
                    }
                    dr.Close();
                }
            }
            return dataTable;
        }


        public static DataTable ExecStoreProcedure(String ConnString, String SPName, List<SqlParameter> ListParam, String timeout_duration)
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
                        cmd.CommandTimeout = Convert.ToInt32(timeout_duration);
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