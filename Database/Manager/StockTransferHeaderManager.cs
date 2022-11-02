using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

    public class StockTransferHeaderManager
    {
        public class Identity
        {
            public const string Name = "Stock Transfer Header";

            public const string TableName = "FICHTransferHeader";

            public const string RegType = "F";
        }

        public class MessageText
        {
        }

        protected class StoredProcedure
        {
            public const string TransferHeader_Insert = "sp_TransferHeader_Insert";

            public const string isTransferHeader_Exsist = "sp_TransferHeader_Check";
        }

        private Database.ConfigClass.Database.DBCommand dbCom = null;

        public void TransferHeaderInsert(StockTransferHeaderData StockTransferHeaderDat)
        {
            SqlCommand sPCommand = dbCom.GetSPCommand();
            sPCommand.CommandText = "sp_TransferHeader_Insert";
            sPCommand.Parameters.Add(new SqlParameter("@Warehouse", StockTransferHeaderDat.Warehouse));
            sPCommand.Parameters.Add(new SqlParameter("@RegType", StockTransferHeaderDat.RegType));
            sPCommand.Parameters.Add(new SqlParameter("@Till", StockTransferHeaderDat.Till));
            sPCommand.Parameters.Add(new SqlParameter("@Operation", StockTransferHeaderDat.Operation));
            sPCommand.Parameters.Add(new SqlParameter("@OpsDate", StockTransferHeaderDat.OpsDate));
            sPCommand.Parameters.Add(new SqlParameter("@OpsTime", StockTransferHeaderDat.OpsTime));
            sPCommand.Parameters.Add(new SqlParameter("@ReceiverWH", StockTransferHeaderDat.ReceiverWH));
            sPCommand.Parameters.Add(new SqlParameter("@Sign", StockTransferHeaderDat.Sign));
            sPCommand.Parameters.Add(new SqlParameter("@TotalQty", StockTransferHeaderDat.TotalQty));
            sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", SESSION.FileName));
            sPCommand.ExecuteNonQuery();
        }

        public void AssignTransaction(SqlTransaction trx)
        {
            dbCom = new Database.ConfigClass.Database.DBCommand();
            dbCom.GetTransaction(trx);
        }

        public void AssignConnection(SqlConnection con)
        {
            dbCom = new Database.ConfigClass.Database.DBCommand();
            dbCom.GetConnection(con);
        }
    }
}