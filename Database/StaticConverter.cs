 
using FastMember;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public static class StaticConverter
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
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }





        public enum StatusDO
        {
            LOAD,
            DROP,
            POD,
            ClaimBelumBayar,
            ClaimSudahBayar,
            PengajuanSubcon,
            ApproveSubcon,
            [Display(Name = " ")]
            PENDING,


        }

        public static string SP_GETLISTDO = "SPT_GetViewDO";


        

    }

}
