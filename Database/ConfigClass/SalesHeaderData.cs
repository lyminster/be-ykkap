using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.ConfigClass
{
    [Serializable]
    public class SalesHeaderData
    {
        public string store = string.Empty;

        public string sales = string.Empty;

        public string pos = string.Empty;

        public DateTime salesDate = Convert.ToDateTime("1900-01-01 00:00:00");

        public int rate = 0;

        public string currency = string.Empty;

        public decimal totalGrossAmt = 0m;

        public decimal totalGrossAmtex = 0m;

        public decimal totalDisc1 = 0m;

        public decimal totalDisc1Ex = 0m;

        public decimal totalDisc2 = 0m;

        public decimal totalDisc2Ex = 0m;

        public decimal totalNetAmt = 0m;

        public decimal totalNetAmtEx = 0m;

        public string reference = string.Empty;

        public string refManual = string.Empty;

        public string customer = string.Empty;

        public int deposit = 0;

        public int returnType = 0;

        public int printed = 0;

        public int pluexist = 0;

        public ArrayList SalesDetails = null;

        public ArrayList SalesPayments = null;

        public ArrayList SalesTax = null;

        public KeyFICHData KeyFICH;
    }
}