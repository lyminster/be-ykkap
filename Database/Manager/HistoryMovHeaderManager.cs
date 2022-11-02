using Database.ConfigClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class HistoryMovHeaderManager
	{
		protected class StoredProcedure
		{
			public const string GetHistoryMovement_TsOGift = "sp_GetHistoryMovement_TsOGift";

			public const string GetHistoryMovement_TsO = "sp_GetHistoryMovement_TsO";

			public const string GetHistoryMovement_TsI = "sp_GetHistoryMovement_TsI";

			public const string HistoryMovHeader_Insert = "sp_HistoryMovHeader_Insert";

			public const string isHostMovDoc_Exist = "sp_HostMovDoc_Check";

			public const string GetHistoryMovDetail_TsO = "sp_GetHistoryMovDetail_TsO";

			public const string isPLUExsist = "sp_isPLUExist";

			public const string PROCESS_MOVEMENT_M = "sp_GetHistoryMovement_M";

			public const string PROCESS_MOVEMENT_C = "sp_GetHistoryMovement_C";

			public const string PROCESS_MOVEMENT_H = "sp_GetHistoryMovement_H";

			public const string PROCESS_MOVEMENT_I = "sp_GetHistoryMovement_I";

			public const string PROCESS_MOVEMENT_TsIDevo = "sp_AutoConfirm_DevoFromStore";

			public const string PROCESS_MOVEMENT_TsODevo = "sp_CreateTsO_DevoFromLung";

			public const string PROCESS_MOVEMENT_AutoConfirmTsODevo = "sp_AutoConfirm_DevoFromLung";

			public const string PROCESS_MOVEMENT_AutoConfirm_TsOePacktoZOI = "sp_AutoConfirm_TsOePacktoZOI";

			public const string PROCESS_MOVEMENT_AutoConfirm_TsOFromZOI = "sp_AutoConfirm_TsOFromZOI";

			public const string PROCESS_MOVEMENT_AutoConfirm_TsOePackLeftOver = "sp_AutoConfirm_TsOePackLeftOver";

			public const string PROCESS_MOVEMENT_AutoConfirm_AutoGR_POePack = "sp_AutoGR_POePack";

			public const string PROCESS_MOVEMENT_AutoConfirm_TsOfromSINT = "sp_AutoConfirm_TsOfromSINT";

			public const string is_ArtFlagDeletion = "sp_is_ArtFlagDeletion";

			public const string PROCESS_HistoryMovementHold = "sp_GetHistoryMovementHold";

			public const string CANCEL_TSO = "sp_cancelTSO";

			public const string CANCEL_TSO_DiffDate = "sp_CancelTSO_DiffDate";

			public const string CANCEL_TSO_DiffDoc = "sp_CancelTSO_DiffDoc#";

			public const string CANCEL_TSI = "sp_cancelTSI";

			public const string CANCEL_TSI_DiffDate = "sp_CancelTSI_DiffDate";

			public const string CANCEL_TSI_DiffDoc = "sp_CancelTSI_DiffDoc#";

			public const string REMARK_DOUBLE_I = "sp_remarkDouble";

			public const string ArticleNotUploaded = "sp_CheckArticleNotUploaded_MOV";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void Insert(HistoryMovHeaderData HistoryMovHeaderDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_HistoryMovHeader_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@seq#", HistoryMovHeaderDat.seqno));
			sPCommand.Parameters.Add(new SqlParameter("@store#", HistoryMovHeaderDat.storeno));
			sPCommand.Parameters.Add(new SqlParameter("@doc#", HistoryMovHeaderDat.docno));
			sPCommand.Parameters.Add(new SqlParameter("@doctype#", HistoryMovHeaderDat.doctypeno));
			sPCommand.Parameters.Add(new SqlParameter("@docdate", HistoryMovHeaderDat.docdate));
			sPCommand.Parameters.Add(new SqlParameter("@sender", HistoryMovHeaderDat.sender));
			sPCommand.Parameters.Add(new SqlParameter("@recipient", HistoryMovHeaderDat.recipient));
			sPCommand.Parameters.Add(new SqlParameter("@reference#", HistoryMovHeaderDat.referenceno));
			sPCommand.Parameters.Add(new SqlParameter("@reason#", HistoryMovHeaderDat.reasonno));
			sPCommand.Parameters.Add(new SqlParameter("@remark", HistoryMovHeaderDat.remark));
			sPCommand.Parameters.Add(new SqlParameter("@totalqty", HistoryMovHeaderDat.totalQty));
			sPCommand.Parameters.Add(new SqlParameter("@printed", HistoryMovHeaderDat.printed));
			sPCommand.Parameters.Add(new SqlParameter("@closed", HistoryMovHeaderDat.closed));
			sPCommand.Parameters.Add(new SqlParameter("@fromref", HistoryMovHeaderDat.fromRef));
			sPCommand.Parameters.Add(new SqlParameter("@testkey", HistoryMovHeaderDat.testkey));
			sPCommand.Parameters.Add(new SqlParameter("@createdon", HistoryMovHeaderDat.createdOn));
			sPCommand.Parameters.Add(new SqlParameter("@updatedon", HistoryMovHeaderDat.updatedOn));
			sPCommand.Parameters.Add(new SqlParameter("@confirmedon", HistoryMovHeaderDat.confirmedOn));
			sPCommand.Parameters.Add(new SqlParameter("@createdby", SESSION.UserName));
			sPCommand.Parameters.Add(new SqlParameter("@pluexist", HistoryMovHeaderDat.pluexist));
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

		public bool isMovHeaderExist(HistoryMovHeaderData HistoryMovHeaderDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_HostMovDoc_Check '" + HistoryMovHeaderDat.storeno + "' , '" + HistoryMovHeaderDat.docno + "' ";
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

		public bool Cancel_TSO()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_cancelTSO";
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

		public bool Cancel_TSI()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_cancelTSI";
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

		public bool ProcessMovement_TsO()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovement_TsO";
				if (textCommand.ExecuteNonQuery() != 0)
				{
					result = true;
				}
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return result;
		}

		public bool ProcessMovement_TsOGift()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovement_TsOGift";
				if (textCommand.ExecuteNonQuery() != 0)
				{
					result = true;
				}
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return result;
		}

		public bool ProcessMovement_TsI()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovement_TsI";
				if (textCommand.ExecuteNonQuery() != 0)
				{
					result = true;
				}
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return result;
		}

		public bool Process_H()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovement_H";
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

		public bool Process_I()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovement_I";
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

		public bool ProcessHistoryMovementHold()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_GetHistoryMovementHold";
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

		public bool Process_TsODevo()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_CreateTsO_DevoFromLung";
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

		public bool Process_AutoConfirmTsODevo()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_AutoConfirm_DevoFromLung";
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

		public bool Process_is_ArtFlagDeletion()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_is_ArtFlagDeletion";
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

		public bool RemarkDouble()
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			int num = 0;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_remarkDouble";
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

		public ArrayList FetchListArticleNotUploaded()
		{
			ArrayList arrayList = new ArrayList();
			SqlDataReader sqlDataReader = null;
			try
			{
				SqlCommand sPCommand = dbCom.GetSPCommand();
				sPCommand.CommandText = "sp_CheckArticleNotUploaded_MOV";
				sqlDataReader = sPCommand.ExecuteReader();
				while (sqlDataReader.Read())
				{
					ArticleUploadDataMov articleUploadDataMov = default(ArticleUploadDataMov);
					articleUploadDataMov.MovementType = sqlDataReader.GetString(sqlDataReader.GetOrdinal("MovementType"));
					articleUploadDataMov.Store = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Store"));
					articleUploadDataMov.Document = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Document"));
					articleUploadDataMov.Article = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Article"));
					articleUploadDataMov.Plu = sqlDataReader.GetString(sqlDataReader.GetOrdinal("Plu"));
					arrayList.Add(articleUploadDataMov);
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return arrayList;
		}

		public bool is_PLU_Exsist(string PLU)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_isPLUExist '" + PLU + "'";
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
