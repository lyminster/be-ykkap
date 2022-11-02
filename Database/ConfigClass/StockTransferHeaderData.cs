using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
    [Serializable]
    public class StockTransferHeaderData
    {
        public string Warehouse = string.Empty;

        public string RegType = string.Empty;

        public string Till = string.Empty;

        public string Operation = string.Empty;

        public string OpsDate = string.Empty;

        public string OpsTime = string.Empty;

        public string ReceiverWH = string.Empty;

        public string Sign = string.Empty;

        public string TotalQty = string.Empty;

        public string FICHFileName = string.Empty;

        public DateTime ProcessDate = DateTime.Now;

        public string Remark = string.Empty;

        public int Uploaded = 0;

        internal void AssignTransaction(SqlTransaction sqlTransaction)
        {
            throw new NotImplementedException();
        }
    }
}