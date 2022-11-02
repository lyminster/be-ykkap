using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class StockTransferDetailManager
	{
		public class Identity
		{
			public const string RegType = "R";

			public const string Name = "Stock Transfer Detail";

			public const string TableName = "FICHTransferDetail";
		}

		protected class StoredProcedure
		{
			public const string TransferDetail_Insert = "sp_TransferDetail_Insert";

			public const string isTransferDetail_Exsist = "sp_TransferDetail_Check";

			public const string TransferDetail_Count = "sp_TransferDetail_Check";

			public const string FICHPLU_Update = "sp_FICHPLU_Update";

			public const string GetHistoryMovDetail = "sp_GetHistoryMovDetail";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public int TransferDetailCount(StockTransferDetailData StockTransferDetailDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_TransferDetail_Check '" + StockTransferDetailDat.Warehouse + "' , '" + StockTransferDetailDat.RegType + "' , '" + StockTransferDetailDat.Operation + "' , '" + StockTransferDetailDat.OpsDate + "'";
				sqlDataReader = textCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					num = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("jmlh"));
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return num++;
		}

		public void TransferDetailInsert(StockTransferDetailData StockTransferDetailDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_TransferDetail_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", StockTransferDetailDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", StockTransferDetailDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", StockTransferDetailDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", StockTransferDetailDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@ReceiverWH", StockTransferDetailDat.ReceiverWH));
			sPCommand.Parameters.Add(new SqlParameter("@Line", StockTransferDetailDat.Line));
			sPCommand.Parameters.Add(new SqlParameter("@Model", StockTransferDetailDat.Model));
			sPCommand.Parameters.Add(new SqlParameter("@Color", StockTransferDetailDat.Color));
			sPCommand.Parameters.Add(new SqlParameter("@Size", StockTransferDetailDat.Size));
			sPCommand.Parameters.Add(new SqlParameter("@SalesPrice", StockTransferDetailDat.SalesPrice));
			sPCommand.Parameters.Add(new SqlParameter("@Sign", StockTransferDetailDat.Sign));
			sPCommand.Parameters.Add(new SqlParameter("@Quantity", StockTransferDetailDat.Quantity));
			sPCommand.Parameters.Add(new SqlParameter("@ModelTariff", StockTransferDetailDat.ModelTariff));
			sPCommand.Parameters.Add(new SqlParameter("@OpsType", StockTransferDetailDat.OpsType));
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
