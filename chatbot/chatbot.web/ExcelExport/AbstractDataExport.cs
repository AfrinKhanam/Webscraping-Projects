using System.Collections.Generic;
using System.IO;

using IndianBank_ChatBOT.Models;

using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace IndianBank_ChatBOT.ExcelExport
{
    public interface IAbstractDataExport
    {
        byte[] Export<T>(List<T> exportData, List<LeadGenerationAction> actions, string from, string to, string sheetName);
    }

    public abstract class AbstractDataExport : IAbstractDataExport
    {
        protected string _sheetName;
        protected string _fileName;
        protected List<string> _headers;
        protected List<string> _type;
        protected IWorkbook _workbook;
        protected ISheet _sheet;
        private const string DefaultSheetName = "Sheet1";

        public byte[] Export<T>(List<T> exportData, List<LeadGenerationAction> actions, string from, string to, string sheetName = DefaultSheetName)
        {
            _sheetName = sheetName;

            _workbook = new XSSFWorkbook();
            _sheet = _workbook.CreateSheet(_sheetName);

            WriteData(exportData, actions, from, to);

            using (var memoryStream = new MemoryStream())
            {
                _workbook.Write(memoryStream);

                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Generic Definition to handle all types of List
        /// </summary>
        /// <param name="exportData"></param>
        public abstract void WriteData<T>(List<T> exportData, List<LeadGenerationAction> actions, string from, string to);

    }
}
