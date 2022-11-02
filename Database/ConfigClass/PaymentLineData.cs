using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	[Serializable]
	public class PaymentLineData
	{
		public string Warehouse = string.Empty;

		public string RegType = string.Empty;

		public string Till = string.Empty;

		public string Operation = string.Empty;

		public string OpsDate = string.Empty;

		public string OpsTime = string.Empty;

		public int Line = 0;

		public string ClaveTC = string.Empty;

		public string SignCheque = string.Empty;

		public string ChequeAmt = string.Empty;

		public string SignDebit = string.Empty;

		public string DebitAmt = string.Empty;

		public string SignCash = string.Empty;

		public string CashAmt = string.Empty;

		public string SignVouch = string.Empty;

		public string VoucherAmt = string.Empty;

		public string SignCredit = string.Empty;

		public string CreditAmt = string.Empty;

		public string OnlineOrderID = string.Empty;
	}
}
