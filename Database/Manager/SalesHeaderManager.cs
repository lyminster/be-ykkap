using Database.ConfigClass;
using Database.ModelsClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class SalesHeaderManager
	{
		public class Identity
		{
			public const string TABLE_Version = "Version";
		}

		protected class StoredProcedure
		{
			public const string CancelGIFT_FlagUpdate = "sp_CancelGIFT_FlagUpdate";

			public const string GetSalesHeader = "sp_GetSalesHeader";

			public const string SalesHeader_Insert = "sp_SalesHeader_Insert";

			public const string SalesHeader_Check = "sp_IssSales_Check";

			public const string GetSalesDetail = "sp_GetSalesDetail";

			public const string GetSalesPayment = "sp_GetSalesPayment";

			public const string FICHSalesHeader_FlagUpdate = "sp_FICHSalesHeader_FlagUpdate";

			public const string GetSpecialArticle = "sp_GetSpecialArticle";

			public const string MissingArticle_Report = "sp_GetMissingArticle";

			public const string TrxDocsPending_Report = "sp_GetTrxPending";

			public const string GetServerTime = "sp_GetServerTime";

			public const string SalesPending_Report = "sp_GetSalesPending";

			public const string Get_PLU_Alternate = "sp_Get_PLU_Alteration";

			public const string Sales_Compare = "sp_Sales_Compare";

			public const string ArticleNotUploaded = "sp_CheckArticleNotUploaded";

			public const string SALES_AUTO_BOUND = "sp_is_SalesAutoBound";

			public const string Get_SalesHold = "sp_GetSalesHold";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void InsertSalesHeader(SalesHeaderData SalesHeaderDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_SalesHeader_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@store", SalesHeaderDat.store));
			sPCommand.Parameters.Add(new SqlParameter("@sales", SalesHeaderDat.sales));
			sPCommand.Parameters.Add(new SqlParameter("@pos", SalesHeaderDat.pos));
			sPCommand.Parameters.Add(new SqlParameter("@salesDate", SalesHeaderDat.salesDate));
			sPCommand.Parameters.Add(new SqlParameter("@rate", SalesHeaderDat.rate));
			sPCommand.Parameters.Add(new SqlParameter("@currency", SalesHeaderDat.currency));
			sPCommand.Parameters.Add(new SqlParameter("@totalGrossAmt", SalesHeaderDat.totalGrossAmt));
			sPCommand.Parameters.Add(new SqlParameter("@totalGrossAmtex", SalesHeaderDat.totalGrossAmtex));
			sPCommand.Parameters.Add(new SqlParameter("@totalDisc1", SalesHeaderDat.totalDisc1));
			sPCommand.Parameters.Add(new SqlParameter("@totalDisc1Ex", SalesHeaderDat.totalDisc1Ex));
			sPCommand.Parameters.Add(new SqlParameter("@totalDisc2", SalesHeaderDat.totalDisc2));
			sPCommand.Parameters.Add(new SqlParameter("@totalDisc2Ex", SalesHeaderDat.totalDisc2Ex));
			sPCommand.Parameters.Add(new SqlParameter("@totalNetAmt", SalesHeaderDat.totalNetAmt));
			sPCommand.Parameters.Add(new SqlParameter("@totalNetAmtEx", SalesHeaderDat.totalNetAmtEx));
			sPCommand.Parameters.Add(new SqlParameter("@reference", SalesHeaderDat.reference));
			sPCommand.Parameters.Add(new SqlParameter("@refManual", SalesHeaderDat.refManual));
			sPCommand.Parameters.Add(new SqlParameter("@customer", SalesHeaderDat.customer));
			sPCommand.Parameters.Add(new SqlParameter("@deposit", SalesHeaderDat.deposit));
			sPCommand.Parameters.Add(new SqlParameter("@returnType", SalesHeaderDat.returnType));
			sPCommand.Parameters.Add(new SqlParameter("@printed", SalesHeaderDat.printed));
			sPCommand.Parameters.Add(new SqlParameter("@UpdatedBy", SESSION.UserName));
			sPCommand.ExecuteNonQuery();
		}

		public void UpdateSalesandReturFlag(KeyFICHData KeyFICH)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_FICHSalesHeader_FlagUpdate";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", KeyFICH.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@Till", KeyFICH.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", KeyFICH.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", KeyFICH.FICHFileName));
			sPCommand.ExecuteNonQuery();
		}

		public void SalesHold()
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_GetSalesHold";
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

		public bool CancelGIFTFlagUpdate()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_CancelGIFT_FlagUpdate";
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

		public bool IsSalesExsist(SalesHeaderData SalesHeaderDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_IssSales_Check '" + SalesHeaderDat.store + "' , '" + SalesHeaderDat.sales + "'";
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

		public ArrayList FetchListSales()
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			SalesHeaderData salesHeaderData = null;
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_GetSalesHeader";
			sqlDataReader = sPCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				salesHeaderData = new SalesHeaderData();
				salesHeaderData.store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("store"));
				salesHeaderData.sales = sqlDataReader.GetString(sqlDataReader.GetOrdinal("sales"));
				salesHeaderData.pos = sqlDataReader.GetString(sqlDataReader.GetOrdinal("pos"));
				salesHeaderData.salesDate = Convert.ToDateTime(sqlDataReader.GetString(sqlDataReader.GetOrdinal("salesDate")));
				salesHeaderData.currency = sqlDataReader.GetString(sqlDataReader.GetOrdinal("currency"));
				salesHeaderData.customer = sqlDataReader.GetString(sqlDataReader.GetOrdinal("customer"));
				salesHeaderData.deposit = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("deposit"));
				salesHeaderData.printed = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("printed"));
				salesHeaderData.rate = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("rate"));
				salesHeaderData.reference = sqlDataReader.GetString(sqlDataReader.GetOrdinal("reference"));
				salesHeaderData.refManual = sqlDataReader.GetString(sqlDataReader.GetOrdinal("refManual"));
				salesHeaderData.returnType = Convert.ToInt32(sqlDataReader.GetString(sqlDataReader.GetOrdinal("returnType")));
				salesHeaderData.totalDisc1 = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalDisc1"));
				salesHeaderData.totalDisc1Ex = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalDisc1Ex"));
				salesHeaderData.totalDisc2 = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalDisc2"));
				salesHeaderData.totalDisc2Ex = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalDisc2Ex"));
				salesHeaderData.totalGrossAmt = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalGrossAmt"));
				salesHeaderData.totalGrossAmtex = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalGrossAmtex"));
				salesHeaderData.totalNetAmt = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalNetAmt"));
				salesHeaderData.totalNetAmtEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("totalNetAmtEx"));
				salesHeaderData.KeyFICH.Warehouse = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Warehouse"));
				salesHeaderData.KeyFICH.Till = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Till"));
				salesHeaderData.KeyFICH.Operation = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Operation"));
				salesHeaderData.KeyFICH.FICHFileName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICHFileName"));
				arrayList.Add(salesHeaderData);
			}
			sqlDataReader?.Close();
			for (int i = 0; i < arrayList.Count; i++)
			{
				salesHeaderData = (SalesHeaderData)arrayList[i];
				salesHeaderData.SalesDetails = FetchListSalesDetail(salesHeaderData.KeyFICH);
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				salesHeaderData = (SalesHeaderData)arrayList[i];
				salesHeaderData.SalesPayments = FetchListSalesPayment(salesHeaderData.KeyFICH);
			}
			return arrayList;
		}

		private ArrayList FetchListSalesDetail(KeyFICHData KeyFICHDat)
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			SalesDetailData salesDetailData = null;
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_GetSalesDetail";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", KeyFICHDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@Till", KeyFICHDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", KeyFICHDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", KeyFICHDat.FICHFileName));
			sqlDataReader = sPCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				salesDetailData = new SalesDetailData();
				salesDetailData.article = sqlDataReader.GetString(sqlDataReader.GetOrdinal("article"));
				salesDetailData.descPromo = sqlDataReader.GetString(sqlDataReader.GetOrdinal("descPromo"));
				salesDetailData.descPromo2 = sqlDataReader.GetString(sqlDataReader.GetOrdinal("descPromo2"));
				salesDetailData.description1 = sqlDataReader.GetString(sqlDataReader.GetOrdinal("description1"));
				salesDetailData.description2 = sqlDataReader.GetString(sqlDataReader.GetOrdinal("description2"));
				salesDetailData.disc1 = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("disc1"));
				salesDetailData.disc1Ex = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("disc1Ex"));
				salesDetailData.disc2 = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("disc2"));
				salesDetailData.disc2Ex = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("disc2Ex"));
				salesDetailData.grossAmt = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("grossAmt"));
				salesDetailData.grossAmtEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("grossAmtEx"));
				salesDetailData.line = Convert.ToInt32(sqlDataReader.GetString(sqlDataReader.GetOrdinal("line")));
				salesDetailData.membership = Convert.ToInt16(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("membership")));
				salesDetailData.netAmt = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("netAmt"));
				salesDetailData.netAmtEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("netAmtEx"));
				salesDetailData.plu = sqlDataReader.GetString(sqlDataReader.GetOrdinal("plu"));
				salesDetailData.price = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("price"));
				salesDetailData.priceEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("priceEx"));
				salesDetailData.promo = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("promo"));
				salesDetailData.promo2 = Convert.ToInt16(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("promo2")));
				salesDetailData.qty = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("qty"));
				salesDetailData.qtyReturn = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("qtyReturn"));
				salesDetailData.sales = sqlDataReader.GetString(sqlDataReader.GetOrdinal("sales"));
				salesDetailData.spg = sqlDataReader.GetString(sqlDataReader.GetOrdinal("spg"));
				salesDetailData.store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("store"));
				arrayList.Add(salesDetailData);
			}
			sqlDataReader?.Close();
			return arrayList;
		}

		private ArrayList FetchListSalesPayment(KeyFICHData KeyFICHDat)
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			SalesPaymentData salesPaymentData = null;
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_GetSalesPayment";
			sPCommand.Parameters.Add(new SqlParameter("@Warehouse", KeyFICHDat.Warehouse));
			sPCommand.Parameters.Add(new SqlParameter("@Till", KeyFICHDat.Till));
			sPCommand.Parameters.Add(new SqlParameter("@Operation", KeyFICHDat.Operation));
			sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", KeyFICHDat.FICHFileName));
			sqlDataReader = sPCommand.ExecuteReader();
			while (sqlDataReader.Read())
			{
				salesPaymentData = new SalesPaymentData();
				salesPaymentData.amt = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("amt"));
				salesPaymentData.amtEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("amtEx"));
				salesPaymentData.amtPaid = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("amtPaid"));
				salesPaymentData.change = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("change"));
				salesPaymentData.changeEx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("changeEx"));
				salesPaymentData.currency = sqlDataReader.GetString(sqlDataReader.GetOrdinal("currency"));
				salesPaymentData.description = sqlDataReader.GetString(sqlDataReader.GetOrdinal("description"));
				salesPaymentData.line = Convert.ToInt32(sqlDataReader.GetValue(sqlDataReader.GetOrdinal("line")));
				salesPaymentData.payment = sqlDataReader.GetString(sqlDataReader.GetOrdinal("payment"));
				salesPaymentData.paymentNo = sqlDataReader.GetString(sqlDataReader.GetOrdinal("paymentNo"));
				salesPaymentData.rate = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("rate"));
				salesPaymentData.sales = sqlDataReader.GetString(sqlDataReader.GetOrdinal("sales"));
				salesPaymentData.store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("store"));
				arrayList.Add(salesPaymentData);
			}
			sqlDataReader?.Close();
			return arrayList;
		}

		public ArrayList FetchListSpecialArticle()
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			SpecialArticleData specialArticleData = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_GetSpecialArticle";
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					specialArticleData = new SpecialArticleData();
					specialArticleData.FICHMCS = sqlDataReader.GetString(sqlDataReader.GetOrdinal("FICHMCS"));
					specialArticleData.Article = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Article#"));
					specialArticleData.PLU = sqlDataReader.GetString(sqlDataReader.GetOrdinal("PLU#"));
					specialArticleData.Brand = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Brand"));
					arrayList.Add(specialArticleData);
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return arrayList;
		}

		public ArrayList FetchListMissingArticle()
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_GetMissingArticle";
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					zMissingArticle zMissingArticle2 = default(zMissingArticle);
					zMissingArticle2.Article = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Article"));
					arrayList.Add(zMissingArticle2);
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return arrayList;
		}

		public ArrayList FetchListTrxDocsPending()
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_GetTrxPending";
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					zTrxDocsPending zTrxDocsPending2 = default(zTrxDocsPending);
					zTrxDocsPending2.Store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Store"));
					zTrxDocsPending2.TrxType = sqlDataReader.GetString(sqlDataReader.GetOrdinal("TrxType"));
					zTrxDocsPending2.TrxDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("TrxDate"));
					zTrxDocsPending2.TrxDoc = sqlDataReader.GetString(sqlDataReader.GetOrdinal("TrxDoc"));
					zTrxDocsPending2.TotalTrx = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("TotalTrx"));
					zTrxDocsPending2.Remark = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Remark"));
					arrayList.Add(zTrxDocsPending2);
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return arrayList;
		}

		public ArrayList FetchSalesPending(string SalesDate)
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_GetSalesPending";
				sPCommand.Parameters.Add(new SqlParameter("@SalesDate", SalesDate));
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					zSalesPending zSalesPending2 = default(zSalesPending);
					zSalesPending2.TrxDate = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("TrxDate"));
					zSalesPending2.Store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Store"));
					zSalesPending2.FICHSales = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("FICHSales"));
					zSalesPending2.PendingFICHSales = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("PendingFICHSales"));
					zSalesPending2.ISSSales = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("ISSSales"));
					arrayList.Add(zSalesPending2);
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return arrayList;
		}

		public DateTime FetchDateServer()
		{
			DateTime result = Convert.ToDateTime("1900-01-01 00:00:00");
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_GetServerTime";
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					result = sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal("ServerDate"));
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return result;
		}

		//public bool isUpdateVersion(Version VersionDat)
		//{
		//	SqlDataReader sqlDataReader = null;
		//	bool result = false;
		//	try
		//	{
		//		SqlCommand textCommand = dbCom.GetTextCommand();
		//		textCommand.CommandText = "Select * from Version where Major = '" + VersionDat.Major + "' and Minor = '" + VersionDat.Minor + "' and Revision = '" + VersionDat.Revision + "'";
		//		sqlDataReader = textCommand.ExecuteReader();
		//		if (sqlDataReader.HasRows)
		//		{
		//			result = true;
		//		}
		//		sqlDataReader?.Close();
		//	}
		//	finally
		//	{
		//		sqlDataReader?.Close();
		//	}
		//	return result;
		//}

		public bool SalesAutoBound()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_is_SalesAutoBound";
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
	}

}
