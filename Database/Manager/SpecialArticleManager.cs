using Database.ConfigClass;
using Database.ModelsClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{

	public class SpecialArticleManager
	{
		protected class StoredProcedure
		{
			public const string INSERT_Alteration = "sp_Alteration_Insert";

			public const string is_SpecialArticle_Exist = "sp_is_SpecialArticle_Exist";

			public const string get_SpecialArticle = "sp_SpecialArticle";
		}

		protected class View
		{
			public const string FETCH = "vw_SpecialArticle_Fetch";
		}

		public class Identity
		{
			public const string NAME = "SpecialArticle";

			public const string TABLE = "MNGSpecialArticle";
		}

		private Database.ConfigClass.Database.DBCommand dbCom = null;

		public void Insert(SpecialArticleData SpecialArticleDat)
		{
			SqlCommand sPCommand = dbCom.GetSPCommand();
			sPCommand.CommandText = "sp_Alteration_Insert";
			sPCommand.Parameters.Add(new SqlParameter("@Brand", SpecialArticleDat.Brand));
			sPCommand.Parameters.Add(new SqlParameter("@Remark", SpecialArticleDat.Remark));
			sPCommand.Parameters.Add(new SqlParameter("@Article", SpecialArticleDat.Article));
			sPCommand.Parameters.Add(new SqlParameter("@PLU", SpecialArticleDat.PLU));
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

		public bool isAlterationExsist(SpecialArticleData SpecialArticleDat)
		{
			SqlDataReader sqlDataReader = null;
			bool result = false;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_is_SpecialArticle_Exist '" + SpecialArticleDat.Brand + "' , '" + SpecialArticleDat.Remark + "'";
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

		public DataSet Fetch2DataSet(string szParameter)
		{
			DataSet dataSet = new DataSet("SpecialArticle");
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "Select * from vw_SpecialArticle_Fetch " + szParameter;
				SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
				sqlDataAdapter.TableMappings.Add("Table", "SpecialArticle");
				sqlDataAdapter.SelectCommand = textCommand;
				sqlDataAdapter.Fill(dataSet);
			}
			finally
			{
			}
			return dataSet;
		}

		public string get_BrandCode(string szProdID, string szEAN1, string szStoreID)
		{
			SqlDataReader sqlDataReader = null;
			string text = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_SpecialArticle '" + szProdID + "' , '" + szStoreID + "'";
				sqlDataReader = textCommand.ExecuteReader();
				if (sqlDataReader.HasRows)
				{
					while (sqlDataReader.Read())
					{
						text = sqlDataReader.GetString(0);
					}
				}
				sqlDataReader?.Close();
			}
			finally
			{
				sqlDataReader?.Close();
			}
			return text + szEAN1;
		}

		public string get_BrandCode1(string szProdID, string szStoreID)
		{
			SqlDataReader sqlDataReader = null;
			string result = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			try
			{
				SqlCommand textCommand = dbCom.GetTextCommand();
				textCommand.CommandText = "EXEC sp_SpecialArticle '" + szProdID + "' , '" + szStoreID + "'";
				sqlDataReader = textCommand.ExecuteReader();
				if (sqlDataReader.HasRows)
				{
					while (sqlDataReader.Read())
					{
						result = sqlDataReader.GetString(0);
					}
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

