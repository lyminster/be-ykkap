using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

	[Serializable]
	public class ReturLineData
	{
		public string Warehouse = string.Empty;

		public string RegType = string.Empty;

		public string Till = string.Empty;

		public string Operation = string.Empty;

		public string OpsDate = string.Empty;

		public string OpsTime = string.Empty;

		public string OriWH = string.Empty;

		public string OriOpsDate = string.Empty;

		public string OriOperation = string.Empty;

		public string OriQty = string.Empty;

		public string OriClaveTC = string.Empty;

		public string isDiscount = string.Empty;

		public string isPromotion = string.Empty;

		public string isEANScan = string.Empty;
	}
}
