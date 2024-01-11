

using ClosedXML.Excel;

using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using Plugins.Interfaces;

using System.Data;


namespace Plugins
{
    public class ExcelServices : IExcelServices
    {
        public DataTable ReadRange(string excelFilePath, bool isFirstRowHeader, string sheetName, string startCell = null)
        {
            DataTable dataTable = new DataTable();

            IWorkbook workbook;
            FileStream stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read);

            try
            {
                if (excelFilePath.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    workbook = new XSSFWorkbook(stream);
                }
                else
                {
                    // Handle .xls format if needed
                    throw new NotSupportedException("Only .xlsx format is supported.");
                }

                ISheet sheet;

                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = workbook.GetSheet(sheetName);
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }

                // Set the start column and row
                int startColumn = 0;
                int startRow = 0;

                if (!string.IsNullOrEmpty(startCell))
                {
                    var startCellAddress = new CellReference(startCell);
                    startColumn = startCellAddress.Col;
                    startRow = startCellAddress.Row;
                }

                // Process headers
                if (isFirstRowHeader)
                {
                    IRow headerRow = sheet.GetRow(startRow);
                    foreach (ICell headerCell in headerRow.Skip(startColumn))
                    {
                        string columnName = string.IsNullOrEmpty(headerCell.ToString())
                            ? $"Column{dataTable.Columns.Count + 1}"
                            : headerCell.ToString();

                        dataTable.Columns.Add(columnName);
                    }

                    startRow++;
                }
                else
                {
                    // Add columns if not using the first row as headers
                    for (int c = startColumn; c < sheet.GetRow(startRow).LastCellNum; c++)
                    {
                        dataTable.Columns.Add($"Column{c + 1}");
                    }
                }

                // Process rows
                for (int r = startRow; r <= sheet.LastRowNum; r++)
                {
                    IRow row = sheet.GetRow(r);

                    if (row == null)
                    {
                        continue;
                    }

                    DataRow dataRow = dataTable.NewRow();
                    List<string> cells = new List<string>();

                    for (int cellCount = startColumn; cellCount < row.LastCellNum; cellCount++)
                    {
                        ICell cell = row.GetCell(cellCount, MissingCellPolicy.RETURN_NULL_AND_BLANK);

                        if (cell != null)
                        {
                            if (cell.CellType.ToString() == "Formula")
                            {
                                switch (cell.CachedFormulaResultType.ToString().ToLower())
                                {
                                    case "numeric":
                                        cells.Add(cell.DateCellValue.ToString());
                                        break;
                                    case "string":
                                        cells.Add(cell.StringCellValue.ToString());
                                        break;
                                }
                            }
                            else
                            {
                                cells.Add(cell.ToString());
                            }
                        }
                        else
                        {
                            cells.Add("");
                        }
                    }

                    dataRow.ItemArray = cells.ToArray();
                    dataTable.Rows.Add(dataRow);
                }

                stream.Close();
                return dataTable;
            }
            catch (Exception ex)
            {
                stream.Close();
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public void WriteRange(string excelFilePath, DataTable dataTable, string sheetName, string? startCell = null)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);
            // Set the start column and row
            int startColumn = 1;
            int startRow = 1;

            if (!string.IsNullOrEmpty(startCell))
            {
                var startCellAddress = new CellReference(startCell);
                startColumn = startCellAddress.Col;
                startRow = startCellAddress.Row;
            }

            worksheet.Cell(startRow, startColumn).InsertData(dataTable.AsEnumerable(), false) ;
            workbook.SaveAs(excelFilePath);
        }

    }
}
