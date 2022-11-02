using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

	[Serializable]
	public class HistoryMovDetailData
	{
		public int seqno = 0;

		public string storeno = string.Empty;

		public string docno = string.Empty;

		public string doctypeno = string.Empty;

		public int lineno = 0;

		public string articleno = string.Empty;

		public string uom = string.Empty;

		public double qty = 0.0;

		public string pluno = string.Empty;

		public string name = string.Empty;

		public string uomPlu = string.Empty;

		public double qtyPlu = 0.0;

		public double variancePlu = 0.0;

		public string sloc = string.Empty;

		public string reasonno = string.Empty;

		public string remark = string.Empty;
	}
}
