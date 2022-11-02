using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{

    [Serializable]
    public class StockTransferDetailData
    {
        public string Warehouse = string.Empty;

        public string RegType = string.Empty;

        public string Till = string.Empty;

        public string Operation = string.Empty;

        public string OpsDate = string.Empty;

        public string ReceiverWH = string.Empty;

        public int Line = 0;

        public string Model = string.Empty;

        public string Color = string.Empty;

        public string Size = string.Empty;

        public string SalesPrice = string.Empty;

        public string Sign = string.Empty;

        public string Quantity = string.Empty;

        public string ModelTariff = string.Empty;

        public string OpsType = string.Empty;

        internal void AssignTransaction(SqlTransaction sqlTransaction)
        {
            throw new NotImplementedException();
        }
    }
}