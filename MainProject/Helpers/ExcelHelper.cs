using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IISHOSTV1.Helpers
{
    public class ExcelHelper
    {

        public static ISheet GetFileStream(string fullFilePath)
        {
            var fileExtension = Path.GetExtension(fullFilePath);
            string sheetName;
            ISheet sheet = null;
            switch (fileExtension)
            {
                case ".xlsx":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new XSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (XSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
                case ".xls":
                    using (var fs = new FileStream(fullFilePath, FileMode.Open, FileAccess.Read))
                    {
                        var wb = new HSSFWorkbook(fs);
                        sheetName = wb.GetSheetAt(0).SheetName;
                        sheet = (HSSFSheet)wb.GetSheet(sheetName);
                    }
                    break;
            }
            return sheet;
        }
        public static void GenerateExcelFile(String PathName, DataTable DT)
        {
            // Below code is create datatable and add one row into datatable.  

            // Declare HSSFWorkbook object for create sheet  
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet1");

            var headerRow = sheet.CreateRow(0);

            //Below loop is create header  
            for (int i = 0; i < DT.Columns.Count; i++)
            {
                var cell = headerRow.CreateCell(i);
                cell.SetCellValue(DT.Columns[i].ColumnName.ToUpper());
            }

            //Below loop is fill content  
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                var rowIndex = i + 1;
                var row = sheet.CreateRow(rowIndex);

                for (int j = 0; j < DT.Columns.Count; j++)
                {

                    var cell = row.CreateCell(j);
                    var Data = DT.Rows[i][j];
                    DateTime temp;
                    if (DateTime.TryParse(Data.ToString(), out temp))
                    {
                        cell.SetCellValue(Convert.ToDateTime(Data.ToString()).ToString("dd-MMM-yyyy"));
                    }
                    else
                    {
                        cell.SetCellValue(Data.ToString());
                    }


                }
            }

            // Declare one MemoryStream variable for write file in stream  
            var stream = new MemoryStream();
            workbook.Write(stream);
            //return stream;
            string FilePath = PathName;

            //Write to file using file stream  
            FileStream file = new FileStream(FilePath, FileMode.CreateNew, FileAccess.Write);
            stream.WriteTo(file);
            file.Close();
            stream.Close();
        }
        public static Boolean ContainColumn(string columnName, DataTable table)
        {
            Boolean Result = false;
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                Result = true;
            }

            return Result;
        }
        public static DataTable GetRequestsDataFromExcel(string fullFilePath)
        {
            try
            {
                var sh = GetFileStream(fullFilePath);
                var dtExcelTable = new DataTable();
                dtExcelTable.Rows.Clear();
                dtExcelTable.Columns.Clear();
                var headerRow = sh.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (var c = 0; c < colCount; c++)
                    dtExcelTable.Columns.Add(headerRow.GetCell(c).ToString());
                var i = 1;
                var currentRow = sh.GetRow(i);
                while (currentRow != null)
                {
                    var dr = dtExcelTable.NewRow();
                    for (var j = 0; j < colCount; j++)
                    {
                        var cell = currentRow.GetCell(j);

                        if (cell != null)
                            switch (cell.CellType)
                            {
                                case CellType.Numeric:
                                    dr[j] = DateUtil.IsCellDateFormatted(cell)
                                        //? cell.DateCellValue.ToString(CultureInfo.InvariantCulture)
                                        ? cell.DateCellValue.ToString("yyyy-MM-dd")
                                        : cell.NumericCellValue.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                    break;
                                case CellType.String:
                                    dr[j] = cell.StringCellValue;
                                    break;
                                case CellType.Blank:
                                    dr[j] = string.Empty;
                                    break;
                            }
                    }
                    dtExcelTable.Rows.Add(dr);
                    i++;
                    currentRow = sh.GetRow(i);
                }
                return dtExcelTable;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}