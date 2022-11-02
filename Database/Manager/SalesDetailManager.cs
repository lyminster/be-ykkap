using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
	public class SalesDetailManager
	{
		protected class StoredProcedure
		{
			public const string SalesDetail_Insert = "sp_SalesDetail_Insert";

			public const string isPLUExsist = "sp_isPLUExsist";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void Insert(SalesDetailData SalesDetailDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_SalesDetail_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@store", SalesDetailDat.store));
			sPCommand.Parameters.Add(new SqlParameter("@sales", SalesDetailDat.sales));
			sPCommand.Parameters.Add(new SqlParameter("@line", SalesDetailDat.line));
			sPCommand.Parameters.Add(new SqlParameter("@article", SalesDetailDat.article));
			sPCommand.Parameters.Add(new SqlParameter("@plu", SalesDetailDat.plu));
			sPCommand.Parameters.Add(new SqlParameter("@price", SalesDetailDat.price));
			sPCommand.Parameters.Add(new SqlParameter("@priceEx", SalesDetailDat.priceEx));
			sPCommand.Parameters.Add(new SqlParameter("@qty", SalesDetailDat.qty));
			sPCommand.Parameters.Add(new SqlParameter("@qtyReturn", SalesDetailDat.qtyReturn));
			sPCommand.Parameters.Add(new SqlParameter("@grossAmt", SalesDetailDat.grossAmt));
			sPCommand.Parameters.Add(new SqlParameter("@grossAmtEx", SalesDetailDat.grossAmtEx));
			sPCommand.Parameters.Add(new SqlParameter("@disc1", SalesDetailDat.disc1));
			sPCommand.Parameters.Add(new SqlParameter("@disc1Ex", SalesDetailDat.disc1Ex));
			sPCommand.Parameters.Add(new SqlParameter("@disc2", SalesDetailDat.disc2));
			sPCommand.Parameters.Add(new SqlParameter("@disc2Ex", SalesDetailDat.disc2Ex));
			sPCommand.Parameters.Add(new SqlParameter("@netAmt", SalesDetailDat.netAmt));
			sPCommand.Parameters.Add(new SqlParameter("@netAmtEx", SalesDetailDat.netAmtEx));
			sPCommand.Parameters.Add(new SqlParameter("@promo", SalesDetailDat.promo));
			sPCommand.Parameters.Add(new SqlParameter("@descPromo", SalesDetailDat.descPromo));
			sPCommand.Parameters.Add(new SqlParameter("@membership", SalesDetailDat.membership));
			sPCommand.Parameters.Add(new SqlParameter("@spg", SalesDetailDat.spg));
			sPCommand.Parameters.Add(new SqlParameter("@description1", SalesDetailDat.description1));
			sPCommand.Parameters.Add(new SqlParameter("@description2", SalesDetailDat.description2));
			sPCommand.Parameters.Add(new SqlParameter("@promo2", SalesDetailDat.promo2));
			sPCommand.Parameters.Add(new SqlParameter("@descPromo2", SalesDetailDat.descPromo2));
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

		public bool is_PLU_Exsist(SalesDetailData SalesDetailDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_isPLUExsist '" + SalesDetailDat.plu + "'";
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

		public bool is_PLU_Exsist(string PLU)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_isPLUExsist '" + PLU + "'";
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
