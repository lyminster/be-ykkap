using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
    public class FICHFileDataManager
    {
        public class Identity
        {
            public const string TABLE_Version = "Version";

            public const string xmlRoot = "ITX_CLOSE_EXPORT";
        }

        protected class StoredProcedure
        {
            public const string isFICHFile_Exist = "sp_isFICHFile_Exist";

            public const string MasterFile_Insert = "sp_MasterFile_Insert";

            public const string FICHPLU_Update = "sp_FICHPLU_Update";
        }

        private Database.DBCommand dbCom = null;

        public void InsertFile2History(string fileName)
        {
            SqlCommand sPCommand = dbCom.GetSPCommand();
            sPCommand.CommandText = "sp_MasterFile_Insert";
            sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", fileName));
            sPCommand.ExecuteNonQuery();
        }

        public bool IsFICHFileExist(string FileName)
        {
            SqlDataReader sqlDataReader = null;
            bool result = false;
            try
            {
                SqlCommand textCommand = dbCom.GetTextCommand();
                textCommand.CommandText = "EXEC sp_isFICHFile_Exist '" + FileName + "'";
                sqlDataReader = textCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    result = true;
                }
                sqlDataReader?.Close();
            }
            finally
            {
                sqlDataReader?.Close();
            }
            return result;
        }

        public bool UpdateFICHPLU()
        {
            SqlDataReader sqlDataReader = null;
            bool result = false;
            int num = 0;
            try
            {
                SqlCommand textCommand = dbCom.GetTextCommand();
                textCommand.CommandText = "EXEC sp_FICHPLU_Update";
                if (textCommand.ExecuteNonQuery() != 0)
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public void AssignTransaction(SqlTransaction trx)
        {
            dbCom = new Database.DBCommand();
            dbCom.GetTransaction(trx);
        }

        public void AssignConnection(SqlConnection con)
        {
            dbCom = new Database.DBCommand();
            dbCom.GetConnection(con);
        }

        public bool isUpdateVersion(Version VersionDat)
        {
            SqlDataReader sqlDataReader = null;
            bool result = false;
            try
            {
                SqlCommand textCommand = dbCom.GetTextCommand();
                textCommand.CommandText = "Select * from Version where Major = '" + VersionDat.Major + "' and Minor = '" + VersionDat.Minor + "' and Revision = '" + VersionDat.Revision + "'";
                sqlDataReader = textCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    result = true;
                }
                sqlDataReader?.Close();
            }
            finally
            {
                sqlDataReader?.Close();
            }
            return result;
        }
    }
}