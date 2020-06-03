using System;
using System.Linq;
using static CommonServices.ExcelConversionService.TablesModel;
using TheReviewer.Website.Services.Common;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CommonServices
{
    public partial class ExcelConversionService
    {
        public class ExcelSupport
        {
            public TableModel ExcelTableLoader(MemoryStream inMemoryCopy, string TableName)
            {
                TableModel LoadedTable = new TableModel();
                //MemoryStream inMemoryCopy = new MemoryStream();
                try
                {
                    TablesModel Tables = new TablesModel();
                    ExcelPackage excelPackage = new ExcelPackage(inMemoryCopy);
                    var package = excelPackage;
                    Tables = Parse(package, false);
                    LoadedTable = FetchTableByName(Tables, TableName);
                    var count = LoadedTable.Body.Count;
                    Debug.WriteLine("Row Count: " + count);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
                return LoadedTable;
            }
            public TableModel ExcelTableLoader(string ExcelPath, string TableName)
            {
                TableModel LoadedTable = new TableModel();

                MemoryStream inMemoryCopy = new MemoryStream();
                try
                {
                    using (FileStream fs = File.OpenRead(ExcelPath))
                    {
                        fs.CopyTo(inMemoryCopy);
                    }
                    TablesModel Tables = new TablesModel();
                    ExcelPackage excelPackage = new ExcelPackage(inMemoryCopy);
                    var package = excelPackage;
                    Tables = Parse(package, false);
                    LoadedTable = FetchTableByName(Tables, TableName);
                    var count = LoadedTable.Body.Count;
                    Debug.WriteLine("Row Count: " + count);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
                return LoadedTable;
            }
            public RowsModel ExcelTableLoader(string ExcelPath, string TableName, bool RowModelFlag)
            {
                RowsModel LoadedRowTable = new RowsModel();

                MemoryStream inMemoryCopy = new MemoryStream();
                try
                {
                    using (FileStream fs = File.OpenRead(ExcelPath))
                    {
                        fs.CopyTo(inMemoryCopy);
                    }
                    TablesModel Tables = new TablesModel();
                    ExcelPackage excelPackage = new ExcelPackage(inMemoryCopy);
                    var package = excelPackage;
                    Tables = Parse(package, false);
                    var Result = FetchTableByName(Tables, TableName);
                    RowsModel rowsModel = new RowsModel();
                    foreach (var row in Result.Body)
                    {
                        RowsModel.RowModel row2 = new RowsModel.RowModel();
                        int cell2 = 0;
                        foreach (var cell in row.Cells)
                        {
                            row2.Cells[cell2].Value = cell.Value;

                            cell2 += 1;
                        }
                        rowsModel.Rows.Add(row2);
                    }
                    LoadedRowTable = rowsModel;
                    var count = LoadedRowTable.Rows.Count;
                    Debug.WriteLine("Row Count: " + count);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
                return LoadedRowTable;
            }
            public static RowModel LocateRow(TableModel table, string SearchWord)
            {
                foreach (RowModel row in table.Body)
                {
                    if (row.Cells[0].Value == SearchWord)
                    {
                        return row;
                    }
                }
                return null;
            }
            public static TableModel FetchTableByName(TablesModel Tables, string tableName)
            {
                foreach (var table in Tables.Tables)
                {
                    if (table.Name == tableName)
                    {
                        return table;
                    }
                }
                return null;
            }
            public static int FindRowByCellContent(TableModel table, string SearchWord, int Col, int Row)
            {
                for (int i = Row; i < table.Body.Count(); i++)
                {
                    string cellValue = FetchCellFromRow(table.Body[i], Col);
                    if (cellValue == SearchWord)
                    {
                        return i;
                    }
                }
                return 0;
            }
            public static bool CheckRowByCellContent(TableModel table, string SearchWord, int Col, int rowNumber)
            {

                string cellValue = FetchCellFromRow(table.Body[rowNumber], Col);
                if (cellValue == SearchWord)
                {
                    return true;
                }
                return false;
            }
            public static RowModel ReadRow(TableModel table, int Row)
            {
                for (int i = Row; i < table.Body.Count(); i++)
                {
                    if ((i == Row))
                    // if ((i == Row) && (EmptyCellChecker(table, Row, 0) == false))
                    {
                        return table.Body[Row];
                    }
                }
                return null;
            }
            public static RowModel ReadRow(TableModel table, int Row, int Col)
            {
                for (int i = Row; i < table.Body.Count(); i++)
                {
                    if ((i == Row))
                    // if ((i == Row) && (EmptyCellChecker(table, Row, 0) == false))
                    {
                        return table.Body[Row];
                    }
                }
                return null;
            }
            public static RowModel SizeRow(RowModel Row, int Size, int StartCol)
            {
                RowModel retRow = new RowModel();
                for (int i = StartCol; i < (StartCol + Size); i++)
                {
                    string cellValue = Row.Cells[i].Value;
                    CellModel cellModel = new CellModel();
                    cellModel.Value = cellValue;
                    retRow.Cells.Add(cellModel);
                }
                return retRow;
            }
            public static RowModel CellTyper(RowModel Row)
            {
                RowModel retRow = new RowModel();
                foreach (var cell in Row.Cells)
                {
                    CellModel cellModel = new CellModel();
                    if (CommonUtils.IsNumeric(cell.Value.ToString()))
                    {
                        cellModel.Value = CommonUtils.DeciString(cell.Value.ToString());
                    }
                    else
                    {
                        cellModel.Value = cell.Value;
                    }
                    retRow.Cells.Add(cellModel);
                }
                return retRow;
            }
            public static bool EmptyCellChecker(TableModel table, int Row, int Col)
            {
                bool result = string.IsNullOrEmpty(table.Body[Row].Cells[Col].Value);
                return result;
            }
            public static bool EmptyCellChecker(RowModel Row, int Col)
            {
                bool result = string.IsNullOrEmpty(Row.Cells[Col].Value);
                return result;
            }
            public static RowModel GetRow(TableModel table, int RowNumber)
            {
                int rowCount = 1;
                foreach (var row in table.Body)
                {
                    if (rowCount == RowNumber)
                    {
                        return row;
                    }
                    rowCount += 1;

                }
                return null;
            }
            public static CellModel GetCell(RowModel row, int cellNumber)
            {
                int cellCount = 1;
                foreach (var cell in row.Cells)
                {
                    if (cellCount == cellNumber)
                    {
                        return cell;
                    }
                    cellCount += 1;
                }
                return null;
            }
            public static bool SaveExcelFile(TableModel tableModel, string fileName, string path)
            {
                string fileDate = CommonUtils.FileDater();
                string filePath = "D:\\Data\\" + fileName + fileDate + ".xlsx";
                var newFile = new FileInfo(filePath);
                using ExcelPackage package = new ExcelPackage(newFile);
                ExcelWorksheet excelWorksheet = package.Workbook.Worksheets.Add(tableModel.Name);
                CustomerStyler(excelWorksheet);
                HeaderStyler(excelWorksheet);
                FooterStyler(excelWorksheet);
                excelWorksheet = AddHeader(tableModel, excelWorksheet);
                int rowCounter = 2;
                foreach (var row in tableModel.Body)
                {
                    int colCounter = 1;
                    foreach (var cell in row.Cells)
                    {
                        if (colCounter > 1)
                        {
                            excelWorksheet.Cells[rowCounter, colCounter].Value = CommonUtils.DoubleConfromString(CellDeNuller(cell).Value);
                        }
                        else
                        {
                            excelWorksheet.Cells[rowCounter, colCounter].Value = CellDeNuller(cell).Value;
                            excelWorksheet.Cells[rowCounter, colCounter].StyleName = "Customer";
                        }
                        Debug.WriteLine("Column:   " + colCounter);
                        colCounter = colCounter + 1;
                    }
                    rowCounter = rowCounter + 1;
                    Debug.WriteLine("Row:    " + rowCounter);
                }
                if (fileName == "Coge")
                {
                    excelWorksheet = AddOther(tableModel, excelWorksheet, rowCounter);
                    rowCounter = rowCounter + 1;
                }
                if (tableModel.Footer != null)
                {
                    excelWorksheet = AddFooter(tableModel, excelWorksheet, rowCounter );
                }
                int i;
                for(i = 1; i <= excelWorksheet.Dimension.End.Column; i++) { excelWorksheet.Column(i).AutoFit(); }
                package.Save();
                return true;
            }

            private static void HeaderStyler(ExcelWorksheet excelWorksheet)
            {
                var namedStyle = excelWorksheet.Workbook.Styles.CreateNamedStyle("Header");
                namedStyle.Style.Font.UnderLine = false;
                namedStyle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                namedStyle.Style.Font.Color.SetColor(Color.White);
                namedStyle.Style.Font.Bold = true;
                namedStyle.Style.Fill.BackgroundColor.SetColor(Color.Black);
                namedStyle.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
            private static void CustomerStyler(ExcelWorksheet excelWorksheet)
            {
                var namedStyle = excelWorksheet.Workbook.Styles.CreateNamedStyle("Customer");
                namedStyle.Style.Font.UnderLine = false;
                namedStyle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                namedStyle.Style.Font.Color.SetColor(Color.Black);
                namedStyle.Style.Font.Bold = false;
                namedStyle.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
            }
            private static void FooterStyler(ExcelWorksheet excelWorksheet)
            {
                var namedStyle = excelWorksheet.Workbook.Styles.CreateNamedStyle("Footer");
                namedStyle.Style.Font.UnderLine = false;
                namedStyle.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                namedStyle.Style.Font.Color.SetColor(Color.White);
                namedStyle.Style.Font.Bold = false;
                namedStyle.Style.Fill.BackgroundColor.SetColor(Color.Teal);
            }
            private static ExcelWorksheet AddHeader(TableModel tableModel, ExcelWorksheet excelWorksheet)
            {
                int colCounter = 1;
                RowModel row = tableModel.Header;
                foreach (var cell in row.Cells)
                {
                    excelWorksheet.Cells[1, colCounter].StyleName = "Header";
                    excelWorksheet.Cells[1, colCounter].Value = CellDeNuller(cell).Value;
                    colCounter = colCounter + 1;
                }

                return excelWorksheet;
            }
            private static ExcelWorksheet AddOther(TableModel tableModel, ExcelWorksheet excelWorksheet,int rowCounter)
            {
                int colCounter = 1;
                RowModel row = tableModel.Other;
                foreach (var cell in row.Cells)
                {
                    if (colCounter > 1)
                    {
                        excelWorksheet.Cells[rowCounter, colCounter].Value = CommonUtils.DoubleConfromString(CellDeNuller(cell).Value);
                    }
                    else
                    {
                        excelWorksheet.Cells[rowCounter, colCounter].Value = CellDeNuller(cell).Value;
                    }
                    colCounter = colCounter + 1;
                }
                return excelWorksheet;
            }
            private static ExcelWorksheet AddFooter(TableModel tableModel, ExcelWorksheet excelWorksheet, int rowCounter)
            {
                int colCounter = 1;
                RowModel row = tableModel.Footer;
                foreach (var cell in row.Cells)
                {
                    if (colCounter > 1)
                    {
                        excelWorksheet.Cells[rowCounter, colCounter].StyleName = "Footer";
                        excelWorksheet.Cells[rowCounter, colCounter].Value = CommonUtils.DoubleConfromString(CellDeNuller(cell).Value);
                    }
                    else
                    {
                        excelWorksheet.Cells[rowCounter, colCounter].StyleName = "Footer";
                        excelWorksheet.Cells[rowCounter, colCounter].Value = CellDeNuller(cell).Value;
                    }
                    colCounter = colCounter + 1;
                }
                return excelWorksheet;
            }

            private static CellModel CellDeNuller(CellModel celltobechecked)
            {
                if (celltobechecked.Value == null)
                {
                    celltobechecked.Value = "0";
                    return celltobechecked;
                }
                else
                {
                    return celltobechecked;
                }
            }
            private static string FetchCellFromRow(RowModel row, int col)
            {
                for (int i = 0; i < row.Cells.Count(); i++)
                {
                    CellModel Cell = row.Cells[i];
                    if (i == col)
                    {
                        return Cell.Value;
                    }
                }
                return " ";
            }
        }
    }
}

