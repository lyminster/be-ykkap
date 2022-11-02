using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class TotalSalesManager
	{
		public class Identity
		{
			public const string Name = "FICH SalesHeader";

			public const string TableName = "FICHSalesHeader";

			public const string RegType = "T";
		}

		protected class StoredProcedure
		{
			public const string FICHSalesHeader_Insert = "sp_FICHSalesHeader_Insert";

			public const string isFICHSalesHeader_Exsist = "sp_FICHSalesHeader_Check";

			public const string FICHSalesHeader_OrderID_Update = "sp_FICHSalesHeader_OrderID_Update";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void TotalSalesInsert(TotalSalesData TotalSalesDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_FICHSalesHeader_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", TotalSalesDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@Till", TotalSalesDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", TotalSalesDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", TotalSalesDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", TotalSalesDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", TotalSalesDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@PrintedSummary", TotalSalesDat.PrintedSummary));
			sPCommand.Parameters.Add(new SqlParameter("@Updated", TotalSalesDat.Updated));
			sPCommand.Parameters.Add(new SqlParameter("@StaffCode", TotalSalesDat.StaffCode));
			sPCommand.Parameters.Add(new SqlParameter("@ReturnType", TotalSalesDat.ReturnType));
			sPCommand.Parameters.Add(new SqlParameter("@Status", TotalSalesDat.Status));
			sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", SESSION.FileName));
			sPCommand.ExecuteNonQuery();
		}

		public void FICHSalesHeaderOrderIDUpdate(TotalSalesData TotalSalesDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_FICHSalesHeader_OrderID_Update";
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

		public bool isTotalSalesExsist(TotalSalesData TotalSalesDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_FICHSalesHeader_Check '" + TotalSalesDat.Warehouse + "' , '" + TotalSalesDat.Till + "' , '" + TotalSalesDat.Operation + "' ," + TotalSalesDat.Updated;
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
