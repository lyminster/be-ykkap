using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	public struct zSalesPending
	{
		public DateTime TrxDate;

		public string Store;

		public decimal FICHSales;

		public decimal PendingFICHSales;

		public decimal ISSSales;

		public decimal PendingISS;
	}

}
