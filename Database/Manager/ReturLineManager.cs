using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class ReturLineManager
	{
		public class Identity
		{
			public const string Name = "Retur Line";

			public const string TableName = "FICHReturLine";

			public const string RegType = "d";
		}

		protected class StoredProcedure
		{
			public const string ReturLine_Insert = "sp_ReturLine_Insert";

			public const string isReturLine_Exsist = "sp_ReturLine_Check";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void ReturLineInsert(ReturLineData ReturLineDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_ReturLine_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", ReturLineDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", ReturLineDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Till", ReturLineDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", ReturLineDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", ReturLineDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", ReturLineDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@OriWH", ReturLineDat.OriWH));
			sPCommand.Parameters.Add(new SqlParameter("@OriOpsDate", ReturLineDat.OriOpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OriOperation", ReturLineDat.OriOperation));
			sPCommand.Parameters.Add(new SqlParameter("@OriQty", ReturLineDat.OriQty));
			sPCommand.Parameters.Add(new SqlParameter("@OriClaveTC", ReturLineDat.OriClaveTC));
			sPCommand.Parameters.Add(new SqlParameter("@isDiscount", ReturLineDat.isDiscount));
			sPCommand.Parameters.Add(new SqlParameter("@isPromotion", ReturLineDat.isPromotion));
			sPCommand.Parameters.Add(new SqlParameter("@isEANScan", ReturLineDat.isEANScan));
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

		public bool isReturLineExsist(ReturLineData ReturLineDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_ReturLine_Check '" + ReturLineDat.Warehouse + "' , '" + ReturLineDat.RegType + "' , " + ReturLineDat.Till + "' , '" + ReturLineDat.Operation + "' , " + ReturLineDat.OpsDate + "' , " + ReturLineDat.OriOperation + "' , " + ReturLineDat.OriOpsDate + "' , " + ReturLineDat.OriWH + "'";
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
