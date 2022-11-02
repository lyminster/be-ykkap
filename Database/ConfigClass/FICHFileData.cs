using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
	[Serializable]
	public class FICHFileData
	{
		public ArrayList resultHeader = new ArrayList();

		public ArrayList resultStockTransferDetail = new ArrayList();

		public ArrayList resultStockReceiveHeader = new ArrayList();

		public ArrayList resultStockReceiveDetail = new ArrayList();

		public ArrayList resultTotalSales = new ArrayList();

		public ArrayList resultSalesLine = new ArrayList();

		public ArrayList resultPaymentLine = new ArrayList();

		public ArrayList resultPaymentLineCOM = new ArrayList();

		public ArrayList resultReturLine = new ArrayList();

		public ArrayList resultCancelLine = new ArrayList();
	}
}