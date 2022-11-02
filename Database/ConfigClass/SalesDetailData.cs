using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	[Serializable]
	public class SalesDetailData
	{
		public string store = string.Empty;

		public string sales = string.Empty;

		public int line = 0;

		public string article = string.Empty;

		public string plu = string.Empty;

		public decimal price = 0m;

		public decimal priceEx = 0m;

		public decimal qty = 0m;

		public int qtyReturn = 0;

		public decimal grossAmt = 0m;

		public decimal grossAmtEx = 0m;

		public decimal disc1 = 0m;

		public decimal disc1Ex = 0m;

		public decimal disc2 = 0m;

		public decimal disc2Ex = 0m;

		public decimal netAmt = 0m;

		public decimal netAmtEx = 0m;

		public int promo = 0;

		public string descPromo = string.Empty;

		public short membership = 0;

		public string spg = string.Empty;

		public string description1 = string.Empty;

		public string description2 = string.Empty;

		public short promo2 = 0;

		public string descPromo2 = string.Empty;
	}

}
