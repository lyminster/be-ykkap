using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class SalesPaymentManager
	{
		protected class StoredProcedure
		{
			public const string SalesPayment_Insert = "sp_SalesPayment_Insert";

			public const string IssPayment_Check = "sp_IssPayment_Check";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void InsertSalesPayment(SalesPaymentData SalesPaymentDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_SalesPayment_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@store", SalesPaymentDat.store));
			sPCommand.Parameters.Add(new SqlParameter("@sales", SalesPaymentDat.sales));
			sPCommand.Parameters.Add(new SqlParameter("@line", SalesPaymentDat.line));
			sPCommand.Parameters.Add(new SqlParameter("@currency", SalesPaymentDat.currency));
			sPCommand.Parameters.Add(new SqlParameter("@rate", SalesPaymentDat.rate));
			sPCommand.Parameters.Add(new SqlParameter("@amt", SalesPaymentDat.amt));
			sPCommand.Parameters.Add(new SqlParameter("@amtEx", SalesPaymentDat.amtEx));
			sPCommand.Parameters.Add(new SqlParameter("@amtPaid", SalesPaymentDat.amtPaid));
			sPCommand.Parameters.Add(new SqlParameter("@change", SalesPaymentDat.change));
			sPCommand.Parameters.Add(new SqlParameter("@changeEx", SalesPaymentDat.changeEx));
			sPCommand.Parameters.Add(new SqlParameter("@payment", SalesPaymentDat.payment));
			sPCommand.Parameters.Add(new SqlParameter("@paymentNo", SalesPaymentDat.paymentNo));
			sPCommand.Parameters.Add(new SqlParameter("@description", SalesPaymentDat.description));
			sPCommand.Parameters.Add(new SqlParameter("@UpdatedBy", SESSION.UserName));
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

		public bool isPaymentExsist(SalesPaymentData SalesPaymentDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_IssPayment_Check '" + SalesPaymentDat.store + "' , '" + SalesPaymentDat.sales + "' , '" + SalesPaymentDat.line + "'";
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
