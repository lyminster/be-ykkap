using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class SalesLineManager
	{
		public class Identity
		{
			public const string RegType = "L";

			public const string Name = "Sales Line";

			public const string TableName = "FICHSalesLine";
		}

		protected class StoredProcedure
		{
			public const string SalesLine_Insert = "sp_SalesLine_Insert";

			public const string isSalesLine_Exsist = "sp_SalesLine_Check";

			public const string SalesLine_Count = "sp_SalesLine_Check";

			public const string FICHPLU_Update = "sp_FICHPLU_Update";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void SalesLineInsert(SalesLineData SalesLineDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_SalesLine_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", SalesLineDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", SalesLineDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Till", SalesLineDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", SalesLineDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", SalesLineDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", SalesLineDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@Line", SalesLineDat.Line));
			sPCommand.Parameters.Add(new SqlParameter("@Model", SalesLineDat.Model));
			sPCommand.Parameters.Add(new SqlParameter("@Color", SalesLineDat.Color));
			sPCommand.Parameters.Add(new SqlParameter("@Size", SalesLineDat.Size));
			sPCommand.Parameters.Add(new SqlParameter("@SalesPrice", SalesLineDat.SalesPrice));
			sPCommand.Parameters.Add(new SqlParameter("@Discount", SalesLineDat.Discount));
			sPCommand.Parameters.Add(new SqlParameter("@Sign", SalesLineDat.Sign));
			sPCommand.Parameters.Add(new SqlParameter("@Quantity", SalesLineDat.Quantity));
			sPCommand.Parameters.Add(new SqlParameter("@SalesAmount", SalesLineDat.SalesAmount));
			sPCommand.Parameters.Add(new SqlParameter("@ModelTariff", SalesLineDat.ModelTariff));
			sPCommand.Parameters.Add(new SqlParameter("@SalesPerson", SalesLineDat.SalesPerson));
			sPCommand.Parameters.Add(new SqlParameter("@ReturnType", SalesLineDat.ReturnType));
			sPCommand.Parameters.Add(new SqlParameter("@OnlineOrderID", SalesLineDat.OnlineOrderID));
			sPCommand.Parameters.Add(new SqlParameter("@OpsType", SalesLineDat.OpsType));
			sPCommand.Parameters.Add(new SqlParameter("@Cancellation", SalesLineDat.Cancellation));
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

		public bool isSalesLineExsist(SalesLineData SalesLineDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_SalesLine_Check '" + SalesLineDat.Warehouse + "' , '" + SalesLineDat.Till + "' , '" + SalesLineDat.RegType + "' , " + SalesLineDat.Operation + "' , " + SalesLineDat.OpsDate + "' , " + SalesLineDat.OnlineOrderID + "'";
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

		public int SalesLineCount(SalesLineData SalesLineDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_SalesLine_Check '" + SalesLineDat.Warehouse + "' , '" + SalesLineDat.Till + "' , '" + SalesLineDat.RegType + "' , '" + SalesLineDat.Operation + "' , '" + SalesLineDat.OpsDate + "' , '" + SalesLineDat.OnlineOrderID + "'";
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
