using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugins.Interfaces
{
    public interface IExcelServices
    {
        public DataTable ReadRange(string excelFilePath, bool isFirstRowHeader, string sheetName, string? startCell = null);
        public void WriteRange(string excelFilePath, DataTable table, string? sheetName=null, string? startCell=null);
    }
}
