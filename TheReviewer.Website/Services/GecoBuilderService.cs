using System;
using System.Collections.Generic;
using System.IO;
using static CommonServices.ExcelConversionService.TablesModel;
using System.Diagnostics;
using static CommonServices.ExcelConversionService;
using TheReviewer.Website.Services.Common;

namespace TheReviewer.Website.Services
{
    public static class GecoBuilderService
    {
        private static readonly int _GecoStartCol = 5;
        private static readonly int _GecoStartRow = 20;
        private static readonly int _GecoMonthHeader = 18;
        private static int _CustomerRowEnd = 0;
        private static GecoPosition gecoPositions;
        public static TableModel GecoFetcher(string path)
        {
            ExcelSupport excelSupport = new ExcelSupport();
            TableModel GecoTable = new TableModel();
            TableModel GecoDisplayTable = new TableModel();
            RowsModel rowsModel = new RowsModel();
            try
            {
                GecoTable = excelSupport.ExcelTableLoader(path, "Profit & Loss");
                rowsModel = excelSupport.ExcelTableLoader(path, "Profit & Loss", true);
                Console.WriteLine("Geco File Loaded");
                GecoDisplayTable = BuildGeco(GecoTable);
                Console.WriteLine("File Decoded");
                return GecoDisplayTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public static TableModel GecoFetcher(MemoryStream memoryStream)
        {
            RowsModel RowsSorted = new RowsModel();
            RowModel rowModel = new RowModel();
            ExcelSupport excelSupport = new ExcelSupport();
            TableModel GecoTable = new TableModel();
            TableModel GecoDisplayTable = new TableModel();
            //IList<RowModel> rowsModel;
            try
            {
                GecoTable = excelSupport.ExcelTableLoader(memoryStream, "Profit & Loss");
                Console.WriteLine("Geco File Loaded");
                GecoDisplayTable = BuildGeco(GecoTable);
                Console.WriteLine("File Decoded");
                return GecoDisplayTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public static TableModel GecoCogeTotalMerger(TableModel gecoDisplayTable, TableModel cogeDisplayTable)
        {
            TableModel TotalDisplayTable = new TableModel();
            RowModel headerRow = GetMonthHeader(gecoDisplayTable);
            TotalDisplayTable.Body.Add(headerRow);
            foreach (var row in gecoDisplayTable.Body)
            {
                RowModel GecoRow = new RowModel();
                if (row.Cells[0].Value == "Total")
                {
                    Console.WriteLine("Done");
                    GecoRow = row;
                    TotalDisplayTable.Body.Add(GecoRow);
                }
            }
            foreach (var row in cogeDisplayTable.Body)
            {
                RowModel CogeRow = new RowModel();
                if (row.Cells[0].Value == "TOTAL")
                {
                    Console.WriteLine("Done");
                    CogeRow = row;
                    TotalDisplayTable.Body.Add(CogeRow);
                }
            }
            return TotalDisplayTable;
        }
        public static TableModel BuildGeco(TableModel table)
        {
            List<GecoPosition> PostionsGeco = DetermineCustomerTotalPositions(table);
            TableModel GecoGatherTable = GecoGather(PostionsGeco, table);
            return GecoGatherTable;
        }
        public static RowModel BuildGecoHeader(TableModel table)
        {
            RowModel rowModel = GetHeaderRow(table);
            return rowModel != null ? rowModel : null;
        }
        public static RowModel BuildGecoRow(TableModel table)
        {
            RowModel rowModel = GetRowData(table);
            return rowModel != null ? rowModel : null;
        }
        public static RowModel GetMonthHeader(TableModel table)
        {
            var row = table.Body[0];
            return row != null ? row : null;
        }
        private static List<GecoPosition> DetermineCustomerTotalPositions(TableModel table)
        {
            List<GecoPosition> GecoPostions = new List<GecoPosition>();
            _CustomerRowEnd = ExcelSupport.FindRowByCellContent(table, "Total", 0, _GecoStartRow);
            Debug.WriteLine("Customer End Row :" + _CustomerRowEnd);

            for (int i = _GecoStartRow; i < _CustomerRowEnd + 1; i++)
            {
                GecoPosition gecoPosition = new GecoPosition();
                var GecoRow = ExcelSupport.ReadRow(table, i);
                GecoRow = ExcelSupport.CellTyper(GecoRow);
                if (ExcelSupport.EmptyCellChecker(GecoRow, 0))
                {

                }
                else
                {
                    Debug.WriteLine("Customer: " + GecoRow.Cells[0].Value);
                    Debug.WriteLine("Line # :" + i);

                    gecoPosition.CustomerRow = i;
                    gecoPosition.CustomerName = GecoRow.Cells[0].Value;

                    gecoPosition.TotalRow = ExcelSupport.FindRowByCellContent(table, "Tot.Customer", 1, i);
                    Debug.WriteLine("Cust Total :" + gecoPosition.TotalRow);

                    GecoPostions.Add(gecoPosition);
                    Debug.Write(gecoPosition.CustomerName);
                }
            }
            return GecoPostions;
        }
        private static TableModel GecoGather(List<GecoPosition> gecoPositions, TableModel table)
        {
            RowsModel Rows = new RowsModel();
            TableModel displayTable = new TableModel();
            RowModel headerRow = GetHeaderRow(table);
            //displayTable.Body.Add(headerRow);
            displayTable.Header = headerRow;
            int entityCount = gecoPositions.Count - 1;
            var gecoRow = new RowModel();
            for (int i = 0; i < entityCount; i++)
            {
                var GecoRow = new RowModel();
                GecoRow = ParseGecoRow(table, gecoPositions[i].CustomerName, gecoPositions[i].TotalRow);

                displayTable.Body.Add(GecoRow);
            }

            gecoRow = ParseGecoRow(table, gecoPositions[entityCount].CustomerName, gecoPositions[entityCount].CustomerRow);
            displayTable.Footer = gecoRow;
            return displayTable;
        }
        private static RowModel ParseGecoRow(TableModel table, string CustomerName, int TotalRow)
        {
            int counter = 1;
            RowModel displayRow = new RowModel();
            CellModel cellName = new CellModel();
            cellName.Value = CustomerName;
            displayRow.Cells.Add(cellName);
            for (int i = 6; i < 43; i += 3)
            {
                var Row = ExcelSupport.ReadRow(table, TotalRow);
                CellModel cell = new CellModel();
                if (ExcelSupport.EmptyCellChecker(Row, i))
                {
                    cell.Value = "0";
                }
                else
                {
                    cell.Value = CommonUtils.DeciString(Row.Cells[i].Value);
                    //cell = Common.GetCell(Row, i);
                    Debug.WriteLine(cell.Value);
                }
                counter++;
                displayRow.Cells.Add(cell);
            }
            return displayRow;
        }
        public static bool SaveGecoExcelFile(TableModel tableModel)
        {
            if (ExcelSupport.SaveExcelFile(tableModel,"Geco", "D:\\Data\\"))
            {
                return true;
            }
            return false;
        }

        private static RowModel GetHeaderRow(TableModel table)
        {
            var row = ExcelSupport.GetRow(table, _GecoMonthHeader);
            return row != null ? GetRowGeco(row, true) : null;
        }
        private static RowModel GetRowData(TableModel table)
        {
            var row = ExcelSupport.GetRow(table, _GecoStartRow);
            return row != null ? GetRowGeco(row, false) : null;
        }
        private static RowModel GetRowGeco(RowModel row, bool Header)
        {
            int intervalCount = 0;
            var gecoRow = new RowModel();
            int gecoStartCol = _GecoStartCol;
            for (int i = 0; i < row.Cells.Count; i++)
            {
                CellModel cell = row.Cells[i];
                if (i == 0 && Header == true)
                {
                    var tempCell = "Customers";
                    var pandlCell = new CellModel();
                    pandlCell.Value = tempCell;
                    gecoRow.Cells.Add(pandlCell);
                }
                if (i > gecoStartCol)
                {
                    intervalCount += 1;
                    if (intervalCount == 1)
                    {
                        var pandlCell = new CellModel();
                        var tempCell = cell.Value;
                        if (string.IsNullOrEmpty(tempCell))
                        {
                            tempCell = Header == false ? "0.00" : i == 42 ? "Total" : " ";

                        }
                        else
                        {
                            if (Header == false)
                            {
                                tempCell = CommonUtils.DeciString(tempCell);
                            }
                            else
                            {

                            }
                        }

                        pandlCell.Value = tempCell;
                        gecoRow.Cells.Add(pandlCell);
                    }
                    if (intervalCount == 3)
                    {
                        intervalCount = 0;
                    }
                }
                //colCount += 1;
                if (i == 44)
                {
                    return gecoRow;
                }
            }
            return gecoRow;
        }
    }
    public class GecoReturns
    {
        private TablesModel Tables = new TablesModel();
        private readonly RowModel rowModel = new RowModel();
        public RowModel GecoHeaderRowModel { get; set; }
        public RowModel GecoDataRowModel { get; set; }
        public string Status { get; set; }
    }
    public class GecoPosition
    {
        public string CustomerName { get; set; }
        public int CustomerRow { get; set; }
        public int TotalRow { get; set; }
    }
}

