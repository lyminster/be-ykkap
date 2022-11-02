using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class StockReceiveHeaderManager
	{
		public class Identity
		{
			public const string Name = "Stock Receive Header";

			public const string TableName = "FICHReceiveHeader";

			public const string RegType = "O";
		}

		public class MessageText
		{
		}

		protected class StoredProcedure
		{
			public const string DoubleTsI_FlagUpdate = "sp_DoubleTsI_FlagUpdate";

			public const string tempReceiveHeader_Insert = "sp_TempReceiveHeader_Insert";

			public const string ReceiveHeader_Insert = "sp_ReceiveHeader_Insert";

			public const string isReceiveHeader_Exsist = "sp_TransferHeader_Check";

			public const string MasterFile_Insert = "sp_MasterFile_Insert";

			public const string ReceiveHeader_Count = "sp_TransferHeader_Check";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public int CountReceiveHeader(StockReceiveHeaderData StockReceiveHeaderDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_TransferHeader_Check '" + StockReceiveHeaderDat.Warehouse + "' , '" + StockReceiveHeaderDat.RegType + "' , '" + StockReceiveHeaderDat.Operation + "' , '" + StockReceiveHeaderDat.OpsDate + "' , '" + StockReceiveHeaderDat.SenderWH + "'";
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

		public void InsertReceiveHeader(StockReceiveHeaderData StockReceiveHeaderDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_ReceiveHeader_Insert";
			sPCommand.ExecuteNonQuery();
		}

		public void InsertTempReceiveHeader(StockReceiveHeaderData StockReceiveHeaderDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_TempReceiveHeader_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", StockReceiveHeaderDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", StockReceiveHeaderDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Till", StockReceiveHeaderDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", StockReceiveHeaderDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", StockReceiveHeaderDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", StockReceiveHeaderDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@Model", StockReceiveHeaderDat.Model));
			sPCommand.Parameters.Add(new SqlParameter("@Color", StockReceiveHeaderDat.Color));
			sPCommand.Parameters.Add(new SqlParameter("@Size", StockReceiveHeaderDat.Size));
			sPCommand.Parameters.Add(new SqlParameter("@SenderWH", StockReceiveHeaderDat.SenderWH));
			sPCommand.Parameters.Add(new SqlParameter("@isTariffProcessed", StockReceiveHeaderDat.isTariffProcessed));
			sPCommand.Parameters.Add(new SqlParameter("@isExport", StockReceiveHeaderDat.isExport));
			sPCommand.Parameters.Add(new SqlParameter("@ReceiveQty", StockReceiveHeaderDat.ReceiveQty));
			sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", SESSION.FileName));
			sPCommand.ExecuteNonQuery();
		}

		public bool DoubleTsIFlagUpdate()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_DoubleTsI_FlagUpdate";
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
