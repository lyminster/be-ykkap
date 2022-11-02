using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

	[Serializable]
	public class HistoryMovHeaderData
	{
		public long seqno;

		public string storeno = string.Empty;

		public string docno = string.Empty;

		public string doctypeno = string.Empty;

		public DateTime docdate;

		public string sender = string.Empty;

		public string recipient = string.Empty;

		public string referenceno = string.Empty;

		public string reasonno = string.Empty;

		public string remark = string.Empty;

		public double totalQty;

		public int printed = 0;

		public int closed = 0;

		public int fromRef = 0;

		public string testkey = string.Empty;

		public DateTime createdOn;

		public string createdBy = string.Empty;

		public DateTime updatedOn;

		public DateTime confirmedOn;

		public DateTime receivedOn;

		public int pluexist;

		public ArrayList HistoryMovDetail = null;
	}

}
