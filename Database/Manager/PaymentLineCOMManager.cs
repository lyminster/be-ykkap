using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class PaymentLineCOMManager
	{
		public class Identity
		{
			public const string Name = "Payment Line";

			public const string TableName = "FICHPaymentLineCOM";

			public const string RegType = "CMK";
		}

		protected class StoredProcedure
		{
			public const string PaymentLineCOM_Insert = "sp_PaymentLineCOM_Insert";

			public const string isPaymentLineCOM_Exsist = "sp_PaymentLineCOM_Check";

			public const string PaymentLineCOM_Count = "sp_PaymentLineCOM_Check";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void PaymentLineCOMInsert(PaymentLineCOMData PaymentLineCOMDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_PaymentLineCOM_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", PaymentLineCOMDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@RegType", PaymentLineCOMDat.RegType));
			sPCommand.Parameters.Add(new SqlParameter("@Till", PaymentLineCOMDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", PaymentLineCOMDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@OpsDate", PaymentLineCOMDat.OpsDate));
			sPCommand.Parameters.Add(new SqlParameter("@OpsTime", PaymentLineCOMDat.OpsTime));
			sPCommand.Parameters.Add(new SqlParameter("@Line", PaymentLineCOMDat.Line));
			sPCommand.Parameters.Add(new SqlParameter("@ClaveTC", PaymentLineCOMDat.ClaveTC));
			sPCommand.Parameters.Add(new SqlParameter("@OnlineOrderID", PaymentLineCOMDat.OnlineOrderID));
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

		public bool isPaymentLineCOMExsist(PaymentLineCOMData PaymentLineCOMDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_PaymentLineCOM_Check '" + PaymentLineCOMDat.Warehouse + "' , '" + PaymentLineCOMDat.RegType + "' , " + PaymentLineCOMDat.Till + "' , '" + PaymentLineCOMDat.Operation + "' , " + PaymentLineCOMDat.OpsDate + "' , " + PaymentLineCOMDat.ClaveTC + "' , " + PaymentLineCOMDat.OnlineOrderID + "'";
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

		public int PaymentLineCOMCount(PaymentLineCOMData PaymentLineCOMDat)
		{
			SqlDataReader sqlDataReader = null;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_PaymentLineCOM_Check '" + PaymentLineCOMDat.Warehouse + "' , '" + PaymentLineCOMDat.RegType + "' , '" + PaymentLineCOMDat.Till + "' , '" + PaymentLineCOMDat.Operation + "' , '" + PaymentLineCOMDat.OpsDate + "' , '" + PaymentLineCOMDat.ClaveTC + "' , '" + PaymentLineCOMDat.OnlineOrderID + "'";
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
