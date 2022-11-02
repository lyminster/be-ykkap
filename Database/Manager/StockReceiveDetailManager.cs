using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class StockReceiveDetailManager
	{
		public class Identity
		{
			public const string RegType = "O";

			public const string Name = "Stock Receive Detail";

			public const string TableName = "FICHReceiveDetail";
		}

		protected class StoredProcedure
		{
			public const string ReceiveDetail_Insert = "sp_ReceiveDetail_Insert";

			public const string isReceiveDetail_Exsist = "sp_ReceiveDetail_Check";

			public const string ReceiveDetail_Count = "sp_ReceiveDetail_Check";

			public const string FICHPLU_Update = "sp_FICHPLU_Update";

			public const string GetHistoryMovDetail = "sp_GetHistoryMovDetail";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public int ReceiveDetailCount(StockReceiveDetailData StockReceiveDetailDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_ReceiveDetail_Check '" + StockReceiveDetailDat.Warehouse + "' , '" + StockReceiveDetailDat.RegType + "' , '" + StockReceiveDetailDat.Operation + "' , '" + StockReceiveDetailDat.OpsDate + "' , '" + StockReceiveDetailDat.SenderWH + "'";
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

		public void ReceiveDetailInsert(StockReceiveDetailData StockReceiveDetailDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_ReceiveDetail_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", StockReceiveDetailDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", StockReceiveDetailDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", StockReceiveDetailDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", StockReceiveDetailDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@SenderWH", StockReceiveDetailDat.SenderWH));
			sPCommand.Parameters.Add(new SqlParameter("@Line", StockReceiveDetailDat.Line));
			sPCommand.Parameters.Add(new SqlParameter("@Model", StockReceiveDetailDat.Model));
			sPCommand.Parameters.Add(new SqlParameter("@Color", StockReceiveDetailDat.Color));
			sPCommand.Parameters.Add(new SqlParameter("@Size", StockReceiveDetailDat.Size));
			sPCommand.Parameters.Add(new SqlParameter("@SendQty", StockReceiveDetailDat.SendQty));
			sPCommand.Parameters.Add(new SqlParameter("@Sign", StockReceiveDetailDat.Sign));
			sPCommand.Parameters.Add(new SqlParameter("@ReceiveQty", StockReceiveDetailDat.ReceiveQty));
			sPCommand.Parameters.Add(new SqlParameter("@DelNoteStatus", StockReceiveDetailDat.DelNoteStatus));
			sPCommand.Parameters.Add(new SqlParameter("@Operador", StockReceiveDetailDat.Operador));
			sPCommand.Parameters.Add(new SqlParameter("@BoxCondition", StockReceiveDetailDat.BoxCondition));
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
