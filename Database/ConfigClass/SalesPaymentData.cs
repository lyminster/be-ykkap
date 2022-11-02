using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	[Serializable]
	public class SalesPaymentData
	{
		public string store = string.Empty;

		public string sales = string.Empty;

		public int line = 0;

		public string currency = string.Empty;

		public decimal rate = 0m;

		public decimal amt = 0m;

		public decimal amtEx = 0m;

		public decimal amtPaid = 0m;

		public decimal change = 0m;

		public decimal changeEx = 0m;

		public string payment = string.Empty;

		public string paymentNo = string.Empty;

		public string description = string.Empty;
	}

}
