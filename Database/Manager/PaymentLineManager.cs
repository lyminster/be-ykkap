using Database.ConfigClass;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Manager
{
    public class PaymentLineManager
    {
        public class Identity
        {
            public const string Name = "Payment Line";

            public const string TableName = "FICHPaymentLine";

            public const string RegType = "T";
        }

        protected class StoredProcedure
        {
            public const string PaymentLine_Insert = "sp_PaymentLine_Insert";

            public const string isPaymentLine_Exsist = "sp_PaymentLine_Check";

            public const string PaymentLine_Count = "sp_PaymentLine_Check";

            public const string eCOMPayment_Insert = "sp_eCOMPayment_Insert";
        }

        private Database.ConfigClass.Database.DBCommand dbCom = null;

        public void PaymentLineInsert(PaymentLineData PaymentLineDat)
        {
            SqlCommand sPCommand = dbCom.GetSPCommand();
            sPCommand.CommandText = "sp_PaymentLine_Insert";
            sPCommand.Parameters.Add(new SqlParameter("@Warehouse", PaymentLineDat.Warehouse));
            sPCommand.Parameters.Add(new SqlParameter("@RegType", PaymentLineDat.RegType));
            sPCommand.Parameters.Add(new SqlParameter("@Till", PaymentLineDat.Till));
            sPCommand.Parameters.Add(new SqlParameter("@Operation", PaymentLineDat.Operation));
            sPCommand.Parameters.Add(new SqlParameter("@OpsDate", PaymentLineDat.OpsDate));
            sPCommand.Parameters.Add(new SqlParameter("@OpsTime", PaymentLineDat.OpsTime));
            sPCommand.Parameters.Add(new SqlParameter("@Line", PaymentLineDat.Line));
            sPCommand.Parameters.Add(new SqlParameter("@ClaveTC", PaymentLineDat.ClaveTC));
            if (Convert.ToDouble(PaymentLineDat.CashAmt).ToString() == "0")
            {
                sPCommand.Parameters.Add(new SqlParameter("@CashSign", ""));
                sPCommand.Parameters.Add(new SqlParameter("@CashAmt", "0"));
                if (Convert.ToDouble(PaymentLineDat.DebitAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@DebitSign", ""));
                    sPCommand.Parameters.Add(new SqlParameter("@DebitAmt", "0"));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@DebitSign", PaymentLineDat.SignDebit));
                    sPCommand.Parameters.Add(new SqlParameter("@DebitAmt", PaymentLineDat.DebitAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.CreditAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@CreditSign", ""));
                    sPCommand.Parameters.Add(new SqlParameter("@CreditAmt", "0"));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@CreditSign", PaymentLineDat.SignCredit));
                    sPCommand.Parameters.Add(new SqlParameter("@CreditAmt", PaymentLineDat.CreditAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.VoucherAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherAmt", "0"));
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherSign", ""));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherSign", PaymentLineDat.SignVouch));
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherAmt", PaymentLineDat.VoucherAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.ChequeAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeAmt", "0"));
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeSign", ""));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeSign", PaymentLineDat.SignCheque));
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeAmt", PaymentLineDat.ChequeAmt));
                }
            }
            else
            {
                sPCommand.Parameters.Add(new SqlParameter("@CashSign", PaymentLineDat.SignCash));
                sPCommand.Parameters.Add(new SqlParameter("@CashAmt", PaymentLineDat.CashAmt));
                if (Convert.ToDouble(PaymentLineDat.DebitAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@DebitSign", ""));
                    sPCommand.Parameters.Add(new SqlParameter("@DebitAmt", "0"));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@DebitSign", PaymentLineDat.SignDebit));
                    sPCommand.Parameters.Add(new SqlParameter("@DebitAmt", PaymentLineDat.DebitAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.CreditAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@CreditSign", ""));
                    sPCommand.Parameters.Add(new SqlParameter("@CreditAmt", "0"));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@CreditSign", PaymentLineDat.SignCredit));
                    sPCommand.Parameters.Add(new SqlParameter("@CreditAmt", PaymentLineDat.CreditAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.VoucherAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherAmt", "0"));
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherSign", ""));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherSign", PaymentLineDat.SignVouch));
                    sPCommand.Parameters.Add(new SqlParameter("@VoucherAmt", PaymentLineDat.VoucherAmt));
                }
                if (Convert.ToDouble(PaymentLineDat.ChequeAmt).ToString() == "0")
                {
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeAmt", "0"));
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeSign", ""));
                }
                else
                {
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeSign", PaymentLineDat.SignCheque));
                    sPCommand.Parameters.Add(new SqlParameter("@ChequeAmt", PaymentLineDat.ChequeAmt));
                }
            }
            sPCommand.Parameters.Add(new SqlParameter("@FICHFileName", SESSION.FileName));
            sPCommand.ExecuteNonQuery();
        }

        public void eCOMPaymentInsert(PaymentLineData PaymentLineDat)
        {
            SqlCommand sPCommand = dbCom.GetSPCommand();
            sPCommand.CommandText = "sp_eCOMPayment_Insert";
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

        public bool isPaymentLineExsist(PaymentLineData PaymentLineDat)
        {
            SqlDataReader sqlDataReader = null;
            bool result = false;
            try
            {
                SqlCommand textCommand = dbCom.GetTextCommand();
                textCommand.CommandText = "EXEC sp_PaymentLine_Check '" + PaymentLineDat.Warehouse + "' , '" + PaymentLineDat.RegType + "' , " + PaymentLineDat.Till + "' , '" + PaymentLineDat.Operation + "' , " + PaymentLineDat.OpsDate + "' , " + PaymentLineDat.ClaveTC + "'";
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

        public int PaymentLineCount(PaymentLineData PaymentLineDat)
        {
            SqlDataReader sqlDataReader = null;
            int num = 0;
            try
            {
                SqlCommand textCommand = dbCom.GetTextCommand();
                textCommand.CommandText = "EXEC sp_PaymentLine_Check '" + PaymentLineDat.Warehouse + "' , '" + PaymentLineDat.RegType + "' , '" + PaymentLineDat.Till + "' , '" + PaymentLineDat.Operation + "' , '" + PaymentLineDat.OpsDate + "' , '" + PaymentLineDat.OpsDate + "'";
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
