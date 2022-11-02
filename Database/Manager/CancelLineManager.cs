using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class CancelLineManager
	{
		public class Identity
		{
			public const string RegType = "N";

			public const string Name = "Cancel Line";

			public const string TableName = "FICHCancelLine";
		}

		protected class StoredProcedure
		{
			public const string CancelLine_Insert = "sp_CancelLine_Insert";

			public const string isCancelLine_Exsist = "sp_CancelLine_Check";

			public const string CancelLine_Count = "sp_CancelLine_Check";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void CancelLineInsert(CancelLineData CancelLineDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_CancelLine_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", CancelLineDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", CancelLineDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Till", CancelLineDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", CancelLineDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", CancelLineDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", CancelLineDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@Line", CancelLineDat.Line));
			sPCommand.Parameters.Add(new SqlParameter("@Model", CancelLineDat.Model));
			sPCommand.Parameters.Add(new SqlParameter("@Color", CancelLineDat.Color));
			sPCommand.Parameters.Add(new SqlParameter("@Size", CancelLineDat.Size));
			sPCommand.Parameters.Add(new SqlParameter("@SalesPrice", CancelLineDat.SalesPrice));
			sPCommand.Parameters.Add(new SqlParameter("@Discount", CancelLineDat.Discount));
			sPCommand.Parameters.Add(new SqlParameter("@Sign", CancelLineDat.Sign));
			sPCommand.Parameters.Add(new SqlParameter("@Quantity", CancelLineDat.Quantity));
			sPCommand.Parameters.Add(new SqlParameter("@SalesAmount", CancelLineDat.SalesAmount));
			sPCommand.Parameters.Add(new SqlParameter("@ModelTariff", CancelLineDat.ModelTariff));
			sPCommand.Parameters.Add(new SqlParameter("@SalesPerson", CancelLineDat.SalesPerson));
			sPCommand.Parameters.Add(new SqlParameter("@ReturnType", CancelLineDat.ReturnType));
			sPCommand.Parameters.Add(new SqlParameter("@CustomerID", CancelLineDat.CustomerID));
			sPCommand.Parameters.Add(new SqlParameter("@OpsType", CancelLineDat.OpsType));
			sPCommand.Parameters.Add(new SqlParameter("@Cancellation", CancelLineDat.Cancellation));
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

		public bool isCancelLineExsist(CancelLineData CancelLineDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_CancelLine_Check '" + CancelLineDat.Warehouse + "' , '" + CancelLineDat.Till + "' , '" + CancelLineDat.RegType + "' , " + CancelLineDat.Operation + "' , " + CancelLineDat.OpsDate + "' , " + CancelLineDat.OpsTime + "'";
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

		public int CancelLineCount(CancelLineData CancelLineDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_CancelLine_Check '" + CancelLineDat.Warehouse + "' , '" + CancelLineDat.Till + "' , '" + CancelLineDat.RegType + "' , '" + CancelLineDat.Operation + "' , '" + CancelLineDat.OpsDate + "' , '" + CancelLineDat.OpsTime + "'";
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
	}

}
