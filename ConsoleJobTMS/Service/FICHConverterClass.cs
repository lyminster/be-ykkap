using Database.ConfigClass;
using Database.Manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleJobTMS.Service
{
    public class FICHConverterClass
    {


        protected ConnectionStringSettings currentString;

        protected Database.ConfigClass.Database.DBConnection currentConnection;

        protected Database.ConfigClass.Database.DBTransaction currentTransaction;

        protected CurrentDatabase CurrentDatabase;

        private configData ConfigDat = new configData();

        private XmlDocument xmlDoc = new XmlDocument();

        private StreamWriter objErrWriter;

        private string strErr = string.Empty;

        private string strCurrentErr = string.Empty;

        protected FICHFileDataManager objFICHFile;

        public string lblFile { get; set; }

        protected StockTransferHeaderManager objTransferHeader;

        protected StockTransferDetailManager objTransferDetail;

        protected StockReceiveHeaderManager objReceiveHeader;

        protected StockReceiveDetailManager objReceiveDetail;

        protected SalesLineManager objSalesLine;

        protected TotalSalesManager objTotalSales;

        protected PaymentLineManager objPaymentLine;

        protected PaymentLineCOMManager objPaymentLineCOM;

        protected CancelLineManager objCancelLine;

        protected ReturLineManager objReturLine;

        protected SpecialArticleManager objAlteration;

        protected SalesHeaderManager objSlsHeader;

        protected SalesDetailManager objSlsDetail;

        protected SalesPaymentManager objSlsPayment;

        protected HistoryMovHeaderManager objHistoryMovHeader;

        protected HistoryMovDetailManager objHistoryMovDetail;


        public List<string> lstLog { get; set; }
        private void p_WriteLog(string text, bool ClearLog)
        {
            Console.WriteLine(text);
            string empty = string.Empty;
            empty = DateTime.Now.ToString("yyyyMMdd") + " " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
            if (ClearLog)
            {
                lstLog = new List<string>();
            }
            lstLog.Add(empty + " >>" + text);
        }

        private void Run()
        {
            p_WriteLog(" Check ARJ file(s)", ClearLog: false);
            ArrayList arrayList = new ArrayList();
            if (Directory.GetFiles(ConfigDat.SourceFTP).Length != 0)
            {
                arrayList = clsUtility.GetZipFiles(ConfigDat.SourceFTP, "ARJ");
                foreach (FileInfo item in arrayList)
                {
                    UnzipARJ(item);
                }
            }
            else
            {
                p_WriteLog(" No ARJ found in " + ConfigDat.SourceFTP, ClearLog: false);
            }
            ArrayList arrayList2 = new ArrayList();
            arrayList2 = clsUtility.GetFICHFiles(ConfigDat.SourcePath);
            bool flag = false;
            if (arrayList2.Count == 0)
            {
                p_WriteLog(" No FICH333 file found", ClearLog: false);
                openConn();
                reUpdateMissingFICHPLU();
                closeConn();
                openConn();
                reProcessCancelGIFTFlagUpdate();
                closeConn();
                openConn();
                reProcessDoubleTsIFlagUpdate();
                closeConn();
                openConn();
                ReProcessTsO();
                closeConn();
                openConn();
                ReProcessTsOGift();
                closeConn();
                openConn();
                ReProcessTsI();
                closeConn();
                openConn();
                ReProcessSalesFlagNol();
                closeConn();
                return;
            }
            openConn();
            reUpdateMissingFICHPLU();
            closeConn();
            foreach (FileInfo item2 in arrayList2)
            {
                string name = item2.Name;
                string text = name.Substring(0, 8);
                lblFile = "File Name : " + item2.Name;
                openConn();
                if (text.Contains("FICH333C"))
                {
                    ReadFICHCOM(item2);
                }
                else if (text.Contains("FICH333"))
                {
                    ReadFICH(item2);
                }
                closeConn();
            }
            openConn();
            reUpdateMissingFICHPLU();
            closeConn();
            openConn();
            reProcessCancelGIFTFlagUpdate();
            closeConn();
            openConn();
            reProcessDoubleTsIFlagUpdate();
            closeConn();
            openConn();
            ReProcessTsO();
            closeConn();
            openConn();
            ReProcessTsOGift();
            closeConn();
            openConn();
            ReProcessTsI();
            closeConn();
            openConn();
            ReProcessSalesFlagNol();
            closeConn();
        }

        private void UnzipARJ(FileInfo fi)
        {
            int num = 0;
            if (!clsUtility.clsFolder.CreateTempFolder(Path.GetDirectoryName(fi.FullName), fi.Name) || !p_Unzip(fi.FullName, SESSION.CurrentTempFolder))
            {
                return;
            }
            ArrayList arrayList = new ArrayList();
            arrayList = clsUtility.GetFICHFiles(SESSION.CurrentTempFolder);
            string remarkARJ = "";
            if (arrayList.Count == 0)
            {
                p_WriteLog(" No FICH333 file(s) found in file  : " + fi.Name, ClearLog: false);
                remarkARJ = "_NO_TRX";
            }
            else
            {
                foreach (FileInfo item in arrayList)
                {
                    string name = item.Name;
                    string text = name.Substring(0, 8);
                    lblFile = "File Name : " + fi.Name;
                    string aRJName = fi.Name.Substring(0, 8);
                    remarkARJ = "";
                    if (text.Contains("FICH331."))
                    {
                        File.Delete(name);
                    }
                    else
                    {
                        clsUtility.clsFolder.RenameFICH(item, aRJName, ConfigDat.SourcePath, text);
                    }
                }
            }
            clsUtility.clsFolder.ClearTempFolder();
            clsUtility.clsFolder.SendARJ2Backup(fi, ConfigDat.BackupFTP, remarkARJ);
        }

        private void ReadFICH(FileInfo fi)
        {
            ArrayList arrayList = new ArrayList();
            arrayList = clsUtility.GetFICHFiles(ConfigDat.SourcePath);
            foreach (FileInfo item in arrayList)
            {
                string text = "Processing file : " + item.Name;
                lblFile = text;
                if (ProcessFICH(item, fi))
                {
                    text += " is SUCCESS.";
                    clsUtility.clsFolder.Send2Backup(item, ConfigDat.BackupPath);
                    p_WriteLog(" Send file : " + item.Name + " to backup folder.", ClearLog: false);
                }
                else
                {
                    text += " is FAILED.";
                    clsUtility.clsFolder.Send2Trash(item, ConfigDat.TrashPath);
                    p_WriteLog(" Send file : " + item.Name + " to trash folder.", ClearLog: false);
                }
                p_WriteLog(text, ClearLog: false);
            }
        }

        private bool ProcessFICH(FileInfo fi, FileInfo fiZip)
        {
            string value = (ConfigDat.ReduceDecimal = clsUtility.ReadXML("ReduceDecimal"));
            FICHFileData fICHFileData = new FICHFileData();
            SESSION.FileName = fi.Name;
            SESSION.FullName = fi.FullName;
            SESSION.UserName = "SYS";
            using (FileStream stream = new FileStream(SESSION.FullName, FileMode.Open))
            {
                using StreamReader streamReader = new StreamReader(stream);
                p_WriteLog(" Starting reading file " + fi.Name, ClearLog: false);
                string empty = string.Empty;
                while ((empty = streamReader.ReadLine()) != null)
                {
                    if (!(empty.Substring(22, 1) == "R") && !(empty.Substring(22, 1) == "F") && !(empty.Substring(22, 1) == "O") && !(empty.Substring(22, 1) == "L") && !(empty.Substring(22, 1) == "T") && !(empty.Substring(22, 1) == "N") && !(empty.Substring(22, 1) == "d"))
                    {
                        continue;
                    }
                    if (empty.Substring(22, 1) == "R")
                    {
                        StockTransferDetailData stockTransferDetailData = new StockTransferDetailData();
                        stockTransferDetailData.Warehouse = empty.ToString().Substring(0, 4);
                        stockTransferDetailData.RegType = empty.ToString().Substring(22, 1);
                        stockTransferDetailData.Till = empty.ToString().Substring(125, 1);
                        stockTransferDetailData.Operation = empty.ToString().Substring(5, 7);
                        stockTransferDetailData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        stockTransferDetailData.ReceiverWH = empty.ToString().Substring(63, 4);
                        stockTransferDetailData.Model = empty.ToString().Substring(23, 8);
                        stockTransferDetailData.Color = empty.ToString().Substring(34, 2);
                        stockTransferDetailData.Size = empty.ToString().Substring(32, 2);
                        stockTransferDetailData.SalesPrice = empty.ToString().Substring(46, 12 - Convert.ToInt16(value));
                        stockTransferDetailData.Sign = empty.ToString().Substring(40, 1);
                        stockTransferDetailData.Quantity = empty.ToString().Substring(41, 4);
                        stockTransferDetailData.ModelTariff = empty.ToString().Substring(82, 2);
                        stockTransferDetailData.OpsType = empty.ToString().Substring(96, 1);
                        fICHFileData.resultStockTransferDetail.Add(stockTransferDetailData);
                    }
                    if (empty.Substring(22, 1) == "F")
                    {
                        StockTransferHeaderData stockTransferHeaderData = new StockTransferHeaderData();
                        stockTransferHeaderData.Warehouse = empty.ToString().Substring(0, 4);
                        stockTransferHeaderData.RegType = empty.ToString().Substring(22, 1);
                        stockTransferHeaderData.Till = empty.ToString().Substring(125, 1);
                        stockTransferHeaderData.Operation = empty.ToString().Substring(5, 7);
                        stockTransferHeaderData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        stockTransferHeaderData.OpsTime = empty.ToString().Substring(18, 4);
                        stockTransferHeaderData.ReceiverWH = empty.ToString().Substring(63, 4);
                        stockTransferHeaderData.Sign = empty.ToString().Substring(40, 1);
                        stockTransferHeaderData.TotalQty = empty.ToString().Substring(41, 4);
                        fICHFileData.resultHeader.Add(stockTransferHeaderData);
                    }
                    if (empty.Substring(22, 1) == "O")
                    {
                        StockReceiveHeaderData stockReceiveHeaderData = new StockReceiveHeaderData();
                        stockReceiveHeaderData.Warehouse = empty.ToString().Substring(0, 4);
                        stockReceiveHeaderData.RegType = empty.ToString().Substring(22, 1);
                        stockReceiveHeaderData.Till = empty.ToString().Substring(124, 2);
                        stockReceiveHeaderData.Operation = empty.ToString().Substring(23, 7);
                        stockReceiveHeaderData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        stockReceiveHeaderData.OpsTime = empty.ToString().Substring(18, 4);
                        stockReceiveHeaderData.SenderWH = empty.ToString().Substring(63, 4);
                        stockReceiveHeaderData.isTariffProcessed = empty.ToString().Substring(98, 1);
                        stockReceiveHeaderData.isExport = empty.ToString().Substring(99, 1);
                        stockReceiveHeaderData.Model = empty.ToString().Substring(31, 8);
                        stockReceiveHeaderData.Color = empty.ToString().Substring(41, 2);
                        stockReceiveHeaderData.Size = empty.ToString().Substring(39, 2);
                        stockReceiveHeaderData.ReceiveQty = empty.ToString().Substring(52, 5);
                        fICHFileData.resultStockReceiveHeader.Add(stockReceiveHeaderData);
                    }
                    if (empty.Substring(22, 1) == "O")
                    {
                        StockReceiveDetailData stockReceiveDetailData = new StockReceiveDetailData();
                        stockReceiveDetailData.Warehouse = empty.ToString().Substring(0, 4);
                        stockReceiveDetailData.RegType = empty.ToString().Substring(22, 1);
                        stockReceiveDetailData.Operation = empty.ToString().Substring(23, 7);
                        stockReceiveDetailData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        stockReceiveDetailData.SenderWH = empty.ToString().Substring(63, 4);
                        stockReceiveDetailData.Model = empty.ToString().Substring(31, 8);
                        stockReceiveDetailData.Color = empty.ToString().Substring(41, 2);
                        stockReceiveDetailData.Size = empty.ToString().Substring(39, 2);
                        stockReceiveDetailData.SendQty = empty.ToString().Substring(44, 6);
                        stockReceiveDetailData.Sign = empty.ToString().Substring(50, 1);
                        stockReceiveDetailData.ReceiveQty = empty.ToString().Substring(52, 5);
                        stockReceiveDetailData.DelNoteStatus = empty.ToString().Substring(30, 1);
                        stockReceiveDetailData.Operador = empty.ToString().Substring(57, 6);
                        stockReceiveDetailData.BoxCondition = empty.ToString().Substring(67, 1);
                        fICHFileData.resultStockReceiveDetail.Add(stockReceiveDetailData);
                    }
                    if (empty.Substring(22, 1) == "L")
                    {
                        SalesLineData salesLineData = new SalesLineData();
                        salesLineData.Warehouse = empty.ToString().Substring(0, 4);
                        salesLineData.RegType = empty.ToString().Substring(22, 1);
                        salesLineData.Till = empty.ToString().Substring(125, 1);
                        salesLineData.Operation = empty.ToString().Substring(4, 8);
                        salesLineData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        salesLineData.OpsTime = empty.ToString().Substring(18, 4);
                        salesLineData.Model = empty.ToString().Substring(23, 8);
                        salesLineData.Color = empty.ToString().Substring(34, 2);
                        salesLineData.Size = empty.ToString().Substring(32, 2);
                        salesLineData.SalesPrice = empty.ToString().Substring(46, 12 - Convert.ToInt16(value));
                        salesLineData.Discount = empty.ToString().Substring(59, 2);
                        salesLineData.Sign = empty.ToString().Substring(40, 1);
                        salesLineData.Quantity = empty.ToString().Substring(41, 4);
                        salesLineData.SalesAmount = empty.ToString().Substring(68, 14 - Convert.ToInt16(value));
                        salesLineData.ModelTariff = empty.ToString().Substring(82, 2);
                        salesLineData.SalesPerson = empty.ToString().Substring(61, 6);
                        salesLineData.ReturnType = empty.ToString().Substring(84, 1);
                        salesLineData.OnlineOrderID = empty.ToString().Substring(86, 8);
                        salesLineData.OpsType = empty.ToString().Substring(96, 1);
                        salesLineData.Cancellation = empty.ToString().Substring(100, 1);
                        fICHFileData.resultSalesLine.Add(salesLineData);
                    }
                    if (empty.Substring(22, 1) == "T")
                    {
                        TotalSalesData totalSalesData = new TotalSalesData();
                        totalSalesData.Warehouse = empty.ToString().Substring(0, 4);
                        totalSalesData.RegType = empty.ToString().Substring(22, 1);
                        totalSalesData.Till = empty.ToString().Substring(125, 1);
                        totalSalesData.Operation = empty.ToString().Substring(4, 8);
                        totalSalesData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        totalSalesData.OpsTime = empty.ToString().Substring(18, 4);
                        totalSalesData.PrintedSummary = empty.ToString().Substring(102, 1);
                        totalSalesData.Updated = empty.ToString().Substring(103, 1);
                        totalSalesData.StaffCode = empty.ToString().Substring(104, 12);
                        totalSalesData.ReturnType = empty.ToString().Substring(100, 1);
                        totalSalesData.Status = empty.ToString().Substring(101, 1);
                        fICHFileData.resultTotalSales.Add(totalSalesData);
                    }
                    if (empty.Substring(22, 1) == "T")
                    {
                        PaymentLineData paymentLineData = new PaymentLineData();
                        paymentLineData.Warehouse = empty.ToString().Substring(0, 4);
                        paymentLineData.RegType = empty.ToString().Substring(22, 1);
                        paymentLineData.Till = empty.ToString().Substring(125, 1);
                        paymentLineData.Operation = empty.ToString().Substring(4, 8);
                        paymentLineData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        paymentLineData.OpsTime = empty.ToString().Substring(18, 4);
                        paymentLineData.ClaveTC = empty.ToString().Substring(98, 2);
                        paymentLineData.SignCheque = empty.ToString().Substring(23, 1);
                        paymentLineData.ChequeAmt = empty.ToString().Substring(24, 14 - Convert.ToInt16(value));
                        paymentLineData.SignDebit = empty.ToString().Substring(38, 1);
                        paymentLineData.DebitAmt = empty.ToString().Substring(39, 14 - Convert.ToInt16(value));
                        paymentLineData.SignCash = empty.ToString().Substring(53, 1);
                        paymentLineData.CashAmt = empty.ToString().Substring(54, 14 - Convert.ToInt16(value));
                        paymentLineData.SignVouch = empty.ToString().Substring(68, 1);
                        paymentLineData.VoucherAmt = empty.ToString().Substring(69, 14 - Convert.ToInt16(value));
                        paymentLineData.SignCredit = empty.ToString().Substring(83, 1);
                        paymentLineData.CreditAmt = empty.ToString().Substring(84, 14 - Convert.ToInt16(value));
                        fICHFileData.resultPaymentLine.Add(paymentLineData);
                    }
                    if (empty.Substring(22, 1) == "N")
                    {
                        CancelLineData cancelLineData = new CancelLineData();
                        cancelLineData.Warehouse = empty.ToString().Substring(0, 4);
                        cancelLineData.RegType = empty.ToString().Substring(22, 1);
                        cancelLineData.Till = empty.ToString().Substring(125, 1);
                        cancelLineData.Operation = empty.ToString().Substring(4, 8);
                        cancelLineData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        cancelLineData.OpsTime = empty.ToString().Substring(18, 4);
                        cancelLineData.Model = empty.ToString().Substring(23, 8);
                        cancelLineData.Color = empty.ToString().Substring(34, 2);
                        cancelLineData.Size = empty.ToString().Substring(32, 2);
                        cancelLineData.SalesPrice = empty.ToString().Substring(46, 12 - Convert.ToInt16(value));
                        cancelLineData.Discount = empty.ToString().Substring(59, 2);
                        cancelLineData.Sign = empty.ToString().Substring(40, 1);
                        cancelLineData.Quantity = empty.ToString().Substring(41, 4);
                        cancelLineData.SalesAmount = empty.ToString().Substring(68, 14 - Convert.ToInt16(value));
                        cancelLineData.ModelTariff = empty.ToString().Substring(82, 2);
                        cancelLineData.SalesPerson = empty.ToString().Substring(61, 6);
                        cancelLineData.ReturnType = empty.ToString().Substring(84, 1);
                        cancelLineData.CustomerID = empty.ToString().Substring(86, 8);
                        cancelLineData.OpsType = empty.ToString().Substring(96, 1);
                        cancelLineData.Cancellation = empty.ToString().Substring(100, 1);
                        fICHFileData.resultCancelLine.Add(cancelLineData);
                    }
                    if (empty.Substring(22, 1) == "d")
                    {
                        ReturLineData returLineData = new ReturLineData();
                        returLineData.Warehouse = empty.ToString().Substring(0, 4);
                        returLineData.RegType = empty.ToString().Substring(22, 1);
                        returLineData.Till = empty.ToString().Substring(125, 1);
                        returLineData.Operation = empty.ToString().Substring(4, 8);
                        returLineData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        returLineData.OpsTime = empty.ToString().Substring(18, 4);
                        returLineData.OriWH = empty.ToString().Substring(23, 4);
                        if (empty.ToString().Substring(27, 6) != "000000")
                        {
                            returLineData.OriOpsDate = DateTime.ParseExact(empty.ToString().Substring(27, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            returLineData.OriOpsDate = "000101";
                        }
                        returLineData.OriOperation = empty.ToString().Substring(33, 8);
                        returLineData.OriQty = empty.ToString().Substring(45, 2);
                        returLineData.OriClaveTC = empty.ToString().Substring(41, 2);
                        returLineData.isDiscount = empty.ToString().Substring(43, 1);
                        returLineData.isPromotion = empty.ToString().Substring(44, 1);
                        returLineData.isEANScan = empty.ToString().Substring(47, 1);
                        fICHFileData.resultReturLine.Add(returLineData);
                    }
                }
            }
            p_WriteLog(" End reading FICH files", ClearLog: false);
            return InsertToMNGFICH(fICHFileData);
        }

        private void ReadFICHCOM(FileInfo fi)
        {
            ArrayList arrayList = new ArrayList();
            arrayList = clsUtility.GetFICHCOMFiles(ConfigDat.SourcePath);
            foreach (FileInfo item in arrayList)
            {
                string text = " Processing file : " + item.Name;
                lblFile = text;
                if (ProcessFICHCOM(item, fi))
                {
                    text += " is SUCCESS.";
                    clsUtility.clsFolder.Send2Backup(item, ConfigDat.BackupPath);
                    p_WriteLog(" Send file : " + item.Name + " to backup folder.", ClearLog: false);
                }
                else
                {
                    text += " is FAILED.";
                    clsUtility.clsFolder.Send2Trash(item, ConfigDat.TrashPath);
                    p_WriteLog(" Send file : " + item.Name + " to trash folder.", ClearLog: false);
                }
                p_WriteLog(text, ClearLog: false);
            }
        }

        private bool ProcessFICHCOM(FileInfo fi, FileInfo fiZip)
        {
            string value = (ConfigDat.ReduceDecimal = clsUtility.ReadXML("ReduceDecimal"));
            FICHFileData fICHFileData = new FICHFileData();
            SESSION.FileName = fi.Name;
            SESSION.FullName = fi.FullName;
            SESSION.UserName = "SYS";
            using (FileStream stream = new FileStream(SESSION.FullName, FileMode.Open))
            {
                using StreamReader streamReader = new StreamReader(stream);
                p_WriteLog(" Start reading eCOMM FICH file " + fi.Name, ClearLog: false);
                string empty = string.Empty;
                while ((empty = streamReader.ReadLine()) != null && empty.Length >= 10)
                {
                    if (empty.Substring(22, 1) == "L" || empty.Substring(22, 1) == "T" || empty.Substring(22, 3) == "CMK")
                    {
                        if (empty.Substring(22, 1) == "L")
                        {
                            SalesLineData salesLineData = new SalesLineData();
                            salesLineData.Warehouse = empty.ToString().Substring(0, 4);
                            salesLineData.RegType = empty.ToString().Substring(22, 1);
                            salesLineData.Till = empty.ToString().Substring(125, 1);
                            salesLineData.Operation = empty.ToString().Substring(4, 8);
                            salesLineData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                            salesLineData.OpsTime = empty.ToString().Substring(18, 4);
                            salesLineData.Model = empty.ToString().Substring(23, 8);
                            salesLineData.Color = empty.ToString().Substring(34, 2);
                            salesLineData.Size = empty.ToString().Substring(32, 2);
                            salesLineData.SalesPrice = empty.ToString().Substring(46, 12 - Convert.ToInt16(value));
                            salesLineData.Discount = empty.ToString().Substring(59, 2);
                            salesLineData.Sign = empty.ToString().Substring(40, 1);
                            salesLineData.Quantity = empty.ToString().Substring(41, 4);
                            salesLineData.SalesAmount = empty.ToString().Substring(68, 14 - Convert.ToInt16(value));
                            salesLineData.ModelTariff = empty.ToString().Substring(82, 2);
                            salesLineData.SalesPerson = empty.ToString().Substring(61, 6);
                            salesLineData.ReturnType = empty.ToString().Substring(84, 1);
                            salesLineData.OnlineOrderID = empty.ToString().Substring(86, 8);
                            salesLineData.OpsType = empty.ToString().Substring(96, 1);
                            salesLineData.Cancellation = empty.ToString().Substring(100, 1);
                            fICHFileData.resultSalesLine.Add(salesLineData);
                        }
                        if (empty.Substring(22, 1) == "T")
                        {
                            TotalSalesData totalSalesData = new TotalSalesData();
                            totalSalesData.Warehouse = empty.ToString().Substring(0, 4);
                            totalSalesData.RegType = empty.ToString().Substring(22, 1);
                            totalSalesData.Till = empty.ToString().Substring(125, 1);
                            totalSalesData.Operation = empty.ToString().Substring(4, 8);
                            totalSalesData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                            totalSalesData.OpsTime = empty.ToString().Substring(18, 4);
                            totalSalesData.PrintedSummary = empty.ToString().Substring(102, 1);
                            totalSalesData.Updated = empty.ToString().Substring(103, 1);
                            totalSalesData.StaffCode = empty.ToString().Substring(104, 12);
                            totalSalesData.ReturnType = empty.ToString().Substring(100, 1);
                            totalSalesData.Status = empty.ToString().Substring(101, 1);
                            fICHFileData.resultTotalSales.Add(totalSalesData);
                        }
                        if (empty.Substring(22, 3) == "CMK")
                        {
                            PaymentLineCOMData paymentLineCOMData = new PaymentLineCOMData();
                            paymentLineCOMData.Warehouse = empty.ToString().Substring(0, 4);
                            paymentLineCOMData.RegType = empty.ToString().Substring(22, 3);
                            paymentLineCOMData.Till = empty.ToString().Substring(125, 1);
                            paymentLineCOMData.Operation = empty.ToString().Substring(4, 8);
                            paymentLineCOMData.OpsDate = DateTime.ParseExact(empty.ToString().Substring(12, 6), "yyMMdd", CultureInfo.CurrentCulture).ToString("yyyy-MM-dd");
                            paymentLineCOMData.OpsTime = empty.ToString().Substring(18, 4);
                            paymentLineCOMData.ClaveTC = empty.ToString().Substring(34, 2);
                            paymentLineCOMData.OnlineOrderID = empty.ToString().Substring(26, 8);
                            fICHFileData.resultPaymentLineCOM.Add(paymentLineCOMData);
                        }
                    }
                }
            }
            p_WriteLog(" End reading eCOMM FICH files", ClearLog: false);
            return InsertToMNGFICHCOM(fICHFileData);
        }

        private bool InsertToMNGFICH(FICHFileData FICHFileDat)
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                objFICHFile.AssignTransaction(currentTransaction.ThrowTransaction());
                objTransferDetail.AssignTransaction(currentTransaction.ThrowTransaction());
                objTransferHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                objReceiveHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                objReceiveDetail.AssignTransaction(currentTransaction.ThrowTransaction());
                objSalesLine.AssignTransaction(currentTransaction.ThrowTransaction());
                objTotalSales.AssignTransaction(currentTransaction.ThrowTransaction());
                objPaymentLine.AssignTransaction(currentTransaction.ThrowTransaction());
                objCancelLine.AssignTransaction(currentTransaction.ThrowTransaction());
                objReturLine.AssignTransaction(currentTransaction.ThrowTransaction());
                if (objFICHFile.IsFICHFileExist(SESSION.FileName))
                {
                    p_WriteLog("File :" + SESSION.FileName + " already processing before.", ClearLog: false);
                    currentTransaction.ExecuteTransaction(bCommit: false);
                    return false;
                }
                p_WriteLog(" Start posting FICH file", ClearLog: false);
                if (FICHFileDat.resultHeader.Count != 0)
                {
                    p_WriteLog("> Posting Stock Transfer Header", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultHeader.Count; i++)
                    {
                        StockTransferHeaderData stockTransferHeaderDat = (StockTransferHeaderData)FICHFileDat.resultHeader[i];
                        objTransferHeader.TransferHeaderInsert(stockTransferHeaderDat);
                    }
                }
                if (FICHFileDat.resultStockTransferDetail.Count != 0)
                {
                    p_WriteLog("> Posting Stock Transfer Detail", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultStockTransferDetail.Count; i++)
                    {
                        StockTransferDetailData stockTransferDetailData = (StockTransferDetailData)FICHFileDat.resultStockTransferDetail[i];
                        stockTransferDetailData.Line = objTransferDetail.TransferDetailCount(stockTransferDetailData) + 1;
                        objTransferDetail.TransferDetailInsert(stockTransferDetailData);
                    }
                }
                if (FICHFileDat.resultStockReceiveHeader.Count != 0)
                {
                    p_WriteLog("> Posting Stock Receive Header", ClearLog: false);
                    StockReceiveHeaderData stockReceiveHeaderDat = null;
                    for (int i = 0; i < FICHFileDat.resultStockReceiveHeader.Count; i++)
                    {
                        stockReceiveHeaderDat = (StockReceiveHeaderData)FICHFileDat.resultStockReceiveHeader[i];
                        objReceiveHeader.InsertTempReceiveHeader(stockReceiveHeaderDat);
                    }
                    objReceiveHeader.InsertReceiveHeader(stockReceiveHeaderDat);
                }
                if (FICHFileDat.resultStockReceiveDetail.Count != 0)
                {
                    p_WriteLog("> Posting Stock Receive Detail", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultStockReceiveDetail.Count; i++)
                    {
                        StockReceiveDetailData stockReceiveDetailData = (StockReceiveDetailData)FICHFileDat.resultStockReceiveDetail[i];
                        stockReceiveDetailData.Line = objReceiveDetail.ReceiveDetailCount(stockReceiveDetailData) + 1;
                        objReceiveDetail.ReceiveDetailInsert(stockReceiveDetailData);
                    }
                }
                if (FICHFileDat.resultSalesLine.Count != 0)
                {
                    p_WriteLog("> Posting Sales Line", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultSalesLine.Count; i++)
                    {
                        SalesLineData salesLineData = (SalesLineData)FICHFileDat.resultSalesLine[i];
                        salesLineData.Line = objSalesLine.SalesLineCount(salesLineData) + 1;
                        objSalesLine.SalesLineInsert(salesLineData);
                    }
                }
                if (FICHFileDat.resultTotalSales.Count != 0)
                {
                    p_WriteLog("> Posting Total Sales", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultTotalSales.Count; i++)
                    {
                        TotalSalesData totalSalesDat = (TotalSalesData)FICHFileDat.resultTotalSales[i];
                        objTotalSales.TotalSalesInsert(totalSalesDat);
                    }
                }
                if (FICHFileDat.resultPaymentLine.Count != 0)
                {
                    p_WriteLog("> Posting Payment Line", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultPaymentLine.Count; i++)
                    {
                        PaymentLineData paymentLineData = (PaymentLineData)FICHFileDat.resultPaymentLine[i];
                        paymentLineData.Line = objPaymentLine.PaymentLineCount(paymentLineData) + 1;
                        objPaymentLine.PaymentLineInsert(paymentLineData);
                    }
                }
                if (FICHFileDat.resultCancelLine.Count != 0)
                {
                    p_WriteLog("> Posting Cancel Line", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultCancelLine.Count; i++)
                    {
                        CancelLineData cancelLineData = (CancelLineData)FICHFileDat.resultCancelLine[i];
                        cancelLineData.Line = objCancelLine.CancelLineCount(cancelLineData) + 1;
                        objCancelLine.CancelLineInsert(cancelLineData);
                    }
                }
                if (FICHFileDat.resultReturLine.Count != 0)
                {
                    p_WriteLog("> Posting Retur Line", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultReturLine.Count; i++)
                    {
                        ReturLineData returLineDat = (ReturLineData)FICHFileDat.resultReturLine[i];
                        objReturLine.ReturLineInsert(returLineDat);
                    }
                }
                currentTransaction.ExecuteTransaction(bCommit: true);
                FICHFileDat = null;
                p_WriteLog(" End posting FICH file", ClearLog: false);
                objFICHFile.InsertFile2History(SESSION.FileName);
                reUpdateFICHPLU(currentTransaction);
                ProcessCancelGIFTFlagUpdate(currentTransaction);
                SyncTsO2ISS(currentTransaction);
                SyncTsI2ISS(currentTransaction);
                SyncSales2ISS(currentTransaction);
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
        }

        private bool InsertToMNGFICHCOM(FICHFileData FICHFileDat)
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                objFICHFile.AssignTransaction(currentTransaction.ThrowTransaction());
                objSalesLine.AssignTransaction(currentTransaction.ThrowTransaction());
                objTotalSales.AssignTransaction(currentTransaction.ThrowTransaction());
                objPaymentLine.AssignTransaction(currentTransaction.ThrowTransaction());
                objPaymentLineCOM.AssignTransaction(currentTransaction.ThrowTransaction());
                if (objFICHFile.IsFICHFileExist(SESSION.FileName))
                {
                    p_WriteLog("File :" + SESSION.FileName + " already processing before.", ClearLog: false);
                    currentTransaction.ExecuteTransaction(bCommit: false);
                    return false;
                }
                p_WriteLog("Start posting eCOM FICH file", ClearLog: false);
                if (FICHFileDat.resultSalesLine.Count != 0)
                {
                    p_WriteLog(">> Posting eCOM Sales Line", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultSalesLine.Count; i++)
                    {
                        SalesLineData salesLineData = (SalesLineData)FICHFileDat.resultSalesLine[i];
                        salesLineData.Line = objSalesLine.SalesLineCount(salesLineData) + 1;
                        objSalesLine.SalesLineInsert(salesLineData);
                    }
                }
                if (FICHFileDat.resultTotalSales.Count != 0)
                {
                    p_WriteLog("> Posting eCOM Total Sales", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultTotalSales.Count; i++)
                    {
                        TotalSalesData totalSalesDat = (TotalSalesData)FICHFileDat.resultTotalSales[i];
                        objTotalSales.TotalSalesInsert(totalSalesDat);
                    }
                }
                if (FICHFileDat.resultPaymentLineCOM.Count != 0)
                {
                    p_WriteLog(">> Posting Payment Line COM", ClearLog: false);
                    for (int i = 0; i < FICHFileDat.resultPaymentLineCOM.Count; i++)
                    {
                        PaymentLineCOMData paymentLineCOMData = (PaymentLineCOMData)FICHFileDat.resultPaymentLineCOM[i];
                        paymentLineCOMData.Line = objPaymentLineCOM.PaymentLineCOMCount(paymentLineCOMData) + 1;
                        objPaymentLineCOM.PaymentLineCOMInsert(paymentLineCOMData);
                    }
                }
                p_WriteLog("> Posting eCOM Payment", ClearLog: false);
                PaymentLineData paymentLineDat = null;
                objPaymentLine.eCOMPaymentInsert(paymentLineDat);
                currentTransaction.ExecuteTransaction(bCommit: true);
                FICHFileDat = null;
                p_WriteLog(" End posting eCOM FICH file", ClearLog: false);
                objFICHFile.InsertFile2History(SESSION.FileName);
                reUpdateFICHPLU(currentTransaction);
                ProcessCancelGIFTFlagUpdate(currentTransaction);
                SyncTsO2ISS(currentTransaction);
                SyncTsI2ISS(currentTransaction);
                SyncSales2ISS(currentTransaction);
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
        }

        private bool reUpdateMissingFICHPLU()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (reUpdateFICHPLU(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool reUpdateFICHPLU(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objFICHFile.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Updating FICH PLU", ClearLog: false);
                flag = objFICHFile.UpdateFICHPLU();

                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool reProcessCancelGIFTFlagUpdate()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (ProcessCancelGIFTFlagUpdate(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool ProcessCancelGIFTFlagUpdate(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objFICHFile.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Flag Cancel GIFT", ClearLog: false);
                flag = objSlsHeader.CancelGIFTFlagUpdate();

                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool ProcessDoubleTsIFlagUpdate(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objReceiveHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Flag Double TsI Operation", ClearLog: false);
                flag = objReceiveHeader.DoubleTsIFlagUpdate();

                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool reProcessDoubleTsIFlagUpdate()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (ProcessDoubleTsIFlagUpdate(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool ReProcessSalesFlagNol()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (SyncSales2ISS(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool ReProcessTsO()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (SyncTsO2ISS(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool ReProcessTsOGift()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (SyncTsOGift2ISS(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool ReProcessTsI()
        {
            currentTransaction = new Database.ConfigClass.Database.DBTransaction();
            currentTransaction.FetchTransaction(CurrentDatabase.CurrentConnection.StartTransaction());
            try
            {
                if (SyncTsI2ISS(currentTransaction))
                {
                    currentTransaction.ExecuteTransaction(bCommit: true);
                }
                return true;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                currentTransaction.ExecuteTransaction(bCommit: false);
                return false;
            }
            finally
            {
                currentTransaction = null;
            }
        }

        private bool SyncTsO2ISS(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objHistoryMovHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Start Posting Transfer Out to ISS", ClearLog: false);
                flag = objHistoryMovHeader.ProcessMovement_TsO();

                p_WriteLog(" Ending Posting Transfer Out to ISS", ClearLog: false);
                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool SyncTsOGift2ISS(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objHistoryMovHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Start Posting TsOut-Gift to ISS", ClearLog: false);
                flag = objHistoryMovHeader.ProcessMovement_TsOGift();

                p_WriteLog(" Ending Posting TsOut-Gift to ISS", ClearLog: false);
                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool SyncTsI2ISS(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            bool flag = false;
            currentTransaction = Transaction;
            try
            {
                objHistoryMovHeader.AssignTransaction(currentTransaction.ThrowTransaction());
                p_WriteLog(" Start Posting TsIn to ISS", ClearLog: false);
                flag = objHistoryMovHeader.ProcessMovement_TsI();

                p_WriteLog(" Ending Posting TsIn to ISS", ClearLog: false);
                return flag;
            }
            catch (Exception ex)
            {
                p_WriteLog(ex.Message, ClearLog: false);
                objErrWriter.Write(strCurrentErr);
                SendErrorLog();
                return false;
            }
        }

        private bool SyncSales2ISS(Database.ConfigClass.Database.DBTransaction Transaction)
        {
            currentTransaction = Transaction;
            objSlsHeader.AssignTransaction(currentTransaction.ThrowTransaction());
            objSlsDetail.AssignTransaction(currentTransaction.ThrowTransaction());
            objSlsPayment.AssignTransaction(currentTransaction.ThrowTransaction());
            p_WriteLog(" Start Insert Sales to ISS", ClearLog: false);
            ArrayList arrayList = objSlsHeader.FetchListSales();
            SalesHeaderData salesHeaderData = null;
            for (int i = 0; i < arrayList.Count; i++)
            {
                salesHeaderData = (SalesHeaderData)arrayList[i];
                if (!objSlsHeader.IsSalesExsist(salesHeaderData))
                {
                    p_WriteLog("> Insert Sales Store# : " + salesHeaderData.store + " | Sales# :" + salesHeaderData.sales + " (" + salesHeaderData.KeyFICH.Warehouse + ", " + salesHeaderData.KeyFICH.Till + ", " + salesHeaderData.KeyFICH.Operation + ") ", ClearLog: false);
                    objSlsHeader.InsertSalesHeader(salesHeaderData);
                    SalesDetailData salesDetailData = null;
                    for (int j = 0; j < salesHeaderData.SalesDetails.Count; j++)
                    {
                        salesDetailData = (SalesDetailData)salesHeaderData.SalesDetails[j];
                        objSlsDetail.Insert(salesDetailData);
                    }
                    SalesPaymentData salesPaymentData = null;
                    for (int k = 0; k < salesHeaderData.SalesPayments.Count; k++)
                    {
                        salesPaymentData = (SalesPaymentData)salesHeaderData.SalesPayments[k];
                        objSlsPayment.InsertSalesPayment(salesPaymentData);
                    }
                    objSlsHeader.UpdateSalesandReturFlag(salesHeaderData.KeyFICH);
                }
            }
            p_WriteLog(" End Posting Sales to ISS", ClearLog: false);
            arrayList = null;
            salesHeaderData = null;
            return true;
        }

        private void ReadConfig()
        {
            p_WriteLog(" Read file configuration", ClearLog: false);
            ConfigDat.AppName = clsUtility.ReadXML("AppName");
            ConfigDat.SourceFTP = clsUtility.ReadXML("SourceFTP");
            ConfigDat.SourcePath = clsUtility.ReadXML("SourcePath");
            ConfigDat.BackupPath = clsUtility.ReadXML("BackupPath");
            ConfigDat.TrashPath = clsUtility.ReadXML("TrashPath");
            ConfigDat.BackupFTP = clsUtility.ReadXML("BackupFTP");
            ConfigDat.UnZipperLoc = clsUtility.ReadXML("UnZipperLoc");
            ConfigDat.ReduceDecimal = clsUtility.ReadXML("ReduceDecimal");

            SESSION.parMail = new Database.ModelsClass.ParamMail();
            SESSION.parMail.sServerHost = clsUtility.ReadXML("sServerHost");
            SESSION.parMail.sUserCredential = clsUtility.ReadXML("sUserCredential");
            SESSION.parMail.sPasswordCredential = clsUtility.ReadXML("sPasswordCredential");
            SESSION.parMail.sSender = clsUtility.ReadXML("sSender");
            SESSION.parMail.sSenderAlias = clsUtility.ReadXML("sSenderAlias");
            SESSION.MailErrorLog = new Database.ModelsClass.zMail();
            SESSION.MailErrorLog.sSubject = clsUtility.ReadXML("sSubject_Err");
            SESSION.MailErrorLog.HeaderMessage = clsUtility.ReadXML("HeaderMessage_Err");
            SESSION.MailErrorLog.FooterMessage = clsUtility.ReadXML("FooterMessage_Err");
            SESSION.MailErrorLog.sReceiversTo = clsUtility.ReadXML("sReceiversTo_Err").Split(';');
            SESSION.MailErrorLog.sReceiversCC = clsUtility.ReadXML("sReceiversCC_Err").Split(';');
            SESSION.MailErrorLog.sReceiversBCC = clsUtility.ReadXML("sReceiversBCC_Err").Split(';');
            SESSION.MailArticle = new Database.ModelsClass.zMail();
            SESSION.MailArticle.sSubject = clsUtility.ReadXML("sSubject_art");
            SESSION.MailArticle.HeaderMessage = clsUtility.ReadXML("HeaderMessage_art");
            SESSION.MailArticle.FooterMessage = clsUtility.ReadXML("FooterMessage_art");
            SESSION.MailArticle.sReceiversTo = clsUtility.ReadXML("sReceiversTo_art").Split(';');
            SESSION.MailArticle.sReceiversCC = clsUtility.ReadXML("sReceiversCC_art").Split(';');
            SESSION.MailArticle.sReceiversBCC = clsUtility.ReadXML("sReceiversBCC_art").Split(';');
            SESSION.MailSales = new Database.ModelsClass.zMail();
            SESSION.MailSales.sSubject = clsUtility.ReadXML("sSubject_sls");
            SESSION.MailSales.HeaderMessage = clsUtility.ReadXML("HeaderMessage_sls");
            SESSION.MailSales.FooterMessage = clsUtility.ReadXML("FooterMessage_sls");
            SESSION.MailSales.sReceiversTo = clsUtility.ReadXML("sReceiversTo_sls").Split(';');
            SESSION.MailSales.sReceiversCC = clsUtility.ReadXML("sReceiversCC_sls").Split(';');
            SESSION.MailSales.sReceiversBCC = clsUtility.ReadXML("sReceiversBCC_sls").Split(';');
            SESSION.MailMovement = new Database.ModelsClass.zMail();
            SESSION.MailMovement.sSubject = clsUtility.ReadXML("sSubject_mvt");
            SESSION.MailMovement.HeaderMessage = clsUtility.ReadXML("HeaderMessage_mvt");
            SESSION.MailMovement.FooterMessage = clsUtility.ReadXML("FooterMessage_mvt");
            SESSION.MailMovement.sReceiversTo = clsUtility.ReadXML("sReceiversTo_mvt").Split(';');
            SESSION.MailMovement.sReceiversCC = clsUtility.ReadXML("sReceiversCC_mvt").Split(';');
            SESSION.MailMovement.sReceiversBCC = clsUtility.ReadXML("sReceiversBCC_mvt").Split(';');
        }

        private void setInit()
        {
            objFICHFile = new FICHFileDataManager();
            objTransferHeader = new StockTransferHeaderManager();
            objTransferDetail = new StockTransferDetailManager();
            objReceiveHeader = new StockReceiveHeaderManager();
            objReceiveDetail = new StockReceiveDetailManager();
            objSalesLine = new SalesLineManager();
            objTotalSales = new TotalSalesManager();
            objPaymentLine = new PaymentLineManager();
            objPaymentLineCOM = new PaymentLineCOMManager();
            objCancelLine = new CancelLineManager();
            objReturLine = new ReturLineManager();
            objSlsHeader = new SalesHeaderManager();
            objSlsDetail = new SalesDetailManager();
            objSlsPayment = new SalesPaymentManager();
            objHistoryMovHeader = new HistoryMovHeaderManager();
            objHistoryMovDetail = new HistoryMovDetailManager();
            objAlteration = new SpecialArticleManager();
        }

        private bool SendErrorLog()
        {
            openConn();
            bool flag = false;
            string empty = string.Empty;
            try
            {
                string empty2 = string.Empty;
                empty2 = SESSION.MailErrorLog.HeaderMessage + " \r\n \r\n";
                empty2 += "MANGO Data Process get some error as below. Please check FICH file or Database.ConfigClass.Database. \r\n \r\n";
                empty2 += strCurrentErr;
                empty2 += " \r\n";
                empty2 += SESSION.MailErrorLog.FooterMessage;
                empty2 = (SESSION.MailErrorLog.sBodyMessage = empty2 + " \r\n \r\n \r\nThis email is auto generated by system, please DON’T REPLY this email \r\nThank You \r\n****** Send By Automail # MANGO # Data notification warning ******");
                clsUtility.Mail.Send(SESSION.parMail, SESSION.MailErrorLog);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                empty = ex.Message;
            }
            return flag;
        }

        private bool SendMissingArticle()
        {
            openConn();
            bool flag = false;
            string text = string.Empty;
            try
            {
                objSlsHeader.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
                ArrayList arrayList = objSlsHeader.FetchListMissingArticle();
                string empty = string.Empty;
                empty = SESSION.MailArticle.HeaderMessage + " \r\n \r\n";
                empty = empty + "No" + clsUtility.ManipulateString.EmptySpace(12) + "Article" + clsUtility.ManipulateString.EmptySpace(2) + " \r\n";
                for (int i = 0; i < arrayList.Count; i++)
                {
                    zMissingArticle zMissingArticle2 = (zMissingArticle)arrayList[i];
                    empty = empty + clsUtility.ManipulateString.Format(" ", 6, (i + 1).ToString()) + clsUtility.ManipulateString.EmptySpace(4) + clsUtility.ManipulateString.Format(" ", 2, zMissingArticle2.Article) + clsUtility.ManipulateString.EmptySpace(4) + " \r\n";
                }
                empty += SESSION.MailArticle.FooterMessage;
                empty = (SESSION.MailArticle.sBodyMessage = empty + " \r\n \r\n \r\nThis email is auto generated by system, please DON’T REPLY this email \r\nThank You \r\n****** Send By Automail # MANGO # Data notification warning ******");
                clsUtility.Mail.Send(SESSION.parMail, SESSION.MailArticle);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                text = ex.Message;
            }
            finally
            {
                closeConn();
                if (flag)
                {
                    p_WriteLog(" Mailing Missing Article or PLU in HOST : SUCCESS", ClearLog: false);
                }
                else if (text != string.Empty)
                {
                    p_WriteLog(" Mailing Missing Article or PLU in HOST : FAILED", ClearLog: false);
                    p_WriteLog(" ERROR : " + text, ClearLog: false);
                }
                else
                {
                    p_WriteLog(" All Article(s) or PLU(s) are Complete in HOST", ClearLog: false);
                }
            }
            return flag;
        }

        private bool SendTrxDocsPending()
        {
            openConn();
            bool flag = false;
            string text = string.Empty;
            try
            {
                objSlsHeader.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
                ArrayList arrayList = objSlsHeader.FetchListTrxDocsPending();
                string empty = string.Empty;
                empty = SESSION.MailMovement.HeaderMessage + " \r\n \r\n";
                empty = empty + "No" + clsUtility.ManipulateString.EmptySpace(7) + "Store" + clsUtility.ManipulateString.EmptySpace(6) + "Trx-Type" + clsUtility.ManipulateString.EmptySpace(6) + "Trx-Date" + clsUtility.ManipulateString.EmptySpace(10) + "Docs-No." + clsUtility.ManipulateString.EmptySpace(8) + "Total" + clsUtility.ManipulateString.EmptySpace(8) + "Remark" + clsUtility.ManipulateString.EmptySpace(4) + " \r\n";
                for (int i = 0; i < arrayList.Count; i++)
                {
                    zTrxDocsPending zTrxDocsPending2 = (zTrxDocsPending)arrayList[i];
                    empty = empty + clsUtility.ManipulateString.Format(" ", 6, (i + 1).ToString()) + clsUtility.ManipulateString.EmptySpace(1) + clsUtility.ManipulateString.Format(" ", 10, zTrxDocsPending2.Store) + clsUtility.ManipulateString.EmptySpace(2) + clsUtility.ManipulateString.Format(" ", 10, zTrxDocsPending2.TrxType) + clsUtility.ManipulateString.EmptySpace(12) + clsUtility.ManipulateString.Format(" ", 8, zTrxDocsPending2.TrxDate.ToString("yyyy-MM-dd")) + clsUtility.ManipulateString.EmptySpace(8) + clsUtility.ManipulateString.Format(" ", 8, zTrxDocsPending2.TrxDoc) + clsUtility.ManipulateString.EmptySpace(3) + clsUtility.ManipulateString.Format(" ", 11, zTrxDocsPending2.TotalTrx.ToString("#,##0")) + clsUtility.ManipulateString.EmptySpace(6) + clsUtility.ManipulateString.Format(" ", 10, zTrxDocsPending2.Remark) + clsUtility.ManipulateString.EmptySpace(4) + " \r\n";
                }
                empty += SESSION.MailMovement.FooterMessage;
                empty = (SESSION.MailMovement.sBodyMessage = empty + " \r\n \r\n \r\nThis email is auto generated by system, please DON’T REPLY this email \r\nThank You \r\n******Send By Automail # MANGO # Data notification warning ******");
                clsUtility.Mail.Send(SESSION.parMail, SESSION.MailMovement);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                text = ex.Message;
            }
            finally
            {
                closeConn();
                if (flag)
                {
                    p_WriteLog(" Mailing ending Sales or Movements in HOST : SUCCESS", ClearLog: false);
                }
                else if (text != string.Empty)
                {
                    p_WriteLog(" Mailing Pending Sales or Movements in HOST : FAILED", ClearLog: false);
                    p_WriteLog(" ERROR : " + text, ClearLog: false);
                }
                else
                {
                    p_WriteLog(" No Sales Or Movements pending in HOST", ClearLog: false);
                }
            }
            return flag;
        }

        private bool SendSalesPending()
        {
            openConn();
            bool flag = false;
            string text = string.Empty;
            try
            {
                objSlsHeader.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
                objSlsDetail.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
                objSlsPayment.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
                DateTime dateTime = objSlsHeader.FetchDateServer();
                ArrayList arrayList = objSlsHeader.FetchSalesPending(dateTime.AddDays(-31.0).ToString("yyyyMMdd"));
                string empty = string.Empty;
                empty = SESSION.MailSales.HeaderMessage + " \r\n \r\n";
                empty = empty + "Report Date : " + dateTime.AddDays(0.0).ToString("yyyy-MM-dd") + " \r\n";
                empty = empty + "No" + clsUtility.ManipulateString.EmptySpace(4) + "Sales Date" + clsUtility.ManipulateString.EmptySpace(8) + "Store" + clsUtility.ManipulateString.EmptySpace(4) + "FICH-Sales" + clsUtility.ManipulateString.EmptySpace(4) + "FICH-Pending" + clsUtility.ManipulateString.EmptySpace(4) + "ISS-Sales" + clsUtility.ManipulateString.EmptySpace(4) + " \r\n";
                for (int i = 0; i < arrayList.Count; i++)
                {
                    zSalesPending zSalesPending2 = (zSalesPending)arrayList[i];
                    empty = empty + clsUtility.ManipulateString.Format(" ", 6, (i + 1).ToString()) + clsUtility.ManipulateString.EmptySpace(3) + clsUtility.ManipulateString.Format(" ", 8, zSalesPending2.TrxDate.ToString("yyyy-MM-dd")) + clsUtility.ManipulateString.EmptySpace(1) + clsUtility.ManipulateString.Format(" ", 10, zSalesPending2.Store) + clsUtility.ManipulateString.EmptySpace(3) + clsUtility.ManipulateString.Format(" ", 13, zSalesPending2.FICHSales.ToString("#,##0")) + clsUtility.ManipulateString.EmptySpace(4) + clsUtility.ManipulateString.Format(" ", 13, zSalesPending2.PendingFICHSales.ToString("#,##0")) + clsUtility.ManipulateString.EmptySpace(9) + clsUtility.ManipulateString.Format(" ", 13, zSalesPending2.ISSSales.ToString("#,##0")) + clsUtility.ManipulateString.EmptySpace(4) + " \r\n";
                }
                empty += SESSION.MailSales.FooterMessage;
                empty = (SESSION.MailSales.sBodyMessage = empty + " \r\n \r\n \r\nThis email is auto generated by system, please DON’T REPLY this email \r\nThanks You \r\n****** Send By Automail # MANGO # Data notification warning ******");
                clsUtility.Mail.Send(SESSION.parMail, SESSION.MailSales);
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                text = ex.Message;
            }
            finally
            {
                closeConn();
                if (flag)
                {
                    p_WriteLog(" Mailing Sales Pending in MNGFICH and HOST  : SUCCESS", ClearLog: false);
                }
                else
                {
                    p_WriteLog(" Mailing Sales Pending in MNGFICH and HOST : FAILED", ClearLog: false);
                    p_WriteLog(" ERROR : " + text, ClearLog: false);
                }
            }
            return flag;
        }

        private bool CheckVersion()
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName name = executingAssembly.GetName();
            Version version = name.Version;
            openConn();
            objSlsHeader.AssignConnection(CurrentDatabase.CurrentConnection.ThrowConnection());
            return true;
        }


        public void timer1_Tick(SystemConfig systemConfig)
        {

            if (!Directory.Exists(systemConfig.PathJob + "\\Log\\"))
            {
                Directory.CreateDirectory(systemConfig.PathJob + "\\Log\\");
            }
            SESSION.LogDate = DateTime.Now.Date.ToString("yyyyMMdd") + " " + DateTime.Now.TimeOfDay.ToString().Substring(0, 8).Replace(":", "");
            SESSION.FileLog = systemConfig.PathJob + "\\Log\\" + SESSION.LogDate + ".log";
            objErrWriter = new StreamWriter(SESSION.FileLog);
            try
            {
                if (!clsUtility.ProcessesRunning(Process.GetCurrentProcess().ProcessName))
                {
                    p_WriteLog(" ...::: Start MANGO FICH Converter Application :::..", ClearLog: true);
                    ReadConfig();
                    setInit();
                    if (!CheckVersion())
                    {
                        p_WriteLog("Your Application is out of date, please call IT Support to install new version", ClearLog: false);
                        return;
                    }
                    Run();
                    SendMissingArticle();
                    SendTrxDocsPending();
                    SendSalesPending();
                    p_WriteLog(" ...::: End Application :::..", ClearLog: false);
                }
                else
                {
                    p_WriteLog(" This application is still running, close before re-run this application", ClearLog: true);
                }
            }
            catch (Exception ex)
            {
                p_WriteLog(" ERROR : " + ex.Message, ClearLog: false);
            }
            finally
            {
                clsUtility.clsFolder.ClearTempFolder();
                objErrWriter.Write(strErr);
                objErrWriter.Close();
                Environment.Exit(-1);
            }
        }


        private bool p_Unzip(string sourcefolder, string destinationfolder)
        {
            bool flag = false;
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                processStartInfo.FileName = (ConfigDat.UnZipperLoc = clsUtility.ReadXML("UnZipperLoc"));
                processStartInfo.Arguments = "x \"" + sourcefolder + "\" -o" + destinationfolder;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public void openConn()
        {
            currentString = new ConnectionStringSettings("INDITEX", "Server=" + DB_IDENTITY.Server + ";Database=" + DB_IDENTITY.Database + ";User ID=" + DB_IDENTITY.User + ";Password=" + DB_IDENTITY.Password + ";Trusted_Connection=False;");
            if (CurrentDatabase.CurrentConnection == null)
            {
                CurrentDatabase.CurrentConnection = new Database.ConfigClass.Database.DBConnection();
            }
            CurrentDatabase.CurrentConnection.CreateConnection(currentString);
        }

        private void closeConn()
        {
            CurrentDatabase.CurrentConnection.CloseConnection();
        }
    }

}

