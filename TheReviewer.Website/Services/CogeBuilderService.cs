using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static CommonServices.ExcelConversionService.TablesModel;
using static CommonServices.ExcelConversionService;

namespace TheReviewer.Website.Services
{
    public static class CogeBuilderService
    {
        private static int _customerRowStart = 0;
        private static int _customerRowEnd = 0;
        public static TableModel CogeFetcher(string path)
        {
            ExcelSupport excelSupport = new ExcelSupport();
            TableModel CogeTable = new TableModel();
            TableModel CogeDisplayTable = new TableModel();
            bool filedecoded;
            try
            {
                CogeTable = excelSupport.ExcelTableLoader(path, "REV_CUST");
                {
                    Console.WriteLine("File Loaded");
                }
                CogeDisplayTable = BuildCoge(CogeTable);
                if (CogeDisplayTable == null)
                {
                    filedecoded = false;
                    return null;
                }
                filedecoded = true;
                Console.WriteLine("File Decoded");
                return CogeDisplayTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public static TableModel CogeFetcher(MemoryStream memoryStream)
        {
            ExcelSupport excelSupport = new ExcelSupport();
            TableModel CogeTable = new TableModel();
            TableModel CogeOut = new TableModel();
            TableModel CogeDisplayTable = new TableModel();
            RowsModel rowsModel = new RowsModel();
            try
            {
                CogeTable = excelSupport.ExcelTableLoader(memoryStream, "REV_CUST");
                Console.WriteLine("Coge File Loaded");
                CogeDisplayTable = BuildCoge(CogeTable);
                Console.WriteLine("File Decoded");
                CogeOut = SortTable(CogeDisplayTable);
                CogeOut.Header = CogeDisplayTable.Header;
                CogeOut.Other = CogeDisplayTable.Other;
                CogeOut.Footer = CogeDisplayTable.Footer;
                return CogeOut;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public static TableModel BuildCoge(TableModel table)
        {
            TableModel cogeTable = new TableModel();
            _customerRowStart = ExcelSupport.FindRowByCellContent(table, "CUSTOMERS", 0, 0);
            _customerRowEnd = ExcelSupport.FindRowByCellContent(table, "TOTAL", 0, _customerRowStart);
            try
            {
                for (int i = _customerRowStart; i < _customerRowEnd + 1; i++)
                {
                    var CogeRow = ExcelSupport.ReadRow(table, i, 13);
                    CogeRow = ExcelSupport.SizeRow(CogeRow, 14, 0);
                    CogeRow = ExcelSupport.CellTyper(CogeRow);
                    Debug.WriteLine("Index = " + i);
                    switch (ExcelSupport.EmptyCellChecker(CogeRow, 0))
                    {
                        case true:
                            break;
                        default:
                            if (CogeRow.Cells[0].Value == "CUSTOMERS")
                            {
                                cogeTable.Header = CogeRow;
                                break;
                            }
                            else if ((CogeRow.Cells[0].Value).Contains("TOTAL"))
                            {
                                cogeTable.Footer = CogeRow;
                                break;
                            }
                            else if (CogeRow.Cells[0].Value == "Other customers")
                            {
                                cogeTable.Other = CogeRow;
                                break;
                            }
                            else if ((CogeRow.Cells[0].Value).Contains("Customer "))
                            {
                                break;
                            }
                            else if ((CogeRow.Cells[0].Value).Contains("Other customers, "))
                            {
                                break;
                            }
                            else if ((CogeRow.Cells[0].Value).Contains("ADJUSTMENTS"))
                            {
                                break;
                            }
                            else
                            {
                                cogeTable.Body.Add(CogeRow);
                                break;
                            }
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }

            return cogeTable;
        }
        public static RowModel GetMonthHeader(TableModel table)
        {
            var row = table.Body[0];
            return row != null ? row : null;
        }
        public static TableModel SortTable(TableModel table)
        {
            TableModel table1 = new TableModel();
            IList<RowModel> rowsModel;
            RowModel BottomRow = new RowModel();
            RowModel OtherRow = new RowModel();
            rowsModel = table.Body;
            table1.Body = rowsModel.OrderBy(test => test.Cells[0].Value).ToList();
            table1.Header = table.Header;
            table1.Footer = table.Footer;
            return table1;
        }
        public static bool SaveCogeExcelFile(TableModel tableModel)
        {
            if (ExcelSupport.SaveExcelFile(tableModel, "Coge","D:\\Data\\"))
            {
                return true;
            }
            return false;
        }

    }

    public class CogeReturn
    {
        public RowModel cogeRowModel { get; set; }
        public string Status { get; set; }
    }
}

