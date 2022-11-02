using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	[Serializable]
	public class PaymentLineCOMData
	{
		public string Warehouse = string.Empty;

		public string RegType = string.Empty;

		public string Till = string.Empty;

		public string Operation = string.Empty;

		public string OpsDate = string.Empty;

		public string OpsTime = string.Empty;

		public int Line = 0;

		public string ClaveTC = string.Empty;

		public string OnlineOrderID = string.Empty;
	}

}
