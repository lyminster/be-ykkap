using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class HistoryMovDetailManager
	{
		protected class StoredProcedure
		{
			public const string INSERT_MOV_DETAIL = "sp_HistoryMovDetail_Insert";

			public const string is_MOVDETAIL_EXSIST = "sp_is_MOVDETAIL_EXSIST";

			public const string isPLUExsist = "sp_isPLUExsist";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void Insert(HistoryMovDetailData HistoryMovDetailDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_HistoryMovDetail_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@seq#", HistoryMovDetailDat.seqno));
			sPCommand.Parameters.Add(new SqlParameter("@store#", HistoryMovDetailDat.storeno));
			sPCommand.Parameters.Add(new SqlParameter("@doc#", HistoryMovDetailDat.docno));
			sPCommand.Parameters.Add(new SqlParameter("@docType#", HistoryMovDetailDat.doctypeno));
			sPCommand.Parameters.Add(new SqlParameter("@line#", HistoryMovDetailDat.lineno));
			sPCommand.Parameters.Add(new SqlParameter("@article#", HistoryMovDetailDat.articleno));
			sPCommand.Parameters.Add(new SqlParameter("@uom", HistoryMovDetailDat.uom));
			sPCommand.Parameters.Add(new SqlParameter("@qty", HistoryMovDetailDat.qty));
			sPCommand.Parameters.Add(new SqlParameter("@plu#", HistoryMovDetailDat.pluno));
			sPCommand.Parameters.Add(new SqlParameter("@name", HistoryMovDetailDat.name));
			sPCommand.Parameters.Add(new SqlParameter("@uomPlu", HistoryMovDetailDat.uomPlu));
			sPCommand.Parameters.Add(new SqlParameter("@qtyPlu", HistoryMovDetailDat.qtyPlu));
			sPCommand.Parameters.Add(new SqlParameter("@variancePlu", HistoryMovDetailDat.variancePlu));
			sPCommand.Parameters.Add(new SqlParameter("@sloc", HistoryMovDetailDat.sloc));
			sPCommand.Parameters.Add(new SqlParameter("@reason#", HistoryMovDetailDat.reasonno));
			sPCommand.Parameters.Add(new SqlParameter("@remark", HistoryMovDetailDat.remark));
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

		public bool isMovExsist(HistoryMovDetailData HistoryMovDetailDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_is_MOVDETAIL_EXSIST '" + HistoryMovDetailDat.docno + "' , '" + HistoryMovDetailDat.storeno + "' ";
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

		public bool is_PLU_Exsist(HistoryMovDetailData HistoryMovDetailDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_isPLUExsist '" + HistoryMovDetailDat.pluno + "'";
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
